using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Xna.Framework.Net
{
    [Flags]
    public enum GamerStates
    {
        Local = 0x000001,
        Host = 0x000010,
        HasVoice = 0x000100,
        Guest = 0x001000,
        MutedByLocalUser = 0x010000,
        PrivateSlot = 0x100000,
        Ready = 0x1000000,

    }

    internal class CommandGamerJoined : ICommand
    {
        int gamerInternalIndex = -1;
        internal long remoteUniqueIdentifier = -1;
        GamerStates states;
        string gamerTag = string.Empty;
        string displayName = string.Empty;

        public CommandGamerJoined(int internalIndex, bool isHost, bool isLocal)
        {
            gamerInternalIndex = internalIndex;

            if (isHost)
                states = states | GamerStates.Host;
            if (isLocal)
                states = states | GamerStates.Local;

        }

        public CommandGamerJoined(long uniqueIndentifier)
        {
            this.remoteUniqueIdentifier = uniqueIndentifier;

        }

        public string DisplayName
        {
            get
            {
                return displayName;
            }
            set
            {
                displayName = value;
            }
        }

        public string GamerTag
        {
            get
            {
                return gamerTag;
            }
            set
            {
                gamerTag = value;
            }
        }

        public GamerStates State
        {
            get { return states; }
            set { states = value; }
        }

        public int InternalIndex
        {
            get { return gamerInternalIndex; }
        }

        public CommandEventType Command
        {
            get { return CommandEventType.GamerJoined; }
        }
    }
}
