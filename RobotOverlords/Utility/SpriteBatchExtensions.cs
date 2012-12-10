using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RobotOverlords.Utility
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch batch, Texture2D texture, Vector2 position)
        {
            batch.Draw(texture, position, Color.White);
        }
        public static void Draw(this SpriteBatch batch, Texture2D texture, Rectangle position)
        {
            batch.Draw(texture, position, Color.White);
        }

        public static void DrawBatch(this SpriteBatch batch, Action action)
        {
            batch.Begin();

            action.Invoke();

            batch.End();
        }
    }
}
