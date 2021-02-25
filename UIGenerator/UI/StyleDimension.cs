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
    }
}
