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
    private ILogger log;

    public void EntryAddedHandler(EnumLogType logType, string message, params object[] args)
    {
        try {
            if (EnumLogType.Chat == logType) {
                if (args is null || args.Length < 2) {
                    log.Debug("logType was of Chat, but args passed in do not look correct. Skipping message.");
                    log.Debug($"Skipped message: (logType={logType}, message={message}, args={args})");
                } else {
                    string chatMessage = args[0].ToString();
                    int chatId = int.Parse(args[1].ToString());
                    
                    if (!config.mutedChats.Contains(chatId)) {
                        string chatName = config.serverChats.Get(chatId) ?? $"""UnnamedChat({chatId})""";
                        string channelLogPath = Path.Combine(config.chatDirectory,  $"""{loggingStarted:yyyy-mm-dd-HHMM}_{chatName}.md""");
                        File.AppendAllLines(channelLogPath, new List<string> { $"""{DateTime.Now:yyyy.mm.dd HH:MM:ss} {chatMessage}""" });
                    }
                }
            }
        } catch(Exception e) {
            log?.Error(e);
        }
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        LoadConfig(api);
        log = api.Logger;
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
