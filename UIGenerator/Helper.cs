using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace UIGenerator
{
    static class Extentions
    {
        // for extention or helper methods

        /// <returns><b>true</b> if the key has just been pressed</returns>
        public static bool JustPressed(this KeyboardState s, Keys key)
        {
            return Main.lastKeyboard.IsKeyUp(key) && Main.keyboard.IsKeyDown(key);
        }

        /// <returns>The X and Y components of a <b>Vector3</b> as a <b>Vector2</b></returns>
        public static Vector2 ToVector2(this Vector3 v)
        {
            return new Vector2(v.X, v.Y);
        }
        public static Vector2 ToVector2(this System.Drawing.Point p)
        {
            return new Vector2(p.X, p.Y);
        }
        /// <returns> The Size of a Rectangle as a <b>Vector2</b></returns>
        public static Vector2 VectorSize(this Rectangle r)
        {
            return r.Size.ToVector2();
        }
        public static Vector2 RotatedBy(this Vector2 spinningpoint, double radians, Vector2 center = default)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            Vector2 vector = spinningpoint - center;
            Vector2 result = center;
            result.X += vector.X * cos - vector.Y * sin;
            result.Y += vector.X * sin + vector.Y * cos;
            return result;
        }
        public static Vector2 Size(this Texture2D tex)
        {
            return new Vector2(tex.Width, tex.Height);
        }
    }
    static class Helper
    {
        public static bool CheckAABBvAABBCollision(Vector2 position1, Vector2 dimensions1, Vector2 position2, Vector2 dimensions2)
        {
            return position1.X < position2.X + dimensions2.X && position1.Y < position2.Y + dimensions2.Y && position1.X + dimensions1.X > position2.X && position1.Y + dimensions1.Y > position2.Y;
        }
    }
}
