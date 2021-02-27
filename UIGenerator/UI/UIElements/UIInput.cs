using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace UIGenerator.UI.UIElements
{
    class UIInput : UITextBox
    {
        private bool first = true;
        public bool Focused { get; set; }
        private string _previewText;

        public UIInput(string text, float textScale = 1, bool large = false) : base(text, textScale, large)
        {
            _previewText = text;
        }

        public override void Click(UIMouseEvent evt)
        {
            Focused = true;
            base.Click(evt);
        }

        public override void KeyTyped(object sender, TextInputEventArgs args)
        {
            if (Focused)
            {
                if (first)
                {
                    first = false;
                    SetText("");
                }

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
                else if (Text.Length < maxLength)
                {
                    Write(args.Character.ToString());
                }
            }
            base.KeyTyped(sender, args);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsMouseHovering && Main.mouseLeft)
            {
                Focused = false;
            }

            if (Text.Length == 0)
            {
                SetText(_previewText);
                first = true;
            }
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Focused)
            {
                frameCount = 21;
            }

            base.Draw(spriteBatch);
        }
    }
}
