using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Monogame_Breakout
{
    class BlockManager : DrawableGameComponent
    {
        public List<MonogameBlock> Blocks { get; private set; } //List of Blocks the are managed by Block Manager

        //Dependancy on Ball
        Ball ballOne, ballTwo;

        List<MonogameBlock> blocksToRemove; //list of block to remove probably because they were hit

        /// <summary>
        /// BlockManager hold a list of blocks and handles updating, drawing a block collision
        /// </summary>
        /// <param name="game">Reference to Game</param>
        /// <param name="ball">Refernce to Ball for collision</param>
        public BlockManager(Game game, Ball bOne, Ball bTwo)
            : base(game)
        {
            this.Blocks = new List<MonogameBlock>();
            this.blocksToRemove = new List<MonogameBlock>();

            this.ballOne = bOne;
            this.ballTwo = bTwo;
        }

        public override void Initialize()
        {
            LoadLevel();
            base.Initialize();
        }

        /// <summary>
        /// Replacable Method to Load a Level by filling the Blocks List with Blocks
        /// </summary>
        /// 

        int BlocksWide = 1;
        protected virtual void LoadLevel()
        {
            ballOne.Speed += ballOne.Speed * (.10f * ScoreManager.Level);
            if(ScoreManager.Level % 2 == 0)
            {
                BlocksWide++;
            }
            CreateBlockArrayByWidthAndHeight(BlocksWide, 13, 1);
        }

        /// <summary>
        /// Simple Level lays out multiple levels of blocks
        /// </summary>
        /// <param name="width">Number of blocks wide</param>
        /// <param name="height">Number of blocks high</param>
        /// <param name="margin">space between blocks</param>
        private void CreateBlockArrayByWidthAndHeight(int width, int height, int margin)
        {
            int Center = (GraphicsDevice.Viewport.Width / 2);

            MonogameBlock b;
            //Create Block Array based on with and hieght
            for (int w = 0; w < width; w++)
            {
                for (int h = 0; h < height; h++)
                {
                    b = new MonogameBlock(this.Game);
                    b.Initialize();
                    //b.Location = new Vector2(5 + (w * b.SpriteTexture.Width + (w * margin)), 50 + (h * b.SpriteTexture.Height + (h * margin)));
                    b.Location = new Vector2(Center - (w * (b.SpriteTexture.Width)) + (w * margin), 50 + (h * b.SpriteTexture.Height + (h * margin)));
                    Blocks.Add(b);
                }
            }
        }

        bool reflected; //the ball should only reflect once even if it hits two bricks
        public override void Update(GameTime gameTime)
        {
            this.reflected = false; //only reflect once per update
            UpdateCheckBlocksForCollision(gameTime);
            UpdateBlocks(gameTime);
            UpdateRemoveDisabledBlocks();


            base.Update(gameTime);
        }

        private void UpdateBlocks(GameTime gameTime)
        {
            foreach (var block in Blocks)
            {
                block.Update(gameTime);
            }
        }

        /// <summary>
        /// Removes disabled blocks from list
        /// </summary>
        private void UpdateRemoveDisabledBlocks()
        {
            //remove disabled blocks
            foreach (var block in blocksToRemove)
            {
                Blocks.Remove(block);
                //ScoreManager.Score++;
            }
            blocksToRemove.Clear();
        }

        private void UpdateCheckBlocksForCollision(GameTime gameTime)
        {
            foreach (MonogameBlock b in Blocks)
            {
                if (b.Enabled) //Only chack active blocks
                {
                    b.Update(gameTime); //Update Block
                    //Ball Collision
                    if (b.Intersects(ballOne)) //chek rectagle collision between ball and current block 
                    {
                        BlockHit(b, ballOne);
                    }
                    else if (b.Intersects(ballTwo))
                    {
                        BlockHit(b, ballTwo);
                    }
                }
            }
        }

        private void BlockHit(MonogameBlock b, Ball ball)
        {
            //hit
            b.HitByBall(ball);
            if (b.BlockState == BlockState.Broken)
                blocksToRemove.Add(b);  //Ball is hit add it to remove list
            if (!reflected) //only reflect once
            {
                ball.Reflect(b);
                this.reflected = true;
            }
        }

        public void BlockReset()
        {
            foreach (MonogameBlock b in Blocks)
            {
                blocksToRemove.Add(b);
            }
            UpdateRemoveDisabledBlocks();
            LoadLevel();
        }

        /// <summary>
        /// Block Manager Draws blocks they don't draw themselves
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            foreach (var block in this.Blocks)
            {
                if (block.Visible)   //respect block visible property
                    block.Draw(gameTime);
            }
            base.Draw(gameTime);
        }
    }
}
