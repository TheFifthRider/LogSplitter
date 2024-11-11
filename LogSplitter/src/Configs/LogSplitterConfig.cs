using System.Collections.Generic;
using Vintagestory.API.Config;
namespace LogSplitter.Configs
{
    public class LogSplitterConfig
    {
        public Dictionary<int, string> serverChats = new Dictionary<int, string>
        {
                { GlobalConstants.GeneralChatGroup, "GeneralChat" },
                { GlobalConstants.ServerInfoChatGroup, "ServerInfo" },
                { GlobalConstants.DamageLogChatGroup, "DamageLog" },
                { GlobalConstants.InfoLogChatGroup, "InfoLog" },
                { GlobalConstants.ConsoleGroup, "Console" }
        };

        public List<int> mutedChats = new List<int>{ GlobalConstants.ServerInfoChatGroup, GlobalConstants.DamageLogChatGroup, GlobalConstants.InfoLogChatGroup, GlobalConstants.ConsoleGroup};

        public string chatDirectory = GamePaths.Logs;

    }
}