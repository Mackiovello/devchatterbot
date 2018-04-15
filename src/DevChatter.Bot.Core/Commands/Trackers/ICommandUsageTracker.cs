using System;
using System.Collections.Generic;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public interface ICommandUsageTracker
    {
        List<CommandUsage> GetByUserDisplayName(string displayName);
        void PurgeExpiredUserCommandCooldowns(DateTimeOffset currentTime);
        void RecordUsage(CommandUsage commandUsage);
    }
}
