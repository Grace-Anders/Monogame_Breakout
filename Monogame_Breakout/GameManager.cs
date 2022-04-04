using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monogame_Breakout
{
    public class GameManager : DrawableGameComponent
    {
        //Service Dependencies
        public static GameConsole console;

        public Paddle pOne;
        public Ball ballOne;

        public Paddle pTwo;
        public Ball ballTwo;

        Game g;

        //static values 
        public static bool P1;

        public static bool P1Lost, P2Lost = false;

        public static bool GameOver = false;

        public GameManager(Game game) : base(game)
        {
            //Lazy load GameConsole
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console make a new one and add it to the game
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }

            g = game;
            //GameComponents
            ballOne = new Ball(game); //Ball first paddle and block manager depend on ball
            game.Components.Add(ballOne);
            pOne = new Paddle(game, ballOne);
            game.Components.Add(pOne);

            ballTwo = new Ball(game); //Ball first paddle and block manager depend on ball
            game.Components.Add(ballTwo);
            pTwo = new Paddle(game, ballTwo);
            game.Components.Add(pTwo);

            r = new Random();
        }

        protected override void LoadContent()
        {
            pOne.spriteTexture = g.Content.Load<Texture2D>("paddleOne");
            pOne.Location = new Vector2((0), (GraphicsDevice.Viewport.Height / 2 - (pOne.spriteTexture.Height / 2)));
            pOne.controller.Up = Keys.W;
            pOne.controller.Down = Keys.S;
            pOne.controller.Launch = Keys.LeftShift;
            ballOne.LaunchDirection = new Vector2(1, -1);

            pTwo.spriteTexture = g.Content.Load<Texture2D>("paddleTwo");
            pTwo.Location = new Vector2((GraphicsDevice.Viewport.Width - (pTwo.spriteTexture.Width)), (GraphicsDevice.Viewport.Height / 2 - (pTwo.spriteTexture.Height / 2)));
            pTwo.controller.Up = Keys.U;
            pTwo.controller.Down = Keys.J;
            pTwo.controller.Launch = Keys.RightShift;
            ballTwo.LaunchDirection = new Vector2(-1, -1);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


        public void DirectionAfterCollision()
        {
            if (pOne.LocationRect.Intersects(ballOne.LocationRect)) //if P1 ball hits P1 paddle
            {
                ballOne.Direction.X *= 1;
                UpdateCheckBallCollision(ballOne, pOne);
            }
            if (pOne.LocationRect.Intersects(ballTwo.LocationRect)) //if P2 ball hits P1 paddle
            {
                ballTwo.Direction.X *= 1;
                UpdateCheckBallCollision(ballTwo, pOne);
            }
            if (pTwo.LocationRect.Intersects(ballOne.LocationRect)) //if P1 ball hits P2 paddle
            {
                ballOne.Direction.X *= -1;
                UpdateCheckBallCollision(ballOne, pTwo);
            }
            if (pTwo.LocationRect.Intersects(ballTwo.LocationRect)) //if P2 ball hits P2 paddle
            {
                ballTwo.Direction.X *= -1;
                UpdateCheckBallCollision(ballTwo, pTwo);
            }
        }

        private void UpdateCheckBallCollision(Ball b, Paddle p)
        {
            UpdateBallCollisionBasedOnPaddleImpactLocation(b, p);
            UpdateBallCollisionRandomFuness(b, p);
            console.GameConsoleWrite("Paddle collision ballLoc:" + b.Location + " paddleLoc:" + p.Location.ToString());
        }

        Random r;

        private void UpdateBallCollisionRandomFuness(Ball b, Paddle p)
        {
            CheckWhichPlayer(p, Game);
            if (P1 == true) { b.Direction.X = 1 + ((r.Next(0, 3) - 1) * 0.1f); }//return -.9, -1 or -1.1
            else if (P1 == false) { b.Direction.X = -1 + ((r.Next(0, 3) - 1) * 0.1f); }
        }

        /// <summary>
        /// Makes the paddle more able to direct the ball
        /// </summary>
        private void UpdateBallCollisionBasedOnPaddleImpactLocation(Ball b, Paddle p)
        {
            //Change angle based on paddle movement

            if (p.Direction.Y > 0)//down
            {
                b.Direction.Y += .1f;
            }
            if (p.Direction.Y < 0)//up
            {
                b.Direction.Y -= .1f;
            }
            //Change anlge based on side of paddle
            //First Third

            if ((b.Location.Y > p.Location.Y) && (b.Location.Y < p.Location.Y + p.spriteTexture.Height / 3))
            {
                console.GameConsoleWrite("1st Third");
                b.Direction.Y += .1f;
            }
            if ((b.Location.Y > p.Location.Y + (p.spriteTexture.Height / 3)) && (b.Location.Y < p.Location.Y + (p.spriteTexture.Height / 3) * 2))
            {
                console.GameConsoleWrite("2nd third");
            }
            if ((b.Location.Y > (p.Location.Y + (p.spriteTexture.Height / 3) * 2)) && (b.Location.Y < p.Location.Y + (p.spriteTexture.Height)))
            {
                console.GameConsoleWrite("3rd third");
                b.Direction.Y -= .1f;
            }

        }

        public static void WriteToConsole(string write)
        {
            console.GameConsoleWrite(write);
        }

        public static void CheckWhichPlayer(Paddle p, Game game)
        {
            if (p.Location.X < game.GraphicsDevice.Viewport.Width / 2) { P1 = true; }
            else if (p.Location.X > game.GraphicsDevice.Viewport.Width / 2) { P1 = false; }
        }
    }
}
