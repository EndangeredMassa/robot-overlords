using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace RobotOverlords.Utility
{
    public static class GamePadExtensions
    {
        public static bool AnyState(Func<GamePadState, bool> func)
        {
            var gs = GamePad.GetState(PlayerIndex.One);
            if (func(gs))
                return true;
            
            gs = GamePad.GetState(PlayerIndex.Two);
            if (func(gs))
                return true;

            gs = GamePad.GetState(PlayerIndex.Three);
            if (func(gs))
                return true;

            gs = GamePad.GetState(PlayerIndex.Four);
            if (func(gs))
                return true;

            return false;
        }
    }
}
