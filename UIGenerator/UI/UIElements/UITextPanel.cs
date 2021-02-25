//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;

//namespace UIGenerator.UI
//{
//    public class UITextPanel<T> : UIPanel
//    {
//        protected T _text;

//        protected float _textScale = 1f;

//        protected Vector2 _textSize = Vector2.Zero;

//        protected bool _isLarge;

//        protected Color _color = Color.White;

//        protected bool _drawPanel = true;

//        public float TextHAlign = 0.5f;

//        public bool IsLarge => _isLarge;

//        public bool DrawPanel
//        {
//            get => _drawPanel;
//            set => _drawPanel = value;
//        }

//        public float TextScale
//        {
//            get => _textScale;
//            set => _textScale = value;
//        }

//        public Vector2 TextSize => _textSize;

//        public string Text
//        {
//            get
//            {
//                if (_text != null)
//                {
//                    return _text.ToString();
//                }
//                return "";
//            }
//        }

//        public Color TextColor
//        {
//            get => _color;
//            set => _color = value;
//        }

//        public UITextPanel(T text, float textScale = 1f, bool large = false)
//        {
//            SetText(text, textScale, large);
//        }

//        public override void Recalculate()
//        {
//            SetText(_text, _textScale, _isLarge);
//            base.Recalculate();
//        }

//        public void SetText(T text)
//        {
//            SetText(text, _textScale, _isLarge);
//        }

//        public virtual void SetText(T text, float textScale, bool large)
//        {
//            Vector2 textSize = new Vector2((large ? FontAssets.DeathText.get_Value() : FontAssets.MouseText.get_Value()).MeasureString(text.ToString()).X, large ? 32f : 16f) * textScale;
//            _text = text;
//            _textScale = textScale;
//            _textSize = textSize;
//            _isLarge = large;
//            MinWidth.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
//            MinHeight.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
//        }

//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            if (_drawPanel)
//            {
//                base.DrawSelf(spriteBatch);
//            }
//            DrawText(spriteBatch);
//        }

//        protected void DrawText(SpriteBatch spriteBatch)
//        {
//            CalculatedStyle innerDimensions = GetInnerDimensions();
//            Vector2 pos = innerDimensions.Position();
//            if (_isLarge)
//            {
//                pos.Y -= 10f * _textScale * _textScale;
//            }
//            else
//            {
//                pos.Y -= 2f * _textScale;
//            }
//            pos.X += (innerDimensions.Width - _textSize.X) * TextHAlign;
//            if (_isLarge)
//            {
//                Utils.DrawBorderStringBig(spriteBatch, Text, pos, _color, _textScale);
//            }
//            else
//            {
//                Utils.DrawBorderString(spriteBatch, Text, pos, _color, _textScale);
//            }
//        }
//    }
//}
