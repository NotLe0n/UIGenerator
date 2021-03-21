using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace UIGenerator.UI.UIElements
{
    class UIDynamicInput : UIElement
    {
        public delegate void changed(object value, UIElement elm);
        public event changed OnValueChanged;
        public object Value { get; set; }

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
        }

        #region constuctors
        #region simple Types
        public UIDynamicInput(object value)
        {
            Value = value;
            var input = MakeInput(value);
            input.OnKeyTyped += (evt, elm) =>
            {
                ValueChanged(Value, this);
            };
        }
        public UIDynamicInput(string value)
        {
            Value = value;
            var input = MakeInput(value);
            input.OnKeyTyped += (evt, elm) =>
            {
                if(input.Focused)
                    ValueChanged(input.Text, this);
            };
        }
        public UIDynamicInput(bool value)
        {
            Value = value;
            Height.Set(50, 0);

            var input = new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1));
            input.SetState(value);
            Append(input);

            var text = new UIText(input.IsOn.ToString());
            text.Left.Set(0, 0.3f);
            input.OnClick += (evt, elm) =>
            {
                text.SetText(input.IsOn.ToString());
                ValueChanged(input.IsOn, this);
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
                ValueChanged((byte)Value, this);
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
                ValueChanged((short)Value, this);
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
                ValueChanged((int)Value, this);
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
                ValueChanged((long)Value, this);
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
                ValueChanged((float)Value, this);
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
                ValueChanged((double)Value, this);
            };
        }
        public UIDynamicInput(UIElement value)
        {
            Value = value;
            var input = MakeInput(value.Id);
        }
        #endregion

        #region complex Types
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

            xInput.OnValueChanged += (value, elm) => ValueChanged(new Vector2((float)value, (float)yInput.Value), this);
            yInput.OnValueChanged += (value, elm) => ValueChanged(new Vector2((float)yInput.Value, (float)value), this);
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

            pixelInput.OnValueChanged += (value, elm) => ValueChanged(new StyleDimension((float)value, (float)precentInput.Value), this);
            precentInput.OnValueChanged += (value, elm) => ValueChanged(new StyleDimension((float)pixelInput.Value, (float)value), this);
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

            rInput.OnValueChanged += (value, elm) => ValueChanged(new Color((byte)value, (byte)gInput.Value, (byte)bInput.Value, (byte)aInput.Value), this);
            gInput.OnValueChanged += (value, elm) => ValueChanged(new Color((byte)rInput.Value, (byte)value, (byte)bInput.Value, (byte)aInput.Value), this);
            bInput.OnValueChanged += (value, elm) => ValueChanged(new Color((byte)rInput.Value, (byte)gInput.Value, (byte)value, (byte)aInput.Value), this);
            aInput.OnValueChanged += (value, elm) => ValueChanged(new Color((byte)rInput.Value, (byte)gInput.Value, (byte)bInput.Value, (byte)value), this);
        }
        public UIDynamicInput(CalculatedStyle value)
        {
            Value = value;
            Width.Set(200, 0);
            Height.Set(200, 0);
            const float size = 38;
            const float leftOffset = 20;

            var xText = new UIText("X:");
            Append(xText);

            var xInput = new UIDynamicInput(value.X);
            xInput.Left.Set(leftOffset, 0);
            xInput.Width.Set(0, 1);
            xInput.Height.Set(size, 0f);
            Append(xInput);

            var yText = new UIText("Y:");
            yText.Top.Set(size, 0);
            Append(yText);

            var yInput = new UIDynamicInput(value.Y);
            yInput.Width.Set(0, 1);
            yInput.Height.Set(size, 0f);
            yInput.Top.Set(size, 0);
            yInput.Left.Set(leftOffset, 0);
            Append(yInput);

            var widthText = new UIText("W:");
            widthText.Top.Set(size * 2, 0);
            Append(widthText);

            var widthInput = new UIDynamicInput(value.Width);
            widthInput.Width.Set(0, 1);
            widthInput.Height.Set(size, 0f);
            widthInput.Top.Set(size * 2, 0);
            widthInput.Left.Set(leftOffset, 0);
            Append(widthInput);

            var heightText = new UIText("H:");
            heightText.Top.Set(size * 3, 0);
            Append(heightText);

            var heightInput = new UIDynamicInput(value.Height);
            heightInput.Width.Set(0, 1);
            heightInput.Height.Set(size, 0f);
            heightInput.Top.Set(size * 3, 0);
            heightInput.Left.Set(leftOffset, 0);
            Append(heightInput);

            xInput.OnValueChanged += (value, elm) => ValueChanged(new CalculatedStyle((float)value, (float)yInput.Value, (float)widthInput.Value, (float)heightInput.Value), this);
            yInput.OnValueChanged += (value, elm) => ValueChanged(new CalculatedStyle((float)xInput.Value, (float)value, (float)widthInput.Value, (float)heightInput.Value), this);

            widthInput.OnValueChanged += (value, elm) => ValueChanged(new CalculatedStyle((float)xInput.Value, (float)yInput.Value, (float)value, (float)heightInput.Value), this);
            heightInput.OnValueChanged += (value, elm) => ValueChanged(new CalculatedStyle((float)xInput.Value, (float)yInput.Value, (float)widthInput.Value, (float)value), this);
        }
        public UIDynamicInput(Rectangle value)
        {
            Value = value;
            Width.Set(200, 0);
            Height.Set(200, 0);
            const float size = 38;
            const float leftOffset = 20;

            var xText = new UIText("X:");
            Append(xText);

            var xInput = new UIDynamicInput(value.X);
            xInput.Left.Set(leftOffset, 0);
            xInput.Width.Set(0, 1);
            xInput.Height.Set(size, 0f);
            Append(xInput);

            var yText = new UIText("Y:");
            yText.Top.Set(size, 0);
            Append(yText);

            var yInput = new UIDynamicInput(value.Y);
            yInput.Width.Set(0, 1);
            yInput.Height.Set(size, 0f);
            yInput.Top.Set(size, 0);
            yInput.Left.Set(leftOffset, 0);
            Append(yInput);

            var widthText = new UIText("W:");
            widthText.Top.Set(size * 2, 0);
            Append(widthText);

            var widthInput = new UIDynamicInput(value.Width);
            widthInput.Width.Set(0, 1);
            widthInput.Height.Set(size, 0f);
            widthInput.Top.Set(size * 2, 0);
            widthInput.Left.Set(leftOffset, 0);
            Append(widthInput);

            var heightText = new UIText("H:");
            heightText.Top.Set(size * 3, 0);
            Append(heightText);

            var heightInput = new UIDynamicInput(value.Height);
            heightInput.Width.Set(0, 1);
            heightInput.Height.Set(size, 0f);
            heightInput.Top.Set(size * 3, 0);
            heightInput.Left.Set(leftOffset, 0);
            Append(heightInput);

            xInput.OnValueChanged += (value, elm) => ValueChanged(new Rectangle((int)value, (int)yInput.Value, (int)widthInput.Value, (int)heightInput.Value), this);
            yInput.OnValueChanged += (value, elm) => ValueChanged(new Rectangle((int)xInput.Value, (int)value, (int)widthInput.Value, (int)heightInput.Value), this);

            widthInput.OnValueChanged += (value, elm) => ValueChanged(new Rectangle((int)xInput.Value, (int)yInput.Value, (int)value, (int)heightInput.Value), this);
            heightInput.OnValueChanged += (value, elm) => ValueChanged(new Rectangle((int)xInput.Value, (int)yInput.Value, (int)widthInput.Value, (int)value), this);
        }
        public UIDynamicInput(Array value)
        {
            var scrollbar = new UIScrollbar();
            scrollbar.Left.Set(0, 0.9f);
            scrollbar.Top.Set(0, 0.1f);
            scrollbar.Width.Set(20, 0);
            scrollbar.Height.Set(0, 0.8f);
            Append(scrollbar);

            var list = new UIList();
            list.Width.Set(0, 1f);
            list.Height.Set(0, 1f);
            list.ListPadding = 2f;
            list.SetScrollbar(scrollbar);
            Append(list);

            for (int i = 0; i < value.Length; i++)
            {
                var input = new UIDynamicInput(value.GetValue(i));
                input.Width.Set(0, 0.9f);
                input.Height.Set(20, 0);
                input.OnValueChanged += (val, elm) =>
                {
                    int index = list._items.IndexOf(elm);
                    value.SetValue(val, index);
                    ValueChanged(value, elm);
                };
                list.Add(input);
            }
        }
        public UIDynamicInput(Texture2D value)
        {
            Width.Set(value.Width, 0);
            Height.Set(value.Height, 0);

            var texture = new UIImage(value);
            texture.OnClick += (evt, elm) =>
            {
                using System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
                {
                    InitialDirectory = Main.instance.Content.RootDirectory,
                    Filter = "image files (*.png)|*.png",
                    RestoreDirectory = true
                };

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    var img = Texture2D.FromStream(Main.graphics.GraphicsDevice, openFileDialog.OpenFile());
                    img.Name = openFileDialog.SafeFileName.Replace(".png", "");
                    texture.SetImage(img);
                    ValueChanged(img, this);
                }
            };
            Append(texture);
        }
        #endregion

        #endregion
    }
}
