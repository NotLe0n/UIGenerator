﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;

namespace UIGenerator
{
    static class Extentions
    {
        // for extention or helper methods

        public static void DrawBoundary(this SpriteBatch spriteBatch, Texture2D texture, Rectangle rectangle, Color color, int lineWidth)
        {
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, lineWidth), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, lineWidth), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Left, rectangle.Top, lineWidth, rectangle.Height), color);
            spriteBatch.Draw(texture, new Rectangle(rectangle.Right, rectangle.Top, lineWidth, rectangle.Height), color);
        }

        /// <returns>A copy of the object</returns>
        public static T Clone<T>(this T source)
        {
            var serialized = JsonConvert.SerializeObject(source);
            return JsonConvert.DeserializeObject<T>(serialized);
        }

        /// <returns><b>true</b> if the key has just been pressed</returns>
        public static bool JustPressed(this KeyboardState s, Keys key)
        {
            return Input.lastKeyboard.IsKeyUp(key) && Input.keyboard.IsKeyDown(key);
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
        public static float GetPrecent(float value, float containerSize)
        {
            return 1 / (containerSize / value);
        }
        public static Vector2 GetPrecent(Vector2 value, Vector2 containerSize)
        {
            return Vector2.One / (containerSize / value);
        }
        public static Vector2 InvertTranslate(Vector2 vector)
        {
            Matrix invMatrix = Matrix.Invert(Main.SceneUI.SceneMatrix);
            return Vector2.Transform(vector / Main.SceneUI.SceneScale, invMatrix);
        }
        public static Vector2 InvertTranslate(Point point)
        {
            Matrix invMatrix = Matrix.Invert(Main.SceneUI.SceneMatrix);
            return Vector2.Transform(point.ToVector2(), invMatrix);
        }
    }
}
