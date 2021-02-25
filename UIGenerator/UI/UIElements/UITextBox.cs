using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using FontStashSharp;

namespace UIGenerator.UI
{
    public class UITextBox : UITextPanel<string>
    {
        public UITextBox(string text, float textScale = 1f, bool large = false) : base(text, textScale, large)
        {
        }
        
        public void Write(string text)
        {
            SetText(Text.Insert(_cursor, text));
            _cursor += text.Length;
        }

        public override void SetText(string text, float textScale, bool large)
        {
            if (text.ToString().Length > _maxLength)
            {
                text = text.ToString().Substring(0, _maxLength);
            }
            base.SetText(text, textScale, large);
            _cursor = Math.Min(Text.Length, _cursor);
        }

        public void SetTextMaxLength(int maxLength)
        {
            _maxLength = maxLength;
        }

        public void Backspace()
        {
            if (_cursor != 0)
            {
                SetText(Text.Substring(0, Text.Length - 1));
            }
        }

        public void CursorLeft()
        {
            if (_cursor != 0)
            {
                _cursor--;
            }
        }

        public void CursorRight()
        {
            if (_cursor < Text.Length)
            {
                _cursor++;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            _cursor = Text.Length;
            base.DrawSelf(spriteBatch);
            _frameCount++;
            if ((_frameCount %= 40) <= 20)
            {
                CalculatedStyle innerDimensions = GetInnerDimensions();
                Vector2 pos = innerDimensions.Position();
                Vector2 size = new Vector2((IsLarge ? Main.fontDeathText : Main.fontMouseText).MeasureString(Text.Substring(0, _cursor)).X, IsLarge ? 32f : 16f) * TextScale;

                if (IsLarge)
                {
                    pos.Y -= 8f * TextScale;
                }
                else
                {
                    pos.Y += 2f * TextScale;
                }
                pos.X += (innerDimensions.Width - TextSize.X) * 0.5f + size.X - (IsLarge ? 8f : 4f) * TextScale + 6f;

                if (IsLarge)
                {
                    var bigFont = Main.fontSystem.GetFont((int)(Main.fontDeathText.FontSize * TextScale));
                    spriteBatch.DrawString(bigFont, "|", pos, TextColor);
                    return;
                }
                var smolFont = Main.fontSystem.GetFont((int)(Main.fontMouseText.FontSize * TextScale));
                spriteBatch.DrawString(smolFont, "|", pos, TextColor);
            }
        }

        private int _cursor;

        private int _frameCount;

        private int _maxLength = 20;
    }
}
