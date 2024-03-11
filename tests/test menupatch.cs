using System;
using System.IO;
using System.Linq;
using Terraria.ModLoader;
using TModder.GameLauncher;
using Mono.Cecil;

class Program
{
    static void Main(string[] args)
    {
        string terrariaExePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Terraria.exe");
        string modsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Mods");

        try
        {
            string[] modDllPaths = Directory.GetFiles(modsDirectory, "*.dll");

            GameEvents.GameLaunched += OnGameLaunched;

            GameEmbedder.EmbedGameWithMods(terrariaExePath, modDllPaths);
            PatchMainLogoText(terrariaExePath);

            Console.WriteLine("Terraria with mods launched successfully!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }

    static void OnGameLaunched(object sender, EventArgs e)
    {
        Console.WriteLine("Terraria game launched!");
    }

    static void PatchMainLogoText(string terrariaExePath)
    {
        try
        {
            using (var assemblyDefinition = AssemblyDefinition.ReadAssembly(terrariaExePath))
            {
                var mainModule = assemblyDefinition.MainModule;
                var type = mainModule.GetType("Terraria.Main");
                if (type != null)
                {
                    foreach (var field in type.Fields)
                    {
                        if (field.Name == "menuLogoString" && field.IsStatic)
                        {
                            field.InitialValue = System.Text.Encoding.ASCII.GetBytes("TModder\0");
                            assemblyDefinition.Write(terrariaExePath);
                            Console.WriteLine("Main menu logo text patched successfully!");
                            return;
                        }
                    }
                }
            }
            Console.WriteLine("Main menu logo text patch failed. Field not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while patching main menu logo text: {ex.Message}");
        }
    }
}
