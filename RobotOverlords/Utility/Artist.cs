using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotOverlords.Utility
{
    public static class Artist
    {
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D spr, Vector2 a, Vector2 b)
        {
            DrawLine(spriteBatch, spr, a, b, Color.White);
        }
        public static void DrawLine(SpriteBatch spriteBatch, Texture2D spr, Vector2 a, Vector2 b, Color col)
        {
            var origin = new Vector2(0.5f, 0f);
            var diff = b - a;
            var angle = (float)(Math.Atan2(diff.Y, diff.X)) - MathHelper.PiOver2;
            var scale = new Vector2(1.0f, diff.Length() / spr.Height);

            spriteBatch.Draw(spr, a, null, col, angle, origin, scale, SpriteEffects.None, 1.0f);
        }
    }
}
