using System;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;

/// <summary>
/// Entry point and manager for TModder functionality.
/// </summary>
public class TerrariaModder : Mod
{
    private const string TitleMenuLogoText = "TModder";
    private const string ModExtension = ".tmo";
    private const string ModsDirectory = "Mods";

    /// <summary>
    /// Loads TModder and manages mods.
    /// </summary>
    public override void Load()
    {
        LoadAndManageMods();
        PatchTitleMenuLogo();
    }

    /// <summary>
    /// Loads and manages mods found in the Mods directory.
    /// </summary>
    private void LoadAndManageMods()
    {
        string modsDirectory = Path.Combine(Main.SavePath, ModsDirectory);
        if (!Directory.Exists(modsDirectory))
        {
            Console.WriteLine($"Mods directory not found: {modsDirectory}");
            return;
        }

        string[] modFiles = Directory.GetFiles(modsDirectory, $"*{ModExtension}");
        if (modFiles.Length == 0)
        {
            Console.WriteLine($"No mod files ({ModExtension}) found in the directory.");
            return;
        }

        foreach (string modFile in modFiles)
        {
            CompileModToTMod(modFile);
            PatchMod(modFile);
        }
    }

    /// <summary>
    /// Compiles the mod to .tmo format.
    /// </summary>
    private void CompileModToTMod(string modFile)
    {
        Console.WriteLine($"Compiling mod to .tmo: {modFile}");

        try
        {
            string modName = Path.GetFileNameWithoutExtension(modFile);
            string modDirectory = Path.GetDirectoryName(modFile);
            string buildCommand = $"dotnet build {modName}.csproj -c Release -o {modDirectory} --output {modDirectory}";

            var processInfo = new System.Diagnostics.ProcessStartInfo("cmd.exe", "/c " + buildCommand)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    Console.WriteLine($"Mod compiled to .tmo successfully: {modName}");
                }
                else
                {
                    Console.WriteLine($"Error compiling mod to .tmo: {process.StandardError.ReadToEnd()}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error compiling mod to .tmo file: {ex.Message}");
        }
    }

    /// <summary>
    /// Loads and patches the mod.
    /// </summary>
    private void PatchMod(string modFile)
    {
        Console.WriteLine($"Patching mod: {modFile}");

        try
        {
            Mod mod = ModLoader.GetModFromFile(modFile);
            if (mod != null)
            {
                mod.Autoload();
                Console.WriteLine($"Mod patched successfully: {mod.Name}");
            }
            else
            {
                Console.WriteLine($"Failed to load mod from file: {modFile}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error patching mod: {ex.Message}");
        }
    }

    /// <summary>
    /// Patches the title menu logo with the TModder logo.
    /// </summary>
    private void PatchTitleMenuLogo()
    {
        try
        {
            Main.logoTexture = GetTitleLogoTexture();
            Console.WriteLine("Title menu logo patched successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to patch title menu logo: {ex.Message}");
        }
    }

    /// <summary>
    /// Retrieves the title menu logo texture.
    /// </summary>
    /// <returns>The title menu logo texture.</returns>
    private Texture2D GetTitleLogoTexture()
    {
        string logoFilePath = "Content/Logo/Logo.png";

        try
        {
            using (FileStream stream = new FileStream(logoFilePath, FileMode.Open))
            {
                return Texture2D.FromStream(Main.instance.GraphicsDevice, stream);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load title menu logo texture: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Modifies the interface layers to include mod management UI.
    /// </summary>
    /// <param name="layers">List of interface layers.</param>
    public override void ModifyInterfaceLayers(System.Collections.Generic.List<GameInterfaceLayer> layers)
    {
        ModUIManager.ModUIInterface.AddLayer(layers);
    }
}
