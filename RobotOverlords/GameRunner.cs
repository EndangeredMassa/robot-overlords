using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RobotOverlords.Sprites;
using RobotOverlords.Utility;

namespace RobotOverlords
{
    public class GameRunner : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _backgroundTexture;
        private Texture2D _titleTexture;
        private Texture2D _botTexture;

        private Color Player1Tint = new Color(255, 137, 141); //red
        private Color Player2Tint = new Color(112, 104, 255); //blue
        private Color Player3Tint = new Color(255, 253, 135); //yellow
        private Color Player4Tint = new Color(48, 150, 60);   //green

        private int _screenX;
        private int _screenY;

        private readonly List<MobileSprite> _bots = new List<MobileSprite>();
        private readonly List<PlayerHud> _playerHuds = new List<PlayerHud>();

        private bool _gameStarted = false;

        public GameRunner()
        {
            _graphics = new GraphicsDeviceManager(this);
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
            //_screenX = 1280;
            //_screenY = 720;

            _screenX = 800;
            _screenY = 600;
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            TextPrinter.Initialize(_spriteBatch, Content, @"Fonts\");
            TextPrinter.CurrentFont = "Lindsey20";
            TextPrinter.CurrentColor = Color.White;

            _backgroundTexture = Content.Load<Texture2D>(@"Textures\Battlefield");
            _titleTexture = Content.Load<Texture2D>(@"Textures\TitleScreen");
            _botTexture = Content.Load<Texture2D>(@"Textures\Basicbot");
 
            var arrowTexture = Content.Load<Texture2D>(@"Textures\SpawnArrow");
            var baseTexture = Content.Load<Texture2D>(@"TempBase");

            var hud = new PlayerHud(arrowTexture, baseTexture, Player1Tint, PlayerIndex.One, new Vector2(20, 5));
            _playerHuds.Add(hud);

            hud = new PlayerHud(arrowTexture, baseTexture, Player2Tint, PlayerIndex.Two, new Vector2(620, 5));
            _playerHuds.Add(hud);

            hud = new PlayerHud(arrowTexture, baseTexture, Player3Tint, PlayerIndex.Three, new Vector2(20, 330));
            _playerHuds.Add(hud);

            hud = new PlayerHud(arrowTexture, baseTexture, Player4Tint, PlayerIndex.Four, new Vector2(620, 330));
            _playerHuds.Add(hud);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (_gameStarted)
            {
                foreach (var bot in _bots)
                    UpdateSpriteMovement(bot, gameTime);

                foreach (var hud in _playerHuds)
                {
                    hud.TimeSinceDeploy += (float)gameTime.ElapsedGameTime.TotalSeconds;

                    var gs = GamePad.GetState(hud.PlayerIndex);
                    hud.Arrow.Angle = gs.ThumbSticks.Left;
                    
                    if (gs.Buttons.A == ButtonState.Pressed
                        && (gs.ThumbSticks.Left.Y != 0
                        || gs.ThumbSticks.Left.X != 0))
                    {
                        if (hud.TimeSinceDeploy >= 0.5f || _bots.Count == 0)
                        {
                            hud.TimeSinceDeploy = 0f;

                            //launch robot!
                            var origin = hud.Arrow.Origin + new Vector2(-15, -15); //adjustment for centering robot
                            var bot = new Robot(_botTexture, origin);
                            bot.SpriteAnimation.Tint = hud.Tint;
                            bot.IsMoving = true;

                            var diff = (hud.Arrow.Origin - hud.Arrow.Destination) * -10;
                            var destination = hud.Arrow.Origin + diff;
                            bot.Target = GetBotTarget(hud.Arrow.Origin, destination);

                            bool leftKey = (gs.ThumbSticks.Left.X < 0);
                            bool rightKey = (gs.ThumbSticks.Left.X >= 0);
                            bool upKey = (gs.ThumbSticks.Left.Y > 0);
  
                            if (upKey)
                                bot.SpriteAnimation.CurrentAnimation = "up";

                            if (leftKey && !upKey)
                                bot.SpriteAnimation.CurrentAnimation = "left";

                            if (rightKey && !upKey)
                                bot.SpriteAnimation.CurrentAnimation = "right";
                            
                            bot.EndPathAnimation = "explode";
                            _bots.Add(bot);
                        }
                    }

                    hud.Base.Update(gameTime);
                }
            }
            else
            {
                var startKey = GamePadExtensions.AnyState(gs => (gs.Buttons.Start == ButtonState.Pressed));
                if (startKey)
                    _gameStarted = true;
            }
            
            base.Update(gameTime);
        }

        //The endpoint for the bot will be some far future target point
        //Here, we'll find where that intersects with a screen border so that
        // we can properly set a destination for the robot animation
        private Vector2 GetBotTarget(Vector2 start, Vector2 end)
        {
            Vector2 hitPoint;

            var hitTop = IntersectionPoint(new Vector2(0,0), new Vector2(_screenX, 0), start, end, out hitPoint);
            if (hitTop)
                return hitPoint;

            var hitLeft = IntersectionPoint(new Vector2(0, 0), new Vector2(0, _screenY), start, end, out hitPoint);
            if (hitLeft)
                return hitPoint;

            var hitRight = IntersectionPoint(new Vector2(_screenX, 0), new Vector2(_screenX, _screenY), start, end, out hitPoint);
            if (hitRight)
                return hitPoint;

            var hitBottom = IntersectionPoint(new Vector2(0, _screenY), new Vector2(_screenX, _screenY), start, end, out hitPoint);
            if (hitBottom)
                return hitPoint;

            return end;
        }

        //Calculates the intersection point of two line segments, if it exists
        private static bool IntersectionPoint(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2, out Vector2 point)
        {
            // Equations to determine whether lines intersect
            double Ua = ((end2.X - start2.X) * (start1.Y - start2.Y) - (end2.Y - start2.Y) * (start1.X - start2.X)) /
                        ((end2.Y - start2.Y) * (end1.X - start1.X) - (end2.X - start2.X) * (end1.Y - start1.Y));

            double Ub = ((end1.X - start1.X) * (start1.Y - start2.Y) - (end1.Y - start1.Y) * (start1.X - start2.X)) /
                        ((end2.Y - start2.Y) * (end1.X - start1.X) - (end2.X - start2.X) * (end1.Y - start1.Y));

            if (Ua >= 0.0f && Ua <= 1.0f && Ub >= 0.0f && Ub <= 1.0f)
            {
                double x = start1.X + Ua * (end1.X - start1.X);
                double y = start1.Y + Ua * (end1.Y - start1.Y);

                point = new Vector2((float)x, (float)y);
                return true;
            }

            point = new Vector2();
            return false;
        }
        
        private static void UpdateSpriteMovement(MobileSprite sprite, GameTime gameTime)
        {
            bool isDead = (sprite.SpriteAnimation.CurrentAnimation == "dead");
            if (isDead)
            {
                sprite.IsVisible = false;
                sprite.Update(gameTime);
                return;
            }
            
            sprite.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.DrawBatch(() =>
            {
               if (_gameStarted)
                   DrawGame(gameTime);
               else
                   DrawStartGame(gameTime);
            });

            base.Draw(gameTime);
        }

        private void DrawGame(GameTime gameTime)
        {
            _spriteBatch.Draw(
                _backgroundTexture,
                new Rectangle(0, 0,
                    _screenX,
                    _screenY));

            foreach (var bot in _bots)
                bot.Draw(_spriteBatch);

            foreach (var hud in _playerHuds)
                hud.Draw(_spriteBatch);
        }

        private void DrawStartGame(GameTime gameTime)
        {
            _spriteBatch.Draw(
                _titleTexture,
                new Rectangle(0, 0,
                    _screenX,
                    _screenY),
                Color.White);
         
            TextPrinter.DrawText("High Quality!", 600, 120, "Lindsey20");
        }
    }
}
