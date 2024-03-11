using System.Diagnostics;

namespace TModder.GameLauncher
{
    public static class GameKillSwitch
    {
        public static void KillGameProcess()
        {
            // Find and kill the process associated with the game
            Process[] processes = Process.GetProcessesByName("Terraria");
            foreach (Process process in processes)
            {
                process.Kill();
            }
        }
    }
}
