//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using System;

//namespace UIGenerator.UI
//{
//    public class UIText : UIElement
//    {
//        private object _text = "";

//        private float _textScale = 1f;

//        private Vector2 _textSize = Vector2.Zero;

//        private bool _isLarge;

//        private Color _color = Color.White;

//        private bool _isWrapped;

//        public bool DynamicallyScaleDownToWidth;

//        private string _visibleText;

//        private string _lastTextReference;

//        public string Text => _text.ToString();

//        public float TextOriginX { get; set; }

//        public float WrappedTextBottomPadding { get; set; }

//        public bool IsWrapped
//        {
//            get => _isWrapped;
//            set
//            {
//                _isWrapped = value;
//                InternalSetText(_text, _textScale, _isLarge);
//            }
//        }

//        public Color TextColor
//        {
//            get => _color;
//            set => _color = value;
//        }

//        public event Action OnInternalTextChange;

//        public UIText(string text, float textScale = 1f, bool large = false)
//        {
//            TextOriginX = 0.5f;
//            IsWrapped = false;
//            WrappedTextBottomPadding = 20f;
//            InternalSetText(text, textScale, large);
//        }

//        public override void Recalculate()
//        {
//            InternalSetText(_text, _textScale, _isLarge);
//            base.Recalculate();
//        }

//        public void SetText(string text)
//        {
//            InternalSetText(text, _textScale, _isLarge);
//        }

//        public void SetText(string text, float textScale, bool large)
//        {
//            InternalSetText(text, textScale, large);
//        }

//        protected override void DrawSelf(SpriteBatch spriteBatch)
//        {
//            base.DrawSelf(spriteBatch);
//            VerifyTextState();
//            CalculatedStyle innerDimensions = GetInnerDimensions();
//            Vector2 pos = innerDimensions.Position();
//            if (_isLarge)
//            {
//                pos.Y -= 10f * _textScale;
//            }
//            else
//            {
//                pos.Y -= 2f * _textScale;
//            }
//            pos.X += (innerDimensions.Width - _textSize.X) * TextOriginX;
//            float num = _textScale;
//            if (DynamicallyScaleDownToWidth && _textSize.X > innerDimensions.Width)
//            {
//                num *= innerDimensions.Width / _textSize.X;
//            }
//            if (_isLarge)
//            {
//                Utils.DrawBorderStringBig(spriteBatch, _visibleText, pos, _color, num);
//            }
//            else
//            {
//                Utils.DrawBorderString(spriteBatch, _visibleText, pos, _color, num);
//            }
//        }

//        private void VerifyTextState()
//        {
//            if ((object)_lastTextReference != Text)
//            {
//                InternalSetText(_text, _textScale, _isLarge);
//            }
//        }

//        private void InternalSetText(object text, float textScale, bool large)
//        {
//            DynamicSpriteFont dynamicSpriteFont = large ? FontAssets.DeathText.get_Value() : FontAssets.MouseText.get_Value();
//            _text = text;
//            _isLarge = large;
//            _textScale = textScale;
//            _lastTextReference = _text.ToString();
//            if (IsWrapped)
//            {
//                _visibleText = dynamicSpriteFont.CreateWrappedText(_lastTextReference, GetInnerDimensions().Width / _textScale);
//            }
//            else
//            {
//                _visibleText = _lastTextReference;
//            }
//            Vector2 vector = dynamicSpriteFont.MeasureString(_visibleText);
//            Vector2 vector2 = _textSize = ((!IsWrapped) ? (new Vector2(vector.X, large ? 32f : 16f) * textScale) : (new Vector2(vector.X, vector.Y + WrappedTextBottomPadding) * textScale));
//            MinWidth.Set(vector2.X + PaddingLeft + PaddingRight, 0f);
//            MinHeight.Set(vector2.Y + PaddingTop + PaddingBottom, 0f);
//            if (OnInternalTextChange != null)
//            {
//                OnInternalTextChange();
//            }
//        }
//    }
//}
