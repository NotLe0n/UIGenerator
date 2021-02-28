namespace UIGenerator.UI.UIElements
{
    class UIDynamicInput<T> : UIElement
    {
        public UIDynamicInput(T value)
        {
            if (typeof(T) == typeof(string))
            {
                var input = new UIStringElement(value.ToString());
                Append(input);
            }
            else if (typeof(T) == typeof(int))
            {
                var input = new UIIntElement(value.ToString());
                Append(input);
            }
        }
    }
    class UIStringElement : UIInput<string>
    {
        public UIStringElement(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            Width.Set(0, 1f);
            Height.Set(0, 1f);
        }
    }
    class UIIntElement : UIInput<int>
    {
        public UIIntElement(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            Width.Set(0, 0.9f);
            Height.Set(0, 1f);

            int parsed = int.Parse(text.ToString());

            //var incDec = new UIIncDec(parsed);
            //incDec.Left.Set(0, 0.9f);
            //incDec.ValueChanged += () =>
            //{
            //    SetText(incDec.Value.ToString());
            //};
            //Append(incDec);
        }
    }
}
