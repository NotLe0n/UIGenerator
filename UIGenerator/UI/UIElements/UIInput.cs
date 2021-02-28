using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace UIGenerator.UI.UIElements
{
    class UIInput<T> : UITextBox
    {
        private bool first = true;
        public bool Focused { get; set; }
        private string _previewText;

        public UIInput(string previewText, float textScale = 1, bool large = false) : base(previewText, textScale, large)
        {
            _previewText = previewText;
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
                    if (typeof(T) == typeof(byte))
                    {
                        if (byte.TryParse(Text + args.Character.ToString(), out _)) {
                            Write(args.Character.ToString());
                        }
                    }
                    else if (typeof(T) == typeof(short))
                    {
                        if (short.TryParse(Text + args.Character.ToString(), out _)) {
                            Write(args.Character.ToString());
                        }
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        if (int.TryParse(Text + args.Character.ToString(), out _)) {
                            Write(args.Character.ToString());
                        }
                    }
                    else if (typeof(T) == typeof(long))
                    {
                        if (long.TryParse(Text + args.Character.ToString(), out _)) {
                            Write(args.Character.ToString());
                        }
                    }
                    else if (typeof(T) == typeof(float))
                    {
                        if (float.TryParse(Text + args.Character.ToString(), out _)) {
                            Write(args.Character.ToString());
                        }
                    }
                    else if (typeof(T) == typeof(double))
                    {
                        if (float.TryParse(Text + args.Character.ToString(), out _)) {
                            Write(args.Character.ToString());
                        }
                    }
                    else
                    {
                        Write(args.Character.ToString());
                    }
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
