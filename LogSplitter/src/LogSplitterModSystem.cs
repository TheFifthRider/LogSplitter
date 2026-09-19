using Vintagestory.API.Client;
using Vintagestory.API.Common;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using LogSplitter.Configs;
using Vintagestory.API.Util;

namespace LogSplitter;

public class LogSplitterModSystem : ModSystem
{
    private static readonly Regex defaultFilter = new(@"<[^>]*>", RegexOptions.Compiled);
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
                    var chatMessage = args[0].ToString();
                    var chatId = int.Parse(args[1].ToString());
                    
                    if (!config.mutedChats.Contains(chatId)) {
                        var chatName = config.serverChats.Get(chatId) ?? $"""UnnamedChat({chatId})""";
                        var channelLogPath = Path.Combine(config.chatDirectory,  $"""{loggingStarted:yyyy-mm-dd-HHMM}_{chatName}.md""");
                        var lineToAppend = formatChatMessage(config, chatMessage);
                        File.AppendAllLines(channelLogPath, new List<string> { lineToAppend });
                    }
                }
            }
        } catch(Exception e) {
            log?.Error(e);
        }
    }

    private string formatChatMessage(LogSplitterConfig config, string message)
    {
        var formattedMessage = message; 
        if (config.filterChatMessage)
        {
            var filter = defaultFilter;
            try
            {
                filter = new Regex(@config.filterRegex, RegexOptions.None);
            }
            catch (ArgumentException ex)
            {
                log.Debug("Invalid filter regex");
                log.Debug(ex.Message);
            }

            formattedMessage = filter.Replace(formattedMessage, "");
        }
        
        if (config.addTimestamp)
        {
            formattedMessage = $"""{DateTime.Now:yyyy.mm.dd HH:MM:ss} {formattedMessage}""";
        }
        
        return formattedMessage;
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
