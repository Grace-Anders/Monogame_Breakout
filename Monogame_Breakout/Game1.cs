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
        Paddle pOne;
        Ball ballOne;

        Paddle pTwo;
        Ball ballTwo;

        ScoreManager sm;

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

            sm = new ScoreManager(this);
            
            this.Components.Add(sm);

            //GameComponents
            ballOne = new Ball(this); //Ball first paddle and block manager depend on ball
            this.Components.Add(ballOne);
            pOne = new Paddle(this, ballOne);
            Utils.Paddles.Add(pOne);
            this.Components.Add(pOne);

            ballTwo = new Ball(this); //Ball first paddle and block manager depend on ball
            this.Components.Add(ballTwo);
            pTwo = new Paddle(this, ballTwo);
            Utils.Paddles.Add(pTwo);
            this.Components.Add(pTwo);



            bm = new BlockManager(this, ballOne, ballTwo);
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

            pOne.spriteTexture = Content.Load<Texture2D>("paddleOne");
            pOne.Location = new Vector2((0), (GraphicsDevice.Viewport.Height / 2 - (pOne.spriteTexture.Height / 2)));
            pOne.controller.Up = Keys.W;
            pOne.controller.Down = Keys.S;
            pOne.controller.Launch = Keys.LeftShift;
            ballOne.LaunchDirection = new Vector2(1, -1);

            pTwo.spriteTexture = Content.Load<Texture2D>("paddleTwo");
            pTwo.Location = new Vector2((GraphicsDevice.Viewport.Width - (pTwo.spriteTexture.Width)), (GraphicsDevice.Viewport.Height / 2 - (pTwo.spriteTexture.Height / 2)));
            pTwo.controller.Up = Keys.U;
            pTwo.controller.Down = Keys.J;
            pTwo.controller.Launch = Keys.RightShift;
            ballTwo.LaunchDirection = new Vector2(-1, -1);

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        bool Which; // P1 = true | P2 = false
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Utils.P1Lost == true)
            {
                Which = true;
                sm.DecreaseLives(Which);
                Utils.P1Lost = false;
            }
            if (Utils.P2Lost == true)
            {
                Which = false;
                sm.DecreaseLives(Which);
                Utils.P2Lost = false;
            }

            if(Utils.GameOver == true)
            {
                //Game over logic
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
