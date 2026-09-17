# WindowsGSM.VRising
 🧩WindowsGSM plugin that provides V Rising Dedicated server
 
 🏷️ To be used with https://windowsgsm.com/ 

> [!CAUTION]
> Please read about the Server Settings below before opening a ticket!

# Basic Installation: 
1. Download  WindowsGSM from the Link above.
2. Download this Plugin as .zip container and don't unpack it.
3. Create a Folder at a Location you wan't all Server to be Installed and Run.
4. Drag WindowsGSM.Exe into previoulsy created folder and execute it.
5. Press on the Puzzle Icon in the left bottom side and install this plugin by navigating to it and select the Zip File.
6. Wait a couple of seconds then close the plugin menu and install the game server.


# The Game:
- 🕹️ **Steam Site:** https://store.steampowered.com/app/1604030/V_Rising/
- 📁 **Homepage:** https://playvrising.com/

# Requirements:
- 🖥️ **WindowsGSM** >= 1.21.0

# Server Settings:
> [!IMPORTANT]
>- **Server Name:** *Name of server in server list
>- **Server IP Adress:** *Local IP of your Server there is no need to change this GSM should get the right IP adress itself*
>- **Server Port:** *UDP port for game traffic, TCP for rcon traffic*
>- **Server Query Port:** *UDP port for Steam server list features*
>- **Server Maxplayer:** *Max number of concurrent players on server (passed as `-maxUsers`)*
>- **Server GSLT:** *Server password. Leave empty for a server without a password. Passed as
>  `-password`; before plugin version 1.1 this field was sent under a parameter V Rising does not
>  have, so it had no effect.*
>- **Server Start Map:** *Name of save file/directory*
>- **Server Start Param:** *Some Parameter are already filled in by default you can add or remove them as you wish! 

> [!TIP]
> The ServerGameSettings.json file will let you configure the gameplay settings. Note, there is a known issue that after a server has been created, several settings cannot 
> be updated, even after restarting the server.

# Other Server Settings:
| Server Start Param| Description |
| --- | --- | 
| `-maxAdmins` | Max number of admins to allow connect even when server is full. Was `-maxConnectedAdmins` before V Rising 1.0; the old spelling is ignored by current servers. |
| `-persistentDataPath` | Absolute or relative path to where Settings and Save files are held. | 



> [!NOTE]
>For more Settings check this link: https://cdn.stunlock.com/blog/2022/05/25083113/Game-Server-Settings.pdf

# Changelog:
### 1.1
- **The server password works now.** The plugin passed `-PrivateServerPassword`, which is an Unreal
  parameter carried over from the Myth of Empires plugin - V Rising has no such option and ignored
  it, so anything entered in the **Server GSLT** field never became a password. It now uses the
  documented `-password`.
- **The player limit works now.** `-maxConnectedUsers` / `-maxConnectedAdmins` are the 0.6.x
  spellings; V Rising 1.0 renamed them to `-maxUsers` / `-maxAdmins`, so the limit was ignored.
  The default Start Parameters are updated accordingly - existing servers should change
  `-maxConnectedAdmins` to `-maxAdmins` by hand.
- **Stopping the server now reaches it.** The stop signal was sent to the server's window with
  SendKeys, but WindowsGSM hides that window right after starting the server - so the keystroke
  went to whatever window happened to have focus on the machine, never to the server. Every stop
  ran into the timeout and ended in a hard kill. The signal is now raised on the server's own
  console, so it shuts down properly instead of being killed.
- The shutdown output stays readable for a few seconds instead of being cleared instantly.
- **Failed installs and updates now say why.** The reason was being swallowed and shown as an
  empty `[ERROR]`; a failed update additionally crashed with a `NullReferenceException`.
- **Importing an existing server works.** It was looking for `PackageInfo.bin`, a file this game
  does not ship, so the import always failed.
- A missing server executable is reported as such instead of a generic Windows error.
- Console output is read as UTF-8, so umlauts and other non-ASCII characters are no longer mangled.

# Other WinGSM Plugins:
| Icon | Game Name | Link | Version |
| --- | --- | --- | --- |
| <img src="https://i.imgur.com/LI1uPIJ.png" width="100" height="100"> | Myth of Empires Dedicated Server | [GitHub Link](https://github.com/Sarpendon/WindowsGSM.MythofEmpires) | 2.0 |
| <img src="https://i.imgur.com/25x4Ohs.png" width="100" height="100"> | Valheim Dedicated Server | [GitHub Link](https://github.com/Sarpendon/WindowsGSM.Valheim) | 1.2 |
| <img src="https://i.imgur.com/A9jtLPQ.png" width="100" height="100"> | V Rising Dedicated Server | [GitHub Link](https://github.com/Sarpendon/WindowsGSM.VRising) | 1.1 |
| <img src="https://i.imgur.com/A6dCSy9.png" width="100" height="100"> | Life is Feudal Dedicated Server | [GitHub Link](https://github.com/Sarpendon/WindowsGSM.LifeIsFeudal) | 1.2 |

