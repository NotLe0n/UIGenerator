using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace UIGenerator.UI.UIElements
{
    public class UITextBox : UITextPanel<string>
    {
        public UITextBox(string text, float textScale = 1f, bool large = false) : base(text, textScale, large)
        {
        }

        public void Write(string text)
        {
            SetText(Text.Insert(cursor, text));
            cursor += text.Length;
        }

        public override void SetText(string text, float textScale, bool large)
        {
            if (text.ToString().Length > maxLength)
            {
                text = text.ToString().Substring(0, maxLength);
            }
            base.SetText(text, textScale, large);
            cursor = Math.Min(Text.Length, cursor);
        }

        public void SetTextMaxLength(int maxLength)
        {
            this.maxLength = maxLength;
        }

        public void Backspace()
        {
            if (cursor != 0)
            {
                SetText(Text[0..^1]);
            }
        }

        public void CursorLeft()
        {
            if (cursor != 0)
            {
                cursor--;
            }
        }

        public void CursorRight()
        {
            if (cursor < Text.Length)
            {
                cursor++;
            }
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            cursor = Text.Length;
            base.DrawSelf(spriteBatch);
            frameCount++;
            if ((frameCount %= 40) <= 20)
            {
                CalculatedStyle innerDimensions = GetInnerDimensions();
                Vector2 pos = innerDimensions.Position();
                Vector2 size = new Vector2((IsLarge ? Main.fontDeathText : Main.fontMouseText).MeasureString(Text.Substring(0, cursor)).X, IsLarge ? 32f : 16f) * TextScale;

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

        protected int cursor;

        protected int frameCount;

        protected int maxLength = 50;
    }
}
