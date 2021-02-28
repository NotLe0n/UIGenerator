using System;

namespace UIGenerator.UI.UIElements
{
    class UIIncDec : UIPanel
    {
        public int Value { get; private set; }
        public Action ValueChanged;


        public UIIncDec(int value)
        {
            Value = value;

            Width.Set(20, 0);
            Height.Set(100, 0);

            var down = new UIText("v");
            down.HAlign = 0.5f;
            down.Top.Set(0, 0.5f);
            down.Height.Set(20, 0f);
            down.OnClick += (evt, elm) =>
            {
                Value--;
                ValueChanged.Invoke();
            };
            Append(down);

            var up = new UIText("^", 1, true);
            up.HAlign = 0.5f;
            up.Height.Set(20, 0f);
            up.OnClick += (evt, elm) =>
            {
                Value++;
                ValueChanged.Invoke();
            };
            Append(up);
        }
    }
}
