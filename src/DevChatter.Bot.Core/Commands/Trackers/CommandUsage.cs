using System;
using DevChatter.Bot.Core.Commands.Operations;

namespace DevChatter.Bot.Core.Commands.Trackers
{
    public class CommandUsage
    {
        public CommandUsage(string displayName, DateTimeOffset timeInvoked
            , IBotCommand command, ICommandOperation operation = null
            , bool wasUserWarned = false)
        {
            DisplayName = displayName;
            TimeInvoked = timeInvoked;
            Command = command;
            Operation = operation;
            WasUserWarned = wasUserWarned;
        }

        public string DisplayName { get; set; }
        public DateTimeOffset TimeInvoked { get; set; }
        public bool WasUserWarned { get; set; }
        public IBotCommand Command { get; set; }
        public ICommandOperation Operation { get; set; }
    }
}
