#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

#endregion

namespace PlatformerPOC
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static PlatformGame game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            game = new PlatformGame();
            game.Run();
        }
    }
}
