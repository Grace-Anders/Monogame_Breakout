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

        ScoreManager sm;

        BlockManager bm;

        //static vaules
        public static GameManager gm;
        

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

            gm = new GameManager(this);
            this.Components.Add(gm);

            bm = new BlockManager(this, gm.ballOne, gm.ballTwo);
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

        bool Which; // P1 = true | P2 = false
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(GameManager.P1Lost == true)
            {
                Which = true;
                sm.DecreaseLives(Which);
                GameManager.P1Lost = false;
            }
            if (GameManager.P2Lost == true)
            {
                Which = false;
                sm.DecreaseLives(Which);
                GameManager.P2Lost = false;
            }

            if(GameManager.GameOver == true)
            {
                GameManager.WriteToConsole("A player has died and the game has restarted");

                gm.ballOne.State = BallState.OnPaddleStart;
                gm.ballTwo.State = BallState.OnPaddleStart;
                ScoreManager.SetupNewGame();
                bm.BlockReset();

                GameManager.GameOver = false;

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
