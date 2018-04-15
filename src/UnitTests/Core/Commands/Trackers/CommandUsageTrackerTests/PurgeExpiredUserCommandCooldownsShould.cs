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
            var tracker = GetTestTracker(1);

            tracker.PurgeExpiredUserCommandCooldowns(DateTimeOffset.UtcNow);

            var usage = tracker.GetByUserDisplayName(Guid.NewGuid().ToString());

            usage.Should().BeNull();
        }

        [Fact]
        public void PurgeOneUsage_GivenCooldownPassed()
        {
            const int cooldown = 1;
            var timeInvoked = DateTimeOffset.UtcNow;
            var displayName = Guid.NewGuid().ToString();
            var tracker = GetTestTracker(cooldown);

            tracker.RecordUsage(new CommandUsage(Guid.NewGuid().ToString(), timeInvoked));

            tracker.PurgeExpiredUserCommandCooldowns(timeInvoked.AddSeconds(cooldown));

            var usage = tracker.GetByUserDisplayName(Guid.NewGuid().ToString());

            usage.Should().BeNull();
        }

        [Fact]
        public void PurgeNothing_GivenCooldownNotPassed()
        {
            const int cooldown = 1;
            var timeInvoked = DateTimeOffset.UtcNow;
            var displayName = Guid.NewGuid().ToString();
            var tracker = GetTestTracker(cooldown);

            tracker.RecordUsage(new CommandUsage(displayName, timeInvoked));

            tracker.PurgeExpiredUserCommandCooldowns(timeInvoked);

            var usage = tracker.GetByUserDisplayName(displayName);

            usage.Should().NotBeNull();
        }

        private static CommandUsageTracker GetTestTracker(int globalCommandCooldown)
        {
            var commandHandlerSettings = new CommandHandlerSettings
            {
                GlobalCommandCooldown = globalCommandCooldown
            };
            var commandUsageTracker = new CommandUsageTracker(commandHandlerSettings);
            return commandUsageTracker;
        }
    }
}
