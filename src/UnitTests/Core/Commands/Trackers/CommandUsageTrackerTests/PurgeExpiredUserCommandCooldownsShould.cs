using System;
using DevChatter.Bot.Core.Commands.Trackers;
using DevChatter.Bot.Core.Events;
using FluentAssertions;
using Xunit;

namespace UnitTests.Core.Commands.Trackers.CommandUsageTrackerTests
{
    public class PurgeExpiredUserCommandCooldownsShould
    {
        [Fact]
        public void DoNothing_GivenNoUsages()
        {
            var (tracker, _, _) = GetTestData(1);

            tracker.PurgeExpiredUserCommandCooldowns(DateTimeOffset.UtcNow);

            var usage = tracker.GetByUserDisplayName(Guid.NewGuid().ToString());

            usage.Should().BeNull();
        }

        [Fact]
        public void PurgeOneUsage_GivenCooldownPassed()
        {
            var cooldown = 1;
            var (tracker, _, time) = GetTestData(cooldown);

            tracker.RecordUsage(new CommandUsage(Guid.NewGuid().ToString(), time));

            tracker.PurgeExpiredUserCommandCooldowns(time.AddSeconds(cooldown));

            var usage = tracker.GetByUserDisplayName(Guid.NewGuid().ToString());

            usage.Should().BeNull();
        }

        [Fact]
        public void PurgeNothing_GivenCooldownNotPassed()
        {
            var (tracker, displayName, time) = GetTestData(1);

            tracker.RecordUsage(new CommandUsage(displayName, time));

            tracker.PurgeExpiredUserCommandCooldowns(time);

            var usage = tracker.GetByUserDisplayName(displayName);

            usage.Should().NotBeNull();
        }

        [Fact]
        public void LeaveUsages_GivenSomeAreNotReadyToPurge()
        {
            var (tracker, displayName, time) = GetTestData(1);

            tracker.RecordUsage(new CommandUsage(displayName, time));
            tracker.RecordUsage(new CommandUsage(displayName, time.AddSeconds(1)));

            tracker.PurgeExpiredUserCommandCooldowns(time);

            var usage = tracker.GetByUserDisplayName(displayName);

            usage.Should().NotBeNull();
        }

        private static (CommandUsageTracker, string, DateTimeOffset) GetTestData(int globalCommandCooldown)
        {
            var commandHandlerSettings = new CommandHandlerSettings
            {
                GlobalCommandCooldown = globalCommandCooldown
            };
            var commandUsageTracker = new CommandUsageTracker(commandHandlerSettings);
            return (commandUsageTracker, Guid.NewGuid().ToString(), DateTimeOffset.UtcNow);
        }
    }
}
