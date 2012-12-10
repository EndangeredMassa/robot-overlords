using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotOverlords.Sprites;

namespace RobotOverlords
{
    public class PlayerHud
    {
        private Color _tint;
        public Color Tint
        {
            get { return _tint; }
            set
            {
                Arrow.Tint = value;
                _tint = value;
            }
        }
        
        private Vector2 _position;
        public Vector2 Position
        {
            get { return _position; }
            set 
            { 
                Base.Position = value;
                Arrow.Origin = Base.Center;

                _position = value;
            }
        }

        public PlayerIndex PlayerIndex { get; set; }
        public Arrow Arrow;
        public Base Base;

        public float TimeSinceDeploy { get; set; }

        public PlayerHud(Texture2D arrowTexture, Texture2D baseTexture, Color tint, PlayerIndex player, Vector2 position)
        {
            PlayerIndex = player;

            Arrow = new Arrow(arrowTexture);
            Base = new Base(baseTexture);
            
            //do these last
            Position = position;
            Tint = tint;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Base.Draw(spriteBatch);
            Arrow.Draw(spriteBatch);
        }
    }
}
