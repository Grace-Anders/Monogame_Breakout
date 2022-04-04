using MonoGameLibrary.Sprite;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;


namespace Monogame_Breakout
{
    public enum BallState { OnPaddleStart, Playing }

    public class Ball : DrawableSprite
    {
        public BallState State { get; private set; }

        GameConsole console;

        public Vector2 LaunchDirection;

        public Ball(Game game)
            : base(game)
        {
            this.State = BallState.OnPaddleStart;

            //Lazy load GameConsole
            console = (GameConsole)this.Game.Services.GetService(typeof(IGameConsole));
            if (console == null) //ohh no no console let's add a new one
            {
                console = new GameConsole(this.Game);
                this.Game.Components.Add(console);  //add a new game console to Game
            }

            //WhichLost = false;
#if DEBUG
            this.ShowMarkers = true;
#endif
        }

        public void LaunchBall(GameTime gameTime)
        {
            this.Speed = 190; //hard coded speed TODO fix this
            this.Direction = LaunchDirection; //hard coded launch direction TODO fix this
            this.State = BallState.Playing;
            this.console.GameConsoleWrite("Ball Launched " + gameTime.TotalGameTime.ToString());
        }

        protected override void LoadContent()
        {
            this.spriteTexture = this.Game.Content.Load<Texture2D>("ballSmall");
            base.LoadContent();
        }

        private void resetBall(GameTime gameTime)
        {
            this.Speed = 0;
            this.State = BallState.OnPaddleStart;
            this.console.GameConsoleWrite("Ball Reset " + gameTime.TotalGameTime.ToString());
        }

        public override void Update(GameTime gameTime)
        {
            switch (this.State)
            {
                case BallState.OnPaddleStart:
                    break;

                case BallState.Playing:
                    UpdateBall(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdateBall(GameTime gameTime)
        {
            this.Location += this.Direction * (this.Speed * gameTime.ElapsedGameTime.Milliseconds / 1000);

            //P2 Fault
            if (this.Location.X + this.spriteTexture.Width > this.Game.GraphicsDevice.Viewport.Width)
            {
                Utils.P2Lost = true;
                this.resetBall(gameTime);
                //this.Direction.X *= -1;
            }
            if (this.Location.X < 0)//P1 Fault
            {
                Utils.P1Lost = true;
                this.resetBall(gameTime);
            }
            if (this.Location.Y + this.spriteTexture.Height > this.Game.GraphicsDevice.Viewport.Height)
            {
                this.Direction.Y *= -1;
                //this.resetBall(gameTime);
            }
            //Top
            if (this.Location.Y < 0)
            {
                this.Direction.Y *= -1;
            }
        }

        public void Reflect(MonogameBlock block)
        {
            //if()

            this.Direction.X *= -1; //TODO check for side collision with block
        }
    }
}
