using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework.Net
{
    public enum CommandEventType
    {
        GamerJoined,
        GamerLeft,
        SessionStateChange,
        SendData,
        ReceiveData,
        GamerStateChange,
    }

    internal interface ICommand
    {
        CommandEventType Command { get; }
    }

    internal class CommandEvent
    {
        CommandEventType command;
        object commandObject;

        public CommandEvent(CommandEventType command, object commandObject)
        {
            this.command = command;
            this.commandObject = commandObject;
        }

        public CommandEvent(ICommand command)
        {
            this.command = command.Command;
            this.commandObject = command;
        }

        public CommandEventType Command
        {
            get { return command; }

        }

        public object CommandObject
        {
            get { return commandObject; }
        }
    }
}
