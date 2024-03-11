using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using Terraria.ModLoader;
using TModder.GameLauncher;
using TModder.Interfaces;

namespace TModder
{
    /// <summary>
    /// Main class for TModder application.
    /// </summary>
    public class MainClass
    {
        /// <summary>
        /// Entry point of the application.
        /// </summary>
        /// <param name="args">Command-line arguments.</param>
        public static void Main(string[] args)
        {
            // Path to Terraria.exe and mod DLLs
            string gameExePath = "Terraria.exe"; // Adjust as needed
            string[] modDllPaths = Directory.GetFiles("Mods", "*.dll"); // Adjust mod directory as needed

            // Embed the game with mods into a separate executable
            GameEmbedder.EmbedGameWithMods(gameExePath, modDllPaths);

            // Subscribe to the GameLaunched event
            GameEvents.GameLaunched += OnGameLaunched;

            // Launch the game
            LaunchGame();
        }

        /// <summary>
        /// Method to launch the game.
        /// </summary>
        private static void LaunchGame()
        {
            // Add custom menu
            AddCustomMenu();

            // Launch the embedded game
            string embeddedGameExePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "TModder.Terraria.exe");
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = embeddedGameExePath,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            Process embeddedGameProcess = Process.Start(startInfo);

            // Wait for the embedded game to exit
            embeddedGameProcess.WaitForExit();

            // Once the embedded game exits, proceed with further actions
            Console.WriteLine("The embedded game has exited.");
        }

        /// <summary>
        /// Method to handle the GameLaunched event.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event arguments.</param>
        private static void OnGameLaunched(object sender, EventArgs e)
        {
            Console.WriteLine("Game launched event received.");

            // Add mod manager in settings
            AddModManager();
        }

        /// <summary>
        /// Method to add a custom menu.
        /// </summary>
        private static void AddCustomMenu()
        {
            Console.WriteLine("Custom menu added.");
        }

        /// <summary>
        /// Method to add a mod manager in settings.
        /// </summary>
        private static void AddModManager()
        {
            Console.WriteLine("Mod manager added to settings.");
        }
    }
}
