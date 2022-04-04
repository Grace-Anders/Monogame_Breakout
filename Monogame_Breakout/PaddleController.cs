using Microsoft.Xna.Framework;
using MonoGameLibrary.Util;
using Microsoft.Xna.Framework.Input;

namespace Monogame_Breakout
{
    public class PaddleController
    {
        InputHandler input;
        Ball ball; //may should delgate to parent
        public Vector2 Direction { get; private set; }

        public Keys Up, Down, Launch;

        public PaddleController(Game game, Ball ball)
        {
            input = (InputHandler)game.Services.GetService(typeof(IInputHandler));
            this.Direction = Vector2.Zero;
            this.ball = ball;   //need refernce to ball to be able to lanch ball could possibly use delegate here
        }

        public void HandleInput(GameTime gametime)
        {
            this.Direction = Vector2.Zero;  //Start with no direction on each new upafet

            //No need to sum input only uses left and right
            if (input.KeyboardState.IsKeyDown(Up))
            {
                this.Direction = new Vector2(0, -1);
            }
            if (input.KeyboardState.IsKeyDown(Down))
            {
                this.Direction = new Vector2(0, 1);
            }

            //Up launches ball
            if (input.KeyboardState.WasKeyPressed(Launch))
            {
                if (ball.State == BallState.OnPaddleStart) //Only Launch Ball is it's on paddle
                    this.ball.LaunchBall(gametime);
            }
        }
    }
}
