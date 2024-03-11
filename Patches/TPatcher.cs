using HarmonyLib;
using Mono.Cecil;
using System.Reflection;

namespace TModder.TPatcher
{
    public static class PatchingLogic
    {
        // Method to apply patches to the DLL mod
        public static void ApplyPatches(string modDllPath)
        {
            // Load the assembly of the DLL mod using Mono.Cecil
            AssemblyDefinition modAssembly = AssemblyDefinition.ReadAssembly(modDllPath);

            // Create an instance of Harmony
            Harmony harmony = new Harmony("com.wrd.tmodder");

            // Apply patches using Harmony
            harmony.PatchAll(modAssembly);

            // Save the modified assembly back to the DLL mod
            modAssembly.Write(modDllPath);
        }
    }
}
