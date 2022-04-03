using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary.Util;

namespace Monogame_Breakout
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //Services
        InputHandler input;
        GameConsole console;

        //Components
        BlockManager bm;
        Paddle paddle;
        Ball ball;

        ScoreManager score;

        public Game1() : base()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //Services
            input = new InputHandler(this);
            console = new GameConsole(this);
            this.Components.Add(console);
#if RELEASE
            console.ToggleConsole(); //close the console
#endif
            this.Components.Add(input);

            score = new ScoreManager(this);
            this.Components.Add(score);

            //GameComponents
            ball = new Ball(this); //Ball first paddle and block manager depend on ball
            this.Components.Add(ball);
            paddle = new Paddle(this, ball);
            this.Components.Add(paddle);

            bm = new BlockManager(this, ball);
            this.Components.Add(bm);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
