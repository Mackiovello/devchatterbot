using System;
using DevChatter.Bot.Core.Data.Model;
using DevChatter.Bot.Core.Events.Args;
using DevChatter.Bot.Core.Systems.Chat;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public class DefaultOperation : ICommandOperation
    {
        private Func<IChatClient, ChatUser, string> _showAllCommands;

        public DefaultOperation(Func<IChatClient, ChatUser, string> showAllCommands)
        {
            _showAllCommands = showAllCommands;
        }

        public int Ordinal { get; } = int.MaxValue;

        public bool ShouldExecute(string operand) => true;

        public string TryToExecute(CommandReceivedEventArgs eventArgs)
        {
            return _showAllCommands();
        }
    }
}
