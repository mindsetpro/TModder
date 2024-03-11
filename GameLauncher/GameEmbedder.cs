using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;

namespace TModder.GameLauncher
{
    public static class GameEmbedder
    {
        public static void EmbedGameWithMods(string gameExePath, string[] modDllPaths)
        {
            // Embed the game and mods into a separate executable
            string embeddedExePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TModder.Terraria.exe");
            File.Copy(gameExePath, embeddedExePath, true);

            // Apply patches to the mods
            foreach (string modDllPath in modDllPaths)
            {
                PatchingLogic.ApplyPatches(modDllPath);
            }

            // Start the embedded game executable in a separate process
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = $"/k \"{embeddedExePath}\"",
                UseShellExecute = false,
                CreateNoWindow = false
            };

            Process process = Process.Start(startInfo);

            // Keep the terminal window open until the game is launched
            while (!process.HasExited)
            {
                Thread.Sleep(3000);
            }

            // Clean up the embedded executable after the game is launched
            File.Delete(embeddedExePath);
        }
    }
}
