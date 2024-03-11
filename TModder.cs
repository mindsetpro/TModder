// VERSION 1.0.0 // DO NOT EXPECT MUCH //
         // Made by Mindset //

using System;
using System.IO;
using System.Reflection;
using Terraria.ModLoader;
using TModder.GameLauncher;
using TModder.Interfaces;

namespace TModder
{
    public class MainClass
    {
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

        // Method to launch the game
        private static void LaunchGame()
        {
            // Add custom menu
            AddCustomMenu();

            // Launch the embedded game
            // This should be replaced with code to start the embedded game
            Console.WriteLine("Launching the game...");
            // GameLauncher.LaunchEmbeddedGame();
        }

        // Method to handle the GameLaunched event
        private static void OnGameLaunched(object sender, EventArgs e)
        {
            Console.WriteLine("Game launched event received.");

            // Add mod manager in settings
            AddModManager();
        }

        // Method to add a custom menu
        private static void AddCustomMenu()
        {
            Console.WriteLine("Custom menu added.");
        }

        // Method to add a mod manager in settings
        private static void AddModManager()
        {
            Console.WriteLine("Mod manager added to settings.");
        }
    }
}
