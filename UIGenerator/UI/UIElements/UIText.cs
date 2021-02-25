using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FontStashSharp;

namespace UIGenerator.UI
{
    public class UIText : UIElement
    {
        public string Text => _text.ToString();
        private string _text = "";
        private float _textScale = 1f;
        private Vector2 _textSize = Vector2.Zero;
        private bool _isLarge;
        private Color _color = Color.White;

        public Color TextColor
        {
            get => _color;
            set => _color = value;
        }

        public UIText(string text, float textScale = 1f, bool large = false)
        {
            InternalSetText(text, textScale, large);
        }

        public override void Recalculate()
        {
            InternalSetText(_text, _textScale, _isLarge);
            base.Recalculate();
        }

        public void SetText(string text)
        {
            InternalSetText(text, _textScale, _isLarge);
        }


        public void SetText(string text, float textScale, bool large)
        {
            InternalSetText(text, textScale, large);
        }

        private void InternalSetText(string text, float textScale, bool large)
        {
            Vector2 textSize = new Vector2((large ? Main.fontDeathText : Main.fontMouseText).MeasureString(text).X, large ? 32f : 16f) * textScale;
            _text = text;
            _textScale = textScale;
            _textSize = textSize;
            _isLarge = large;
            MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
            MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            base.DrawSelf(spriteBatch);
            CalculatedStyle innerDimensions = GetInnerDimensions();
            Vector2 pos = innerDimensions.Position();
            if (_isLarge)
            {
                pos.Y -= 10f * _textScale;
            }
            else
            {
                pos.Y -= 2f * _textScale;
            }
            pos.X += (innerDimensions.Width - _textSize.X) * 0.5f;

            if (_isLarge)
            {
                var bigFont = Main.fontSystem.GetFont((int)(Main.fontDeathText.FontSize * _textScale));
                spriteBatch.DrawString(bigFont, Text, pos, _color);
                return;
            }
            var smolFont = Main.fontSystem.GetFont((int)(Main.fontMouseText.FontSize * _textScale));
            spriteBatch.DrawString(smolFont, Text, pos, _color);
        }
    }
}
