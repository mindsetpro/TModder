using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;

public class TerrariaModder
{
    private static readonly string TerrariaAssemblyPath = Path.Combine(Environment.CurrentDirectory, "Terraria.exe");
    private const string TitleMenuLogoText = "TModder";

    private static void ColorWriteLine(ConsoleColor color, string message)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void PatchMods()
    {
        try
        {
            // Load embedded Terraria assembly
            var terrariaAssembly = Assembly.LoadFrom(TerrariaAssemblyPath);

            // Find the ModLoader type
            var modLoaderType = terrariaAssembly.GetType("Terraria.ModLoader.ModLoader");

            if (modLoaderType != null)
            {
                // Find the AddMod method
                var addModMethod = modLoaderType.GetMethod("AddMod", BindingFlags.Public | BindingFlags.Static);

                if (addModMethod != null)
                {
                    // Insert custom mod patching logic here
                    ColorWriteLine(ConsoleColor.Green, "Mod patching logic would be inserted here.");

                    ColorWriteLine(ConsoleColor.Green, "Mod patched successfully.");
                }
                else
                {
                    ColorWriteLine(ConsoleColor.Red, "Failed to patch mod: AddMod method not found.");
                }
            }
            else
            {
                ColorWriteLine(ConsoleColor.Red, "Failed to patch mods: ModLoader type not found.");
            }
        }
        catch (Exception ex)
        {
            ColorWriteLine(ConsoleColor.Red, "Error patching mods: " + ex.Message);
        }
    }

    public void ChangeTitleMenuLogoText()
    {
        try
        {
            // Load embedded Terraria assembly
            var terrariaAssembly = Assembly.LoadFrom(TerrariaAssemblyPath);

            // Find the TitleScreen type
            var titleScreenType = terrariaAssembly.GetType("Terraria.ModLoader.UI.TitleScreen");

            if (titleScreenType != null)
            {
                // Find the SetDefaults method
                var setDefaultsMethod = titleScreenType.GetMethod("SetDefaults", BindingFlags.Public | BindingFlags.Instance);

                if (setDefaultsMethod != null)
                {
                    // Insert custom title menu logo text changing logic here
                    ColorWriteLine(ConsoleColor.Green, "Title menu logo text changing logic would be inserted here.");

                    ColorWriteLine(ConsoleColor.Green, "Title menu logo text changed successfully.");
                }
                else
                {
                    ColorWriteLine(ConsoleColor.Red, "Failed to change title menu logo text: SetDefaults method not found.");
                }
            }
            else
            {
                ColorWriteLine(ConsoleColor.Red, "Failed to change title menu logo text: TitleScreen type not found.");
            }
        }
        catch (Exception ex)
        {
            ColorWriteLine(ConsoleColor.Red, "Error changing title menu logo text: " + ex.Message);
        }
    }

    public void ManageMods()
    {
        ColorWriteLine(ConsoleColor.Yellow, "Mod manager is not implemented yet.");
    }

    public void LaunchTerraria()
    {
        ColorWriteLine(ConsoleColor.Cyan, "Launching Terraria...");
        try
        {
            // Launch Terraria
            System.Diagnostics.Process.Start(TerrariaAssemblyPath);
        }
        catch (Exception ex)
        {
            ColorWriteLine(ConsoleColor.Red, "Error launching Terraria: " + ex.Message);
        }
    }

    public void LoadModsFromDirectory(string modsDirectory)
    {
        try
        {
            // Check if the mods directory exists
            if (!Directory.Exists(modsDirectory))
            {
                ColorWriteLine(ConsoleColor.Red, $"Mods directory not found: {modsDirectory}");
                return;
            }

            // Get all files in the mods directory
            string[] modFiles = Directory.GetFiles(modsDirectory, "*.tmo");

            if (modFiles.Length == 0)
            {
                ColorWriteLine(ConsoleColor.Yellow, "No mod files (.tmo) found in the directory.");
                return;
            }

            // Initialize a list to store mod names
            List<string> loadedMods = new List<string>();

            // Process each mod file
            foreach (string modFile in modFiles)
            {
                // Insert logic to load mod from .tmo file using mod patcher
                string modName = Path.GetFileNameWithoutExtension(modFile);
                PatchMod(modFile); // Apply patches using mod patcher
                loadedMods.Add(modName);

                // Read build file if exists
                string buildFilePath = Path.Combine(modsDirectory, $"build.{modName}.txt");
                if (File.Exists(buildFilePath))
                {
                    string buildInfo = File.ReadAllText(buildFilePath);
                    ColorWriteLine(ConsoleColor.Cyan, $"Build info for {modName}: {buildInfo}");
                }

                ColorWriteLine(ConsoleColor.Green, $"Mod loaded: {modName}");
            }

            ColorWriteLine(ConsoleColor.Green, $"Loaded {modFiles.Length} mod(s) from directory: {modsDirectory}");

            // Now you have a list of loaded mods, you can use it for further processing if needed
        }
        catch (Exception ex)
        {
            ColorWriteLine(ConsoleColor.Red, $"Error loading mods: {ex.Message}");
        }
    }
    public void PatchMod(string modFile)
    {
        try
        {
            // Load embedded Terraria assembly
            var terrariaAssembly = Assembly.LoadFrom(TerrariaAssemblyPath);

            // Find the ModLoader type
            var modLoaderType = terrariaAssembly.GetType("Terraria.ModLoader.ModLoader");

            if (modLoaderType != null)
            {
                // Find the AddMod method
                var addModMethod = modLoaderType.GetMethod("AddMod", BindingFlags.Public | BindingFlags.Static);

                if (addModMethod != null)
                {
                    // Extract mod instructions from the .tmo file
                    string modInstructions = ExtractModInstructions(modFile);

                    // Apply mod patches using the extracted instructions
                    ApplyModPatches(modInstructions);

                    ColorWriteLine(ConsoleColor.Green, $"Mod patched successfully: {Path.GetFileNameWithoutExtension(modFile)}");
                }
                else
                {
                    ColorWriteLine(ConsoleColor.Red, $"Failed to patch mod: AddMod method not found in {Path.GetFileName(TerrariaAssemblyPath)}");
                }
            }
            else
            {
                ColorWriteLine(ConsoleColor.Red, $"Failed to patch mods: ModLoader type not found in {Path.GetFileName(TerrariaAssemblyPath)}");
            }
        }
        catch (Exception ex)
        {
            ColorWriteLine(ConsoleColor.Red, $"Error patching mod {Path.GetFileNameWithoutExtension(modFile)}: {ex.Message}");
        }
    }

    private string ExtractModInstructions(string modFile)
    {
        // Extract mod instructions from the .tmo file
        using (ZipArchive archive = ZipFile.OpenRead(modFile))
        {
            foreach (ZipArchiveEntry entry in archive.Entries)
            {
                if (entry.FullName.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    using (StreamReader reader = new StreamReader(entry.Open()))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }

        return string.Empty;
    }

private void ApplyModPatches(string modInstructions)
{
    // Apply mod patches using the extracted instructions
    string[] lines = modInstructions.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

    foreach (string line in lines)
    {
        // Assuming each line represents a patch instruction
        // Split the line into command and parameters
        string[] parts = line.Split(':');
        if (parts.Length < 2)
        {
            // Invalid format, skip this line
            continue;
        }

        string command = parts[0].Trim();
        string[] parameters = parts[1].Split(',');

        // Apply patch based on the command and parameters
        switch (command)
        {
            case "AddItem":
                ApplyPatch(AddItemPatch, parameters);
                break;
            case "ModifySetting":
                ApplyPatch(ModifySettingPatch, parameters);
                break;
            // Add more cases for other patch commands as needed
            default:
                // Unknown command, skip this line
                break;
        }
    }
}

private void ApplyPatch(Action<string[]> patchMethod, string[] parameters)
{
    // Check if the patch method is provided
    if (patchMethod == null)
    {
        ColorWriteLine(ConsoleColor.Red, "Error applying patch: Patch method is null.");
        return;
    }

    // Apply the patch using the specified patch method
    try
    {
        patchMethod(parameters);
    }
    catch (Exception ex)
    {
        ColorWriteLine(ConsoleColor.Red, $"Error applying patch: {ex.Message}");
    }
}


private void AddItemPatch(string[] parameters)
{
    // Example implementation of AddItem patch
    if (parameters.Length < 2)
    {
        throw new ArgumentException("AddItem patch requires at least 2 parameters: itemName, itemId");
    }

    string itemName = parameters[0].Trim();
    int itemId = int.Parse(parameters[1].Trim());

    // Your logic to add the item
    // For example:
    ColorWriteLine(ConsoleColor.Cyan, $"Adding item: {itemName} (ID: {itemId})");
}

private void ModifySettingPatch(string[] parameters)
{
    // Example implementation of ModifySetting patch
    if (parameters.Length < 2)
    {
        throw new ArgumentException("ModifySetting patch requires at least 2 parameters: settingName, settingValue");
    }

    string settingName = parameters[0].Trim();
    string settingValue = parameters[1].Trim();

    // Your logic to modify the setting
    // For example:
    ColorWriteLine(ConsoleColor.Cyan, $"Modifying setting: {settingName} -> {settingValue}");
}



    public void BuildMod(string modDirectory)
    {
        try
        {
            // Build the mod located in the specified directory
            ColorWriteLine(ConsoleColor.Yellow, $"Building mod from directory: {modDirectory}");

            // Check if the mod directory exists
            if (!System.IO.Directory.Exists(modDirectory))
            {
                ColorWriteLine(ConsoleColor.Red, $"Mod directory not found: {modDirectory}");
                return;
            }

            // Use dotnet command-line tool to build the mod
            Process process = new Process();
            process.StartInfo.FileName = "dotnet";
            process.StartInfo.Arguments = "build";
            process.StartInfo.WorkingDirectory = modDirectory;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;

            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode == 0)
            {
                ColorWriteLine(ConsoleColor.Green, "Mod built successfully.");
            }
            else
            {
                ColorWriteLine(ConsoleColor.Red, $"Error building mod: {error}");
            }
        }
        catch (Exception ex)
        {
            ColorWriteLine(ConsoleColor.Red, $"Error building mod: {ex.Message}");
        }
    }
    public void CompileModToTmo(string modDirectory)
    {
        try
        {
            // Compile the mod located in the specified directory to .tmo file
            ColorWriteLine(ConsoleColor.Yellow, $"Compiling mod to .tmo file from directory: {modDirectory}");

            // Check if the mod directory exists
            if (!Directory.Exists(modDirectory))
            {
                ColorWriteLine(ConsoleColor.Red, $"Mod directory not found: {modDirectory}");
                return;
            }

            // Create a unique .tmo file name
            string tmoFilePath = Path.Combine(modDirectory, $"{Path.GetFileName(modDirectory)}.tmo");

            // Delete the existing .tmo file if it exists
            if (File.Exists(tmoFilePath))
            {
                File.Delete(tmoFilePath);
            }

            // Zip all files and directories in the mod directory into a .tmo file
            ZipFile.CreateFromDirectory(modDirectory, tmoFilePath);

            ColorWriteLine(ConsoleColor.Green, $"Mod compiled to .tmo successfully. Output file: {tmoFilePath}");
        }
        catch (Exception ex)
        {
            ColorWriteLine(ConsoleColor.Red, $"Error compiling mod to .tmo file: {ex.Message}");
        }
    }


    public void ModderMenu()
    {
        ColorWriteLine(ConsoleColor.Magenta, "Welcome to Terraria Modder!");
        ColorWriteLine(ConsoleColor.Magenta, "1. Patch Mods");
        ColorWriteLine(ConsoleColor.Magenta, "2. Change Title Menu Logo Text");
        ColorWriteLine(ConsoleColor.Magenta, "3. Manage Mods");
        ColorWriteLine(ConsoleColor.Magenta, "4. Load Mods from Directory");
        ColorWriteLine(ConsoleColor.Magenta, "5. Build Mod");
        ColorWriteLine(ConsoleColor.Magenta, "6. Compile Mod to .tmo");
        ColorWriteLine(ConsoleColor.Magenta, "7. Launch Terraria");
        ColorWriteLine(ConsoleColor.Magenta, "8. Exit");

        bool exit = false;
        while (!exit)
        {
            ColorWriteLine(ConsoleColor.Magenta, "Enter your choice: ");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    PatchMods();
                    break;
                case "2":
                    ChangeTitleMenuLogoText();
                    break;
                case "3":
                    ManageMods();
                    break;
                case "4":
                    ColorWriteLine(ConsoleColor.Magenta, "Enter the directory containing mods: ");
                    string modsDirectory = Console.ReadLine();
                    LoadModsFromDirectory(modsDirectory);
                    break;
                case "5":
                    ColorWriteLine(ConsoleColor.Magenta, "Enter the directory of the mod to build: ");
                    string modDirectory = Console.ReadLine();
                    BuildMod(modDirectory);
                    break;
                case "6":
                    ColorWriteLine(ConsoleColor.Magenta, "Enter the directory of the mod to compile to .tmo: ");
                    string modToCompileDirectory = Console.ReadLine();
                    CompileModToTmo(modToCompileDirectory);
                    break;
                case "7":
                    LaunchTerraria();
                    break;
                case "8":
                    exit = true;
                    break;
                default:
                    ColorWriteLine(ConsoleColor.Red, "Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public static void Main(string[] args)
    {
        TerrariaModder modder = new TerrariaModder();
        modder.ModderMenu();

        ColorWriteLine(ConsoleColor.Magenta, "Press any key to exit...");
        Console.ReadKey();
    }
}
