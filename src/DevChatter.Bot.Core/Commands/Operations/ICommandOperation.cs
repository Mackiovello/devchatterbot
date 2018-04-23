using DevChatter.Bot.Core.Events.Args;

namespace DevChatter.Bot.Core.Commands.Operations
{
    public interface ICommandOperation
    {
        int Ordinal { get; }
        bool ShouldExecute(string operand);
        string TryToExecute(CommandReceivedEventArgs eventArgs);
    }
}
