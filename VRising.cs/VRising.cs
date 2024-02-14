using System;
using System.Diagnostics;
using System.Threading.Tasks;
using WindowsGSM.Functions;
using WindowsGSM.GameServer.Query;
using WindowsGSM.GameServer.Engine;
using System.IO;
using System.Linq;
using System.Net;
using System.Collections.Generic;

namespace WindowsGSM.Plugins
{
    public class VRising : SteamCMDAgent
    {
        // - Plugin Details
        public Plugin Plugin = new Plugin
        {
            name = "WindowsGSM.VRising", // WindowsGSM.XXXX
            author = "Sarpendon",
            description = "WindowsGSM plugin for supporting VRising Dedicated Server",
            version = "1.1",
            url = "https://github.com/Sarpendon/WindowsGSM.VRising", // Github repository link (Best practice)
            color = "#8802db" // Color Hex
        };

        // - Settings properties for SteamCMD installer
        public override bool loginAnonymous => true;
        public override string AppId => "1829350"; // Game server appId

        // - Standard Constructor and properties
        public VRising(ServerConfig serverData) : base(serverData) => base.serverData = _serverData = serverData;
        private readonly ServerConfig _serverData;
        public string Error, Notice;


        // - Game server Fixed variables
        public override string StartPath => @"VRisingServer.exe"; // Game server start path
        public string FullName = "VRising Dedicated Server"; // Game server FullName
        public bool AllowsEmbedConsole = false;  // Does this server support output redirect?
        public int PortIncrements = 2; // This tells WindowsGSM how many ports should skip after installation
        public object QueryMethod = new A2S(); // Query method should be use on current server type. Accepted value: null or new A2S() or new FIVEM() or new UT3()


        // - Game server default values
        public string Port = "9876"; // UDP port for game traffic, TCP for rcon traffic
        public string QueryPort = "9877"; // UDP port for Steam server list features
        public string Defaultmap = "world1"; // Name of save file/directory
        public string Maxplayers = "10"; // Max number of concurrent players on server
        public string Additional = "-maxConnectedAdmins 4 -persistentDataPath \"\save-data\""; // Additional server start parameter


        // - Create a default cfg for the game server after installation
        public async void CreateServerCFG()
        {
             
        }

        // - Start server function, return its Process to WindowsGSM
        public async Task<Process> Start()
        {


            string shipExePath = Functions.ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath);

            // Prepare start parameter
			string param = $""; // Set basic parameters
			param += string.IsNullOrWhiteSpace(_serverData.ServerMap) ? string.Empty : $"-saveName \"{_serverData.ServerMap}\"";
			param += string.IsNullOrWhiteSpace(_serverData.ServerIP) ? string.Empty : $" -address \"{_serverData.ServerIP}\"";
			param += string.IsNullOrWhiteSpace(_serverData.ServerName) ? string.Empty : $" -serverName \"{_serverData.ServerName}\"";
			param += string.IsNullOrWhiteSpace(_serverData.ServerGSLT) ? string.Empty : $" -PrivateServerPassword={_serverData.ServerGSLT}";
			param += string.IsNullOrWhiteSpace(_serverData.ServerMaxPlayer) ? string.Empty : $" -maxConnectedUsers {_serverData.ServerMaxPlayer}";
			param += string.IsNullOrWhiteSpace(_serverData.ServerPort) ? string.Empty : $" -gamePort {_serverData.ServerPort}"; 
			param += string.IsNullOrWhiteSpace(_serverData.ServerQueryPort) ? string.Empty : $" -queryPort {_serverData.ServerQueryPort}";
			param += string.IsNullOrWhiteSpace(_serverData.ServerParam) ? string.Empty : $" {_serverData.ServerParam}";



            // Prepare Process
            var p = new Process
            {
                StartInfo =
                {
                    WorkingDirectory = ServerPath.GetServersServerFiles(_serverData.ServerID),
                    FileName = ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath),
                    Arguments = param,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    UseShellExecute = false
                },
                EnableRaisingEvents = true
            };

            // Set up Redirect Input and Output to WindowsGSM Console if EmbedConsole is on
            if (AllowsEmbedConsole)
            {
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                var serverConsole = new ServerConsole(_serverData.ServerID);
                p.OutputDataReceived += serverConsole.AddOutput;
                p.ErrorDataReceived += serverConsole.AddOutput;

                // Start Process
                try
                {
                    p.Start();
                }
                catch (Exception e)
                {
                    Error = e.Message;
                    return null; // return null if fail to start
                }

                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                return p;
            }

            // Start Process
            try
            {
                p.Start();
                return p;
            }
            catch (Exception e)
            {
                Error = e.Message;
                return null; // return null if fail to start
            }
        }

		// - Stop server function
     public async Task Stop(Process p)
		{
			await Task.Run(() =>
			{
				Functions.ServerConsole.SetMainWindow(p.MainWindowHandle);
				Functions.ServerConsole.SendWaitToMainWindow("^c"); // Send Ctrl+C command
				p.WaitForExit(5000);
			});
		}


        // - Update server function
        public async Task<Process> Update(bool validate = false, string custom = null)
        {
            var (p, error) = await Installer.SteamCMD.UpdateEx(serverData.ServerID, AppId, validate, custom: custom, loginAnonymous: loginAnonymous);
            Error = error;
            await Task.Run(() => { p.WaitForExit(); });
            return p;
        }

        public bool IsInstallValid()
        {
            return File.Exists(Functions.ServerPath.GetServersServerFiles(_serverData.ServerID, StartPath));
        }

        public bool IsImportValid(string path)
        {
            string exePath = Path.Combine(path, "PackageInfo.bin");
            Error = $"Invalid Path! Fail to find {Path.GetFileName(exePath)}";
            return File.Exists(exePath);
        }

        public string GetLocalBuild()
        {
            var steamCMD = new Installer.SteamCMD();
            return steamCMD.GetLocalBuild(_serverData.ServerID, AppId);
        }

        public async Task<string> GetRemoteBuild()
        {
            var steamCMD = new Installer.SteamCMD();
            return await steamCMD.GetRemoteBuild(AppId);
        }
    }
}
