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

            Utils.gm = new GameManager(this);
            this.Components.Add(Utils.gm);

            bm = new BlockManager(this, Utils.gm.ballOne, Utils.gm.ballTwo);
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
