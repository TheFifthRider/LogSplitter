# LogSplitter

A [Vintage Story](https://github.com/anegostudios) mod.

Ever wanted to share some of your favorite RP moments with a player who wasn't there? Client-chat.txt has all of the words, but you have to edit it and format it... Not to mention if you only want to capture TheBasic's proximity chat while a lively discussion is happening in general, you're going to have a lot of that editing to do. Plus, if you leave the game and rejoin it'll get wiped! What's a person to do?

Fear not. With LogSplitter, a markdown file will get generated for each chat for each of your play sessions. No editing required!

The markdown files are saved with format YYYY-MM-DD-hhmm_<chat name>.md

## Config

The default LogSplitter config looks something like this:

{
  "serverChats": {
    "0": "GeneralChat",
    "-1": "ServerInfo",
    "-5": "DamageLog",
    "-6": "InfoLog",
    "-4": "Console"
  },
  "mutedChats": [
    -1,
    -5,
    -6,
    -4
  ],
  "chatDirectory": "C:\\Users\\<username>\\AppData\\Roaming\\VintagestoryData\\Logs"
}
### serverChats

A dictionary that maps chat ids to the name you'd like present in the markdown file. Recommended that you avoid spaces and special characters depending on what your OS likes, after all this is going to be in the file name of the created markdown file!

Defaults to handling GeneralChat, ServerInfo, DamageLog, InfoLog, and Console logs. You may need to add more if you're using something like TheBasic's proximity chat.

If a mapping for a chat is not present here, the markdown file will be saved as YYYY-MM-DD-hhmm_UnknownChat(<chat id>).md

### mutedChats

A list of chat ids that you don't want to create markdown files for.

### chatDirectory
The absolute path to where you'd like markdown files generated for each chat.

Defaults to your VintageStoryData\Logs directory.

## ConfigLib

Supports a configlib integration for in-game modification of settings, but due to limitations in ConfigLib at time of writing you can't modify serverChats and mutedChats from there -- just chatDirectory.
