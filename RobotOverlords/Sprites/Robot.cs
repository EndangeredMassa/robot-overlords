using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotOverlords.Sprites
{
    public class Robot : MobileSprite
    {
        public Robot(Texture2D texture, Vector2 position) 
            : base(texture)
        {
            SpriteAnimation.Width  = 30;
            SpriteAnimation.Height = 30;

            SpriteAnimation.AddAnimation("leftstop",  0, 0, 1, 0.1f);
            SpriteAnimation.AddAnimation("left",      0, 0, 4, 0.1f);
            SpriteAnimation.AddAnimation("rightstop", 0, 1, 1, 0.1f);
            SpriteAnimation.AddAnimation("right",     0, 1, 4, 0.1f);
            SpriteAnimation.AddAnimation("upstop",    0, 2, 1, 0.1f);
            SpriteAnimation.AddAnimation("up",        0, 2, 4, 0.1f);

            SpriteAnimation.AddAnimation("shootright", 0, 3, 2, 0.1f);
            SpriteAnimation.AddAnimation("shootleft",  2, 3, 2, 0.1f);
            SpriteAnimation.AddAnimation("shootup",    0, 4, 2, 0.1f);
            SpriteAnimation.AddAnimation("shootdown",  2, 4, 2, 0.1f);

            SpriteAnimation.AddAnimation("dead",    0, 5, 1, 0.1f);
            SpriteAnimation.AddAnimation("explode", 1, 5, 2, 0.2f, "dead");
            SpriteAnimation.AddAnimation("victory", 3, 5, 1, 0.1f);
            
            SpriteAnimation.Scale = 1.5f;
            SpriteAnimation.CurrentAnimation = "rightstop";
            Position = position;
            SpriteAnimation.AutoRotate = false;
            IsPathing = false;
            IsMoving = false;
        }
    }
}
