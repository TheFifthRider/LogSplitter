using Vintagestory.API.Client;
using Vintagestory.API.Common;
using System.IO;
using System;
using System.Collections.Generic;
using LogSplitter.Configs;
using Vintagestory.API.Util;

namespace LogSplitter;

public class LogSplitterModSystem : ModSystem
{
    private static string configName = "LogSplitter.json";
    private static LogSplitterConfig config;
    private DateTime loggingStarted;

    public void EntryAddedHandler(EnumLogType logType, string message, params object[] args)
    {
        if (logType == EnumLogType.Chat) {
            int chatId = int.Parse(args[1].ToString());
            if (!config.mutedChats.Contains(chatId)) {
                string chatName = config.serverChats.Get(chatId) ?? $"""UnnamedChat({chatId})""";
                string channelLogPath = Path.Combine(config.chatDirectory,  $"""{loggingStarted:yyyy-mm-dd-HHMM}_{chatName}.md""");
                File.AppendAllLines(channelLogPath, new List<string> { $"""{DateTime.Now:yyyy.mm.dd HH:MM:ss} {args[0].ToString()}""" });
            }
        }
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        LoadConfig(api);
        loggingStarted = DateTime.Now;
        api.Logger.EntryAdded += EntryAddedHandler;
    }

    private void LoadConfig(ICoreClientAPI clientApi)
    {
        try
        {
            config = clientApi.LoadModConfig<LogSplitterConfig>(configName);
        }
        catch (Exception)
        {
            clientApi.Logger.Error("LogSplitter: Failed to load mod config!");
            return;
        }

        if (config == null)
        {
            clientApi.Logger.Notification("LogSplitter: non-existant modconfig at 'ModConfig/" + configName +
                                        "', creating default...");
            config = new LogSplitterConfig();
            clientApi.StoreModConfig(config, configName);
        }
    }

}
