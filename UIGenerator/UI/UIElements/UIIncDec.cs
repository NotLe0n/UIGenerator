using Microsoft.Xna.Framework.Graphics;
using System;

namespace UIGenerator.UI.UIElements
{
    class UIIncDecBYTE : UIElement
    {
        public byte Value
        {
            get => _value;
            set => _value = value;
        }
        private byte _value;
        public Action ValueChanged;


        public UIIncDecBYTE(byte value)
        {
            _value = value;
            Width.Set(40, 0);
            Height.Set(60, 0);

            UIImageButton up = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowUp"));
            up.Top.Set(0, -0.4f);
            up.OnClick += (evt, elm) =>
            {
                _value++;
                ValueChanged.Invoke();
            };
            Append(up);

            UIImageButton down = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowDown"));
            down.Top.Set(0, 0.4f);
            down.OnClick += (evt, elm) =>
            {
                _value--;
                ValueChanged.Invoke();
            };
            Append(down);
        }
    }
    class UIIncDecSHORT : UIElement
    {
        public short Value
        {
            get => _value;
            set => _value = value;
        }
        private short _value;
        public Action ValueChanged;


        public UIIncDecSHORT(short value)
        {
            _value = value;
            Width.Set(40, 0);
            Height.Set(60, 0);

            UIImageButton up = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowUp"));
            up.Top.Set(0, -0.4f);
            up.OnClick += (evt, elm) =>
            {
                _value++;
                ValueChanged.Invoke();
            };
            Append(up);

            UIImageButton down = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowDown"));
            down.Top.Set(0, 0.4f);
            down.OnClick += (evt, elm) =>
            {
                _value--;
                ValueChanged.Invoke();
            };
            Append(down);
        }
    }
    class UIIncDecINT : UIElement
    {
        public int Value
        {
            get => _value;
            set => _value = value;
        }
        private int _value;
        public Action ValueChanged;


        public UIIncDecINT(int value)
        {
            _value = value;
            Width.Set(40, 0);
            Height.Set(60, 0);

            UIImageButton up = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowUp"));
            up.Top.Set(0, -0.4f);
            up.OnClick += (evt, elm) =>
            {
                _value++;
                ValueChanged.Invoke();
            };
            Append(up);

            UIImageButton down = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowDown"));
            down.Top.Set(0, 0.4f);
            down.OnClick += (evt, elm) =>
            {
                _value--;
                ValueChanged.Invoke();
            };
            Append(down);
        }
    }
    class UIIncDecLONG : UIElement
    {
        public long Value
        {
            get => _value;
            set => _value = value;
        }
        private long _value;
        public Action ValueChanged;


        public UIIncDecLONG(long value)
        {
            _value = value;
            Width.Set(40, 0);
            Height.Set(60, 0);

            UIImageButton up = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowUp"));
            up.Top.Set(0, -0.4f);
            up.OnClick += (evt, elm) =>
            {
                _value++;
                ValueChanged.Invoke();
            };
            Append(up);

            UIImageButton down = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowDown"));
            down.Top.Set(0, 0.4f);
            down.OnClick += (evt, elm) =>
            {
                _value--;
                ValueChanged.Invoke();
            };
            Append(down);
        }
    }
    class UIIncDecFLOAT : UIElement
    {
        public float Value
        {
            get => _value;
            set => _value = value;
        }
        private float _value;
        public Action ValueChanged;

        public UIIncDecFLOAT(float value)
        {
            _value = value;
            Width.Set(100, 0);
            Height.Set(60, 0);

            UIImageButton up = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowUp"));
            up.Top.Set(0, -0.4f);
            up.OnClick += (evt, elm) =>
            {
                _value++;
                ValueChanged.Invoke();
            };
            Append(up);

            UIImageButton down = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowDown"));
            down.Top.Set(0, 0.4f);
            down.OnClick += (evt, elm) =>
            {
                _value--;
                ValueChanged.Invoke();
            };
            Append(down);
        }
    }
    class UIIncDecDOUBLE : UIElement
    {
        public double Value
        {
            get => _value;
            set => _value = value;
        }
        private double _value;
        public Action ValueChanged;


        public UIIncDecDOUBLE(double value)
        {
            _value = value;
            Width.Set(40, 0);
            Height.Set(60, 0);

            UIImageButton up = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowUp"));
            up.Top.Set(0, -0.4f);
            up.OnClick += (evt, elm) =>
            {
                _value++;
                ValueChanged.Invoke();
            };
            Append(up);

            UIImageButton down = new UIImageButton(Main.instance.Content.Load<Texture2D>("ArrowDown"));
            down.Top.Set(0, 0.4f);
            down.OnClick += (evt, elm) =>
            {
                _value--;
                ValueChanged.Invoke();
            };
            Append(down);
        }
    }
}
