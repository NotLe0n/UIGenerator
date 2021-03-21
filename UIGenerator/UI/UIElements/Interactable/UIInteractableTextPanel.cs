using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Globalization;

namespace UIGenerator.UI.UIElements.Interactable
{
    class UIInteractableTextPanel<T> : UIInteractablePanel
    {
        public bool IsLarge => isLarge;

        public bool DrawPanel { get; set; } = true;

        public float TextScale { get; set; } = 1f;

        public Vector2 TextSize => _textSize;

        public string Text
        {
            get => _text;
            set => SetText(value);
        }

        public Color TextColor
        {
            get => _color;
            set => _color = value;
        }

        private string _text;
        private Vector2 _textSize = Vector2.Zero;
        public bool isLarge;
        private Color _color = Color.White;

        public UIInteractableTextPanel(T text, float textScale = 1f, bool large = false)
        {
            SetText(text.ToString(), textScale, large);
        }

        public override string GetConstructor()
        {
            var ci = CultureInfo.CreateSpecificCulture("en-GB");
            return $"(\"{Text}\", {TextScale.ToString(ci)}, {isLarge})";
        }

        public override void Recalculate()
        {
            SetText(_text, TextScale, isLarge);
            base.Recalculate();
        }

        public void SetText(string text)
        {
            SetText(text, TextScale, isLarge);
        }

        public virtual void SetText(string text, float textScale, bool large)
        {
            Vector2 textSize = new Vector2((large ? Main.fontDeathText : Main.fontMouseText).MeasureString(text.ToString()).X, large ? 32f : 16f) * textScale;
            textSize.Y = (large ? 32f : 16f) * textScale;
            _text = text;
            TextScale = textScale;
            _textSize = textSize;
            isLarge = large;
            MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
            MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (DrawPanel)
            {
                base.DrawSelf(spriteBatch);
            }
            CalculatedStyle innerDimensions = GetInnerDimensions();
            Vector2 pos = innerDimensions.Position();
            if (isLarge)
            {
                pos.Y -= 10f * TextScale * TextScale;
            }
            else
            {
                pos.Y -= 2f * TextScale;
            }
            pos.X += (innerDimensions.Width - _textSize.X) * 0.5f;

            if (isLarge)
            {
                var bigFont = Main.fontSystem.GetFont((int)(Main.fontDeathText.FontSize * TextScale));
                spriteBatch.DrawString(bigFont, Text, pos, _color);
                return;
            }
            var smolFont = Main.fontSystem.GetFont((int)(Main.fontMouseText.FontSize * TextScale));
            spriteBatch.DrawString(smolFont, Text, pos, _color);
        }
    }
}
