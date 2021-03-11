using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class UIInteractableText : InteractableElement
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

        public UIInteractableText(string text, float textScale = 1f, bool large = false)
        {
            InternalSetText(text, textScale, large);
        }
        public override string GetConstructor()
        {
            return $"(\"{_text}\", {_textScale}, {_isLarge})";
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
            pos.Y -= (_isLarge ? 10f : 2f) * _textScale;
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

        private int _frame;
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Main.SelectedElement == this)
            {
                var dim = GetDimensions().ToRectangle();

                // Draw box
                spriteBatch.Draw(Main.MagicPixel, new Rectangle(dim.Left, dim.Top, dim.Width, 2), Color.White * .5f);
                spriteBatch.Draw(Main.MagicPixel, new Rectangle(dim.Left, dim.Bottom, dim.Width, 2), Color.White * .5f);
                spriteBatch.Draw(Main.MagicPixel, new Rectangle(dim.Left, dim.Top, 2, dim.Height), Color.White * .5f);
                spriteBatch.Draw(Main.MagicPixel, new Rectangle(dim.Right, dim.Top, 2, dim.Height), Color.White * .5f);

                _frame++;
                if ((_frame %= 40) <= 20)
                {
                    // Draw cursor
                    spriteBatch.Draw(Main.MagicPixel, new Rectangle((int)(dim.X + _textSize.X + (dim.Width - _textSize.X) / 2), dim.Y, 2, dim.Height), Color.White);
                }
            }
        }

        public override void KeyTyped(object sender, TextInputEventArgs args)
        {
            if (Main.SelectedElement == this)
            {
                if (args.Key == Keys.Back)
                {
                    if (Main.keyboard.IsKeyDown(Keys.LeftControl) || Main.keyboard.IsKeyDown(Keys.RightControl))
                    {
                        string[] words = Text.Split(' ');
                        SetText(string.Join(" ", words[0..^1]));
                    }
                    else if (Text.Length > 0)
                    {
                        SetText(Text[0..^1]);
                    }
                }
                else
                {
                    SetText(_text + args.Character.ToString());
                }
            }
            base.KeyTyped(sender, args);
        }
        public override void Click(UIMouseEvent evt)
        {
            base.Click(evt);
            Main.typing = true;
        }
    }
}
