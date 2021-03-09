using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIGenerator.UI.UIElements.Interactable
{
    class UIInteractableTextPanel<T> : UIInteractablePanel
    {
        public bool IsLarge => _isLarge;

        public bool DrawPanel
        {
            get => _drawPanel;
            set => _drawPanel = value;
        }

        public float TextScale
        {
            get => _textScale;
            set => _textScale = value;
        }

        public Vector2 TextSize => _textSize;

        public string Text
        {
            get
            {
                if (_text != null)
                {
                    return _text.ToString();
                }
                return "";
            }
        }

        public Color TextColor
        {
            get => _color;
            set => _color = value;
        }

        public UIInteractableTextPanel(T text, float textScale = 1f, bool large = false)
        {
            SetText(text, textScale, large);

            constructor = $"(\"{text}\", {textScale}, {large})";
        }

        public override void Recalculate()
        {
            SetText(_text, _textScale, _isLarge);
            base.Recalculate();
        }

        public void SetText(T text)
        {
            SetText(text, _textScale, _isLarge);
        }

        public virtual void SetText(T text, float textScale, bool large)
        {
            Vector2 textSize = new Vector2((large ? Main.fontDeathText : Main.fontMouseText).MeasureString(text.ToString()).X, large ? 32f : 16f) * textScale;
            textSize.Y = (large ? 32f : 16f) * textScale;
            _text = text;
            _textScale = textScale;
            _textSize = textSize;
            _isLarge = large;
            MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
            MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_drawPanel)
            {
                base.DrawSelf(spriteBatch);
            }
            CalculatedStyle innerDimensions = GetInnerDimensions();
            Vector2 pos = innerDimensions.Position();
            if (_isLarge)
            {
                pos.Y -= 10f * _textScale * _textScale;
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

        private T _text;
        private float _textScale = 1f;
        private Vector2 _textSize = Vector2.Zero;
        private bool _isLarge;
        private Color _color = Color.White;
        private bool _drawPanel = true;
    }
}
