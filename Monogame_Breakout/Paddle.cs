using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace Monogame_Breakout
{
    public class Paddle : DrawableSprite
    {
        //Service Dependencies
        GameConsole console;

        //Dependencies
        public PaddleController controller;
        Ball ball;      //Need reference to ball for collision

        bool autopaddle;  //cheat

        public Paddle(Game game, Ball b) : base(game)
        {

            this.autopaddle = true;
            this.Speed = 200;
            this.ball = b;
            controller = new PaddleController(game, ball);

            //Lazy load GameConsole
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console make a new one and add it to the game
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }

            r = new Random();
        }

        protected override void LoadContent()
        {
#if DEBUG   //Show markers if we are in debug mode
            this.ShowMarkers = true;
#endif
            //SetInitialLocation();
            base.LoadContent();
        }

        Rectangle collisionRectangle;  //Rectangle for paddle collision uses just the top of the paddle instead of the whole sprite

        public override void Update(GameTime gameTime)
        {
            //Update Collision Rect
            collisionRectangle = new Rectangle((int)this.Location.X, (int)this.Location.Y, this.spriteTexture.Width, 1);

            //Deal with ball state
            switch (ball.State)
            {
                case BallState.OnPaddleStart:
                    //Move the ball with the paddle until launch
                    UpdateMoveBallWithPaddle();
                    break;
                case BallState.Playing:
                    UpdateCheckBallCollision();
                    break;
            }

            //Movement from controller
            controller.HandleInput(gameTime);

            this.Direction = controller.Direction;
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            KeepPaddleOnScreen();

            if (autopaddle && ball.State == BallState.Playing) //Alllow cheating
            {
                this.Location.Y = ball.Location.Y - ((int)this.spriteTexture.Width / 2 * this.scale);
            }

            base.Update(gameTime);
        }

        private void UpdateMoveBallWithPaddle()
        {
            ball.Speed = 0;
            ball.Direction = Vector2.Zero;

            Utils.CheckWhichPlayer(this, Game);

            if (Utils.P1 == true)
            {
                ball.Location = new Vector2(this.Location.X + this.LocationRect.Width, this.Location.Y + (this.SpriteTexture.Height / 2 - ball.spriteTexture.Height / 2));
            }
            else if (Utils.P1 == false)
            {
                ball.Location = new Vector2(this.Location.X - (this.LocationRect.Width  - ball.spriteTexture.Width/2), this.Location.Y + (this.SpriteTexture.Height / 2 - ball.spriteTexture.Height / 2));
            }
        }


        private void UpdateCheckBallCollision()
        {
            //Ball Collsion
            //Very simple collision with ball only uses rectangles
            if (collisionRectangle.Intersects(ball.LocationRect))
            {
                //TODO Change angle based on location of collision or direction of paddle

                Utils.CheckWhichPlayer(this, Game);
                if(Utils.P1 == true) { ball.Direction.Y *= -1; }
                if(Utils.P1 == false) { ball.Direction.Y *= 1; }

                UpdateBallCollisionBasedOnPaddleImpactLocation();
                UpdateBallCollisionRandomFuness();
                console.GameConsoleWrite("Paddle collision ballLoc:" + ball.Location + " paddleLoc:" + this.Location.ToString());
            }
        }

        Random r;

        /// <summary>
        /// Adds a bit of randomness to the ball bounce
        /// </summary>
        private void UpdateBallCollisionRandomFuness()
        {   
            GetReflectEntropy();
        }


        private void GetReflectEntropy()
        {
            Utils.CheckWhichPlayer(this, Game);
            if (Utils.P1 == true) { ball.Direction.X = - 1 + ((r.Next(0, 3) - 1) * 0.1f); }//return -.9, -1 or -1.1
            else if (Utils.P1 == false) { ball.Direction.X = 1 + ((r.Next(0, 3) - 1) * 0.1f); }
            
        }

        /// <summary>
        /// Makes the paddle more able to direct the ball
        /// </summary>
        private void UpdateBallCollisionBasedOnPaddleImpactLocation()
        {
            //Change angle based on paddle movement
            if (this.Direction.Y > 0)
            {
                ball.Direction.Y += .1f;
            }
            if (this.Direction.Y < 0)
            {
                ball.Direction.Y -= .1f;
            }
            //Change anlge based on side of paddle
            //First Third

            if ((ball.Location.Y > this.Location.Y) && (ball.Location.Y < this.Location.Y + this.spriteTexture.Height / 3))
            {
                console.GameConsoleWrite("1st Third");
                ball.Direction.Y += .1f;
            }
            if ((ball.Location.Y > this.Location.Y + (this.spriteTexture.Height / 3)) && (ball.Location.Y < this.Location.Y + (this.spriteTexture.Height / 3) * 2))
            {
                console.GameConsoleWrite("2nd third");
            }
            if ((ball.Location.Y > (this.Location.Y + (this.spriteTexture.Height / 3) * 2)) && (ball.Location.Y < this.Location.Y + (this.spriteTexture.Height)))
            {
                console.GameConsoleWrite("3rd third");
                ball.Direction.Y -= .1f;
            }
        }

        private void KeepPaddleOnScreen()
        {
            this.Location.Y = MathHelper.Clamp(this.Location.Y, 0, this.Game.GraphicsDevice.Viewport.Height - this.spriteTexture.Height);
        }
    }
}
