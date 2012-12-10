using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotOverlords.Utility;

namespace RobotOverlords
{
    public class Arrow
    {
        public readonly Texture2D LineTexture;

        public Arrow(Texture2D texture)
        {
            LineTexture = texture;
        }
        public Arrow(Texture2D texture, Color tint)
        {
            LineTexture = texture;
            Tint = tint;
        }

        public Color Tint { get; set; }
        public Vector2 Angle { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Destination
        {
            get
            {
                var stick = Normalize(Angle);
                var destination = Origin + stick * 100;
                return destination;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Artist.DrawLine(
                    spriteBatch,
                    LineTexture,
                    Origin,
                    Destination,
                    Tint);
        }

        private static Vector2 Normalize(Vector2 angle)
        {
            angle.Y *= -1;
            angle.Normalize();
            return angle;
        }
    }
}
