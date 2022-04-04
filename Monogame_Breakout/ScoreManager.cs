using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame_Breakout
{
    public class ScoreManager : DrawableGameComponent
    {

        SpriteFont font;
        public static int P1Lives, P2Lives;
        //public static int P1Score, P2Score;
        public static int Level;

        Texture2D paddle;   //Texture for drawing lives left scoremanager is also the GUI/HUD

        SpriteBatch sb;
        public Vector2 P1statsLoc, P2statsLoc, levelLoc; //Locations to draw GUI elements


        public ScoreManager(Game game)
            : base(game)
        {
            SetupNewGame();
        }


        private static void SetupNewGame()  //Generally mixing static and non static methods is messy be careful
        {
            P1Lives = P2Lives = 3;
            //P1Score = P2Score = 0;
            Level = 1;
        }

        bool Decreased;
        public void DecreaseLives (bool which)
        {
            Decreased = false;
            if(which == true && Decreased == false)//p1
            {
                P1Lives--;
                Decreased = true;
            }
            if(which == false || Decreased == false)//p2
            {
                P2Lives--;
                Decreased = true;
            }
        }

        protected override void LoadContent()
        {
            sb = new SpriteBatch(this.Game.GraphicsDevice);
            font = this.Game.Content.Load<SpriteFont>("Arial");
            paddle = this.Game.Content.Load<Texture2D>("paddleSmall");

            P1statsLoc = new Vector2(10, 10);
            P2statsLoc = new Vector2(GraphicsDevice.Viewport.Width - 65, 10);
            levelLoc = new Vector2(GraphicsDevice.Viewport.Width /2 -15, 10);
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            sb.Begin();
            //P1
            for (int i = 0; i < P1Lives; i++)
            {
                sb.Draw(paddle, new Rectangle((65 * i) + 100, 15, paddle.Width / 2, paddle.Height / 2), Color.White);
            }
            sb.DrawString(font, $"Player 1\nLives: {P1Lives}", P1statsLoc, Color.White);

            //P2
            for (int i = 0; i < P2Lives; i++)
            {
                sb.Draw(paddle, new Rectangle(((65 * i) + GraphicsDevice.Viewport.Width / 2) + 130, 15, paddle.Width / 2, paddle.Height / 2), Color.White);
            }
            sb.DrawString(font, $"Player 2\n Lives: {P2Lives}", P2statsLoc, Color.White);

            sb.DrawString(font, "Level: " + Level, levelLoc, Color.White);
            sb.End();
            base.Draw(gameTime);
        }
    }
}
