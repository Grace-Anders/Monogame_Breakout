using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monogame_Breakout
{
    public static class Utils
    {
        public static bool P1;

        public static bool P1Lost, P2Lost = false;

        public static bool GameOver = false;

        public static GameManager gm;

        static Utils() { }

        //check which pattle
        public static void CheckWhichPlayer(Paddle p, Game game) 
        {
            if (p.Location.X < game.GraphicsDevice.Viewport.Width / 2) { P1 = true; }
            else if (p.Location.X > game.GraphicsDevice.Viewport.Width / 2) { P1 = false; }
        }
    }
}
