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

        //Dependencies
        public PaddleController controller;
        Ball ball;      //Need reference to ball for collision

        bool autopaddle;  //cheat

        Game g;

        public Paddle(Game game, Ball b) : base(game)
        {
            g = game;
            this.autopaddle = true;
            this.Speed = 200;
            this.ball = b;
            controller = new PaddleController(game, ball);
        }

        protected override void LoadContent()
        {
#if DEBUG   //Show markers if we are in debug mode
            this.ShowMarkers = true;
#endif
            //SetInitialLocation();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            //Deal with ball state
            switch (ball.State)
            {
                case BallState.OnPaddleStart:
                    //Move the ball with the paddle until launch
                    UpdateMoveBallWithPaddle();
                    break;
                case BallState.Playing:
                    Game1.gm.DirectionAfterCollision();
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

            GameManager.CheckWhichPlayer(this, Game);

            if (GameManager.P1 == true)
            {
                ball.Location = new Vector2(this.Location.X + this.LocationRect.Width, this.Location.Y + (this.SpriteTexture.Height / 2 - ball.spriteTexture.Height / 2));
            }
            else if (GameManager.P1 == false)
            {
                ball.Location = new Vector2(this.Location.X - (this.LocationRect.Width  - ball.spriteTexture.Width/2), this.Location.Y + (this.SpriteTexture.Height / 2 - ball.spriteTexture.Height / 2));
            }
        }

        private void KeepPaddleOnScreen()
        {
            if(this.Location.Y < 0)
            {
                this.Location.Y = MathHelper.Clamp(this.Location.Y, 0, this.Game.GraphicsDevice.Viewport.Height - this.spriteTexture.Height);
            }
            if(this.Location.Y > this.Game.GraphicsDevice.Viewport.Height - this.spriteTexture.Height)
            {
                this.Location.Y = MathHelper.Clamp(this.Location.Y, 0, this.Game.GraphicsDevice.Viewport.Height - this.spriteTexture.Height);
            }
        }
    }
}
