using Microsoft.Xna.Framework;
using System.Reflection;

namespace UIGenerator.UI.UIElements
{
    class UIDynamicInput : UIElement
    {
        public delegate void changed(object value, UIElement elm);
        public event changed OnValueChanged;
        public object Value { get; set; }
        public FieldInfo field;

        public UIInput<T> MakeInput<T>(T value)
        {
            MinHeight.Set(40, 0);

            var input = new UIInput<T>(value);
            input.Width.Set(0, 1);
            input.Height.Set(0, 1);
            Append(input);

            return input;
        }
        public void ValueChanged(object value, UIElement elm)
        {
            OnValueChanged?.Invoke(value, elm);
            if (field != null)
                field.SetValue(Main.SelectedElement, value);
        }
        public UIDynamicInput(string value)
        {
            Value = value;
            MakeInput(value);
        }
        public UIDynamicInput(bool value)
        {
            Value = value;

            var input = new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1));
            input.SetState(value);
            Append(input);

            var text = new UIText(input.IsOn.ToString());
            text.Left.Set(0, 0.3f);
            input.OnClick += (evt, elm) =>
            {
                text.SetText(input.IsOn.ToString());
                ValueChanged(Value, this);
            };
            Append(text);
        }
        public UIDynamicInput(byte value)
        {
            Value = value;
            var input = MakeInput(value);

            var incDec = new UIIncDecBYTE(value);
            incDec.Left.Set(0, 0.88f);
            incDec.ValueChanged += () =>
            {
                input.SetText(incDec.Value.ToString());
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
            input.Append(incDec);
            input.OnKeyTyped += (evt, elm) =>
            {
                incDec.Value = byte.TryParse(input.Text, out byte e) ? e : value;
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
        }
        public UIDynamicInput(short value)
        {
            Value = value;
            var input = MakeInput(value);

            var incDec = new UIIncDecSHORT(value);
            incDec.Left.Set(0, 0.88f);
            incDec.ValueChanged += () =>
            {
                input.SetText(incDec.Value.ToString());
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
            input.Append(incDec);
            input.OnKeyTyped += (evt, elm) =>
            {
                incDec.Value = short.TryParse(input.Text, out short e) ? e : value;
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
        }
        public UIDynamicInput(int value)
        {
            Value = value;
            var input = MakeInput(value);

            var incDec = new UIIncDecINT(value);
            incDec.Left.Set(0, 0.88f);
            incDec.ValueChanged += () =>
            {
                input.SetText(incDec.Value.ToString());
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
            input.Append(incDec);

            input.OnKeyTyped += (evt, elm) =>
            {
                incDec.Value = int.TryParse(input.Text, out int e) ? e : value;
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
        }
        public UIDynamicInput(long value)
        {
            Value = value;
            var input = MakeInput(value);

            var incDec = new UIIncDecLONG(value);
            incDec.Left.Set(0, 0.88f);
            incDec.ValueChanged += () =>
            {
                input.SetText(incDec.Value.ToString());
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
            input.Append(incDec);
            input.OnKeyTyped += (evt, elm) =>
            {
                incDec.Value = long.TryParse(input.Text, out long e) ? e : value;
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
        }
        public UIDynamicInput(float value)
        {
            Value = value;
            var input = MakeInput(value);
            input.Width.Set(0, 0.88f);

            var incDec = new UIIncDecFLOAT(value);
            incDec.Left.Set(0, 0.75f);
            incDec.ValueChanged += () =>
            {
                input.SetText(incDec.Value.ToString());
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
            input.Append(incDec);
            input.OnKeyTyped += (evt, elm) =>
            {
                incDec.Value = float.TryParse(input.Text, out float e) ? e : value;
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
        }
        public UIDynamicInput(double value)
        {
            Value = value;
            var input = MakeInput(value);

            var incDec = new UIIncDecDOUBLE(value);
            incDec.Left.Set(0, 0.88f);
            incDec.ValueChanged += () =>
            {
                input.SetText(incDec.Value.ToString());
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
            input.Append(incDec);
            input.OnKeyTyped += (evt, elm) =>
            {
                incDec.Value = double.TryParse(input.Text, out double e) ? e : value;
                Value = incDec.Value;
                ValueChanged(Value, this);
            };
        }
        public UIDynamicInput(Vector2 value)
        {
            Value = value;
            Width.Set(200, 0);
            Height.Set(150, 0);
            const float size = 40;
            const float leftOffset = 20;

            var xText = new UIText("x:");
            Append(xText);

            var xInput = new UIDynamicInput(value.X);
            xInput.Left.Set(leftOffset, 0);
            xInput.Width.Set(0, 1);
            xInput.Height.Set(size, 0f);
            Append(xInput);

            var yText = new UIText("y:");
            yText.Top.Set(0, 0.5f);
            Append(yText);

            var yInput = new UIDynamicInput(value.Y);
            yInput.Width.Set(0, 1);
            yInput.Height.Set(size, 0f);
            yInput.Top.Set(size - 2, 0f);
            yInput.Left.Set(leftOffset, 0f);
            Append(yInput);

            xInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Vector2((float)value, (float)yInput.Value), this);
            };

            yInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Vector2((float)yInput.Value, (float)value), this);
            };
        }
        public UIDynamicInput(StyleDimension value)
        {
            Value = value;
            Width.Set(400, 0);
            Height.Set(150, 0);
            const float size = 38;
            const float leftOffset = 60;

            var rText = new UIText("Pixels:");
            Append(rText);

            var pixelInput = new UIDynamicInput(value.Pixels);
            pixelInput.Left.Set(leftOffset, 0);
            pixelInput.MinWidth.Set(0, 1);
            pixelInput.Height.Set(size, 0f);
            Append(pixelInput);

            var precentText = new UIText("Precent:");
            precentText.Top.Set(size, 0);
            Append(precentText);

            var precentInput = new UIDynamicInput(value.Precent);
            precentInput.Width.Set(0, 1);
            precentInput.Height.Set(size, 0f);
            precentInput.Top.Set(size, 0);
            precentInput.Left.Set(leftOffset, 0);
            Append(precentInput);

            pixelInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new StyleDimension((float)value, (float)precentInput.Value), this);
            };

            precentInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new StyleDimension((float)pixelInput.Value, (float)value), this);
            };
        }
        public UIDynamicInput(Color value)
        {
            Value = value;
            Width.Set(200, 0);
            Height.Set(200, 0);
            const float size = 38;
            const float leftOffset = 20;

            var rText = new UIText("R:");
            Append(rText);

            var rInput = new UIDynamicInput(value.R);
            rInput.Left.Set(leftOffset, 0);
            rInput.Width.Set(0, 1);
            rInput.Height.Set(size, 0f);
            Append(rInput);

            var gText = new UIText("G:");
            gText.Top.Set(size, 0);
            Append(gText);

            var gInput = new UIDynamicInput(value.G);
            gInput.Width.Set(0, 1);
            gInput.Height.Set(size, 0f);
            gInput.Top.Set(size, 0);
            gInput.Left.Set(leftOffset, 0);
            Append(gInput);

            var bText = new UIText("B:");
            bText.Top.Set(size * 2, 0);
            Append(bText);

            var bInput = new UIDynamicInput(value.B);
            bInput.Width.Set(0, 1);
            bInput.Height.Set(size, 0f);
            bInput.Top.Set(size * 2, 0);
            bInput.Left.Set(leftOffset, 0);
            Append(bInput);

            var aText = new UIText("A:");
            aText.Top.Set(size * 3, 0);
            Append(aText);

            var aInput = new UIDynamicInput(value.A);
            aInput.Width.Set(0, 1);
            aInput.Height.Set(size, 0f);
            aInput.Top.Set(size * 3, 0);
            aInput.Left.Set(leftOffset, 0);
            Append(aInput);

            rInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Color((byte)value, (byte)gInput.Value, (byte)bInput.Value, (byte)aInput.Value), this);
            };

            gInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Color((byte)rInput.Value, (byte)value, (byte)bInput.Value, (byte)aInput.Value), this);
            };

            bInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Color((byte)rInput.Value, (byte)gInput.Value, (byte)value, (byte)aInput.Value), this);
            };

            aInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Color((byte)rInput.Value, (byte)gInput.Value, (byte)bInput.Value, (byte)value), this);
            };
        }
        public UIDynamicInput(CalculatedStyle value)
        {
            Value = value;
            Width.Set(200, 0);
            Height.Set(200, 0);
            const float leftOffset = 60;
            const float size = 40;

            var xText = new UIText("X:");
            Append(xText);

            var xInput = new UIDynamicInput(value.X);
            xInput.Left.Set(leftOffset, 0);
            xInput.Width.Set(0, 1);
            xInput.Height.Set(size, 0);
            Append(xInput);

            var yText = new UIText("Y:");
            yText.Top.Set(size * 2, 0);
            Append(yText);

            var yInput = new UIDynamicInput(value.Y);
            yInput.Width.Set(0, 1);
            yInput.Height.Set(size, 0);
            yInput.Top.Set(size * 2, 0);
            yInput.Left.Set(leftOffset, 0);
            Append(yInput);

            var widthText = new UIText("Width:");
            widthText.Top.Set(size * 3, 0);
            Append(widthText);

            var widthInput = new UIDynamicInput(value.Width);
            widthInput.Width.Set(0, 1);
            widthInput.Height.Set(size, 0);
            widthInput.Top.Set(size * 3, 0);
            widthInput.Left.Set(leftOffset, 0);
            Append(widthInput);

            var heightText = new UIText("Height:");
            heightText.Top.Set(size * 4, 0);
            Append(heightText);

            var heightInput = new UIDynamicInput(value.Height);
            heightInput.Width.Set(0, 1);
            heightInput.Height.Set(size, 0);
            heightInput.Top.Set(size * 4, 0);
            heightInput.Left.Set(leftOffset, 0);
            Append(heightInput);

            xInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new CalculatedStyle((byte)value, (byte)yInput.Value, (byte)widthInput.Value, (byte)heightInput.Value), this);
            };

            yInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new CalculatedStyle((byte)xInput.Value, (byte)value, (byte)widthInput.Value, (byte)heightInput.Value), this);
            };

            widthInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new CalculatedStyle((byte)xInput.Value, (byte)yInput.Value, (byte)value, (byte)heightInput.Value), this);
            };

            heightInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new CalculatedStyle((byte)xInput.Value, (byte)yInput.Value, (byte)widthInput.Value, (byte)value), this);
            };
        }
        public UIDynamicInput(Rectangle value)
        {
            Value = value;
            Width.Set(200, 0);
            Height.Set(200, 0);
            const float leftOffset = 60;
            const float size = 40;

            var xText = new UIText("X:");
            Append(xText);

            var xInput = new UIDynamicInput(value.X);
            xInput.Left.Set(leftOffset, 0);
            xInput.Width.Set(0, 1);
            xInput.Height.Set(size, 0);
            Append(xInput);

            var yText = new UIText("Y:");
            yText.Top.Set(size * 2, 0);
            Append(yText);

            var yInput = new UIDynamicInput(value.Y);
            yInput.Width.Set(0, 1);
            yInput.Height.Set(size, 0);
            yInput.Top.Set(size * 2, 0);
            yInput.Left.Set(leftOffset, 0);
            Append(yInput);

            var widthText = new UIText("Width:");
            widthText.Top.Set(size * 3, 0);
            Append(widthText);

            var widthInput = new UIDynamicInput(value.Width);
            widthInput.Width.Set(0, 1);
            widthInput.Height.Set(size, 0);
            widthInput.Top.Set(size * 3, 0);
            widthInput.Left.Set(leftOffset, 0);
            Append(widthInput);

            var heightText = new UIText("Height:");
            heightText.Top.Set(size * 4, 0);
            Append(heightText);

            var heightInput = new UIDynamicInput(value.Height);
            heightInput.Width.Set(0, 1);
            heightInput.Height.Set(size, 0);
            heightInput.Top.Set(size * 4, 0);
            heightInput.Left.Set(leftOffset, 0);
            Append(heightInput);

            xInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Rectangle((byte)value, (byte)yInput.Value, (byte)widthInput.Value, (byte)heightInput.Value), this);
            };

            yInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Rectangle((byte)xInput.Value, (byte)value, (byte)widthInput.Value, (byte)heightInput.Value), this);
            };

            widthInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Rectangle((byte)xInput.Value, (byte)yInput.Value, (byte)value, (byte)heightInput.Value), this);
            };

            heightInput.OnValueChanged += (value, elm) =>
            {
                ValueChanged(new Rectangle((byte)xInput.Value, (byte)yInput.Value, (byte)widthInput.Value, (byte)value), this);
            };
        }
    }
}
