using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RobotOverlords
{
    public class GameRunnerBackup : Microsoft.Xna.Framework.Game
    {
        readonly GraphicsDeviceManager Graphics;
        SpriteBatch SpriteBatch;

        public GameRunnerBackup()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        VectorSprite mage;
        VectorSprite terra;

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            var mageTexture = Content.Load<Texture2D>("BlackMage");
            var terraTexture = Content.Load<Texture2D>("Terra");

            mage = new VectorSprite(mageTexture, new Vector2(200, 200), new Vector2(80,-90));
            terra = new VectorSprite(terraTexture, Vector2.Zero, new Vector2(-110, 80));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        void UpdateSprite(GameTime gameTime, VectorSprite sprite)
        {
            sprite.UpdatePosition(gameTime);


            var maxX = Graphics.GraphicsDevice.Viewport.Width - sprite.Width;
            const int minX = 0;
            var maxY = Graphics.GraphicsDevice.Viewport.Height - sprite.Height;
            const int minY = 0;

            // Check for bounce.
            //if (sprite.Position.X + sprite.Position.X > maxX)
            //{
            //    sprite.Speed.X *= -1;
            //    sprite.Position.X = maxX - sprite.Position.X;
            //}
            //else if (sprite.Position.X < minX)
            //{
            //    sprite.Speed.X *= -1;
            //    sprite.Position.X = minX;
            //}
            //if (sprite.Position.Y + sprite.Position.Y > maxY)
            //{
            //    sprite.Speed.Y *= -1;
            //    sprite.Position.Y = maxY - sprite.Position.Y;
            //}
            //else if (sprite.Position.Y < minY)
            //{
            //    sprite.Speed.Y *= -1;
            //    sprite.Position.Y = minY;
            //}

            // Check for bounce.
            if (sprite.Position.X > maxX)
            {
                sprite.Speed.X *= -1;
                sprite.Position.X = maxX;
            }

            else if (sprite.Position.X < minX)
            {
                sprite.Speed.X *= -1;
                sprite.Position.X = minX;
            }

            if (sprite.Position.Y > maxY)
            {
                sprite.Speed.Y *= -1;
                sprite.Position.Y = maxY;
            }

            else if (sprite.Position.Y < minY)
            {
                sprite.Speed.Y *= -1;
                sprite.Position.Y = minY;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            
            UpdateSprite(gameTime, terra);
            UpdateSprite(gameTime, mage);

            SpriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            SpriteBatch.Draw(terra.Sprite, terra.Position, Color.White);
            SpriteBatch.Draw(mage.Sprite, mage.Position, Color.White);
            SpriteBatch.End();

            if (terra.BoundingBox.Intersects(mage.BoundingBox))
            {
                if (IntersectsX(terra, mage))
                {
                    terra.Speed.X *= -1;
                    mage.Speed.X *= -1;
                }
                else if (IntersectsY(terra, mage))
                {
                    terra.Speed.Y *= -1;
                    mage.Speed.Y *= -1;
                }
                else
                {
                    throw new Exception("broked");
                }
            }
            base.Draw(gameTime);
        }

        public bool IntersectsX(VectorSprite a, VectorSprite b)
        {
            var deltaX1 = a.Position.X + a.Sprite.Width - b.Position.X;
            var deltaX2 = b.Position.X + b.Sprite.Width - a.Position.X;

            if (deltaX1 > 0 && deltaX1 <= 1)
                return true;

            if (deltaX2 > 0 && deltaX2 <= 1)
                return true;

            return false;
        }

        public bool IntersectsY(VectorSprite a, VectorSprite b)
        {
            var deltaY1 = a.Position.Y + a.Sprite.Height - b.Position.Y;
            var deltaY2 = b.Position.Y + b.Sprite.Height - a.Position.Y;

            if (deltaY1 > 0 && deltaY1 <= 1)
                return true;

            if (deltaY2 > 0 && deltaY2 <= 1)
                return true;
            
            return false;
        }
    }
}
