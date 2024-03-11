using System;

namespace TModder.GameLauncher
{
    public static class GameEvents
    {
        // Event raised when the game is launched
        public static event EventHandler GameLaunched;

        // Method to raise the GameLaunched event
        public static void OnGameLaunched()
        {
            GameLaunched?.Invoke(null, EventArgs.Empty);
        }
    }
}
