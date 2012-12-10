using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RobotOverlords.Utility;

namespace RobotOverlords.Sprites
{
    public class Base : MobileSprite
    {
        public Vector2 Center
        {
            get { return Position + new Vector2(75, 75); }
        }

        public Base(Texture2D texture) 
            : base(texture)
        {
            SpriteAnimation.Width  = 150;
            SpriteAnimation.Height = 150;

            //Add some variance to the frame length.
            var frameLength = (float)Randomizer.NextDouble(-0.05, 0.15) + 0.2f;

            SpriteAnimation.AddAnimation("healthy", 0, 0, 4, frameLength);
            SpriteAnimation.AddAnimation("damaged", 0, 1, 4, frameLength);
            SpriteAnimation.AddAnimation("critical", 0, 2, 4, frameLength);

            SpriteAnimation.CurrentAnimation = "healthy";
            SpriteAnimation.AutoRotate = false;
            IsPathing = false;
            IsMoving = false;
        }
    }
}
