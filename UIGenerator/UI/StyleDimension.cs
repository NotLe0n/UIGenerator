using System;
using System.Globalization;

namespace UIGenerator.UI
{
    public struct StyleDimension
    {
        public float Percent
        {
            get => Precent;
            set => Precent = value;
        }

        public static StyleDimension Fill = new StyleDimension(0f, 1f);
        public static StyleDimension Empty = new StyleDimension(0f, 0f);
        public float Pixels;
        public float Precent;

        public StyleDimension(float pixels, float precent)
        {
            Pixels = pixels;
            Precent = precent;
        }

        public void Set(float pixels, float precent)
        {
            Pixels = pixels;
            Precent = precent;
        }

        public float GetValue(float containerSize)
        {
            return Pixels + Precent * containerSize;
        }
        public override string ToString()
        {
            var ci = CultureInfo.CreateSpecificCulture("en-GB");
            return $"{MathF.Round(Pixels, 3).ToString(ci)}f, {MathF.Round(Precent, 3).ToString(ci)}f";
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static bool operator ==(StyleDimension s1, StyleDimension s2)
        {
            if (s1.Pixels == s2.Pixels && s1.Precent == s2.Precent)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(StyleDimension s1, StyleDimension s2)
        {
            if (s1.Pixels != s2.Pixels && s1.Precent != s2.Precent)
            {
                return true;
            }
            return false;
        }
    }
}
