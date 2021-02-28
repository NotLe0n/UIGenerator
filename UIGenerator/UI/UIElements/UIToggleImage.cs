using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIGenerator.UI.UIElements
{
    public class UIToggleImage : UIElement
    {
        public bool IsOn => _isOn;

        public UIToggleImage(Texture2D texture, int width, int height, Point onTextureOffset, Point offTextureOffset)
        {
            _onTexture = texture;
            _offTexture = texture;
            _offTextureOffset = offTextureOffset;
            _onTextureOffset = onTextureOffset;
            _drawWidth = width;
            _drawHeight = height;
            Width.Set(width, 0f);
            Height.Set(height, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Texture2D texture;
            Point point;

            if (_isOn)
            {
                texture = _onTexture;
                point = _onTextureOffset;
            }
            else
            {
                texture = _offTexture;
                point = _offTextureOffset;
            }
            Color color = IsMouseHovering ? Color.White : Color.Silver;

            spriteBatch.Draw(texture, 
                new Rectangle((int)dimensions.X, (int)dimensions.Y, _drawWidth, _drawHeight),
                new Rectangle?(new Rectangle(point.X, point.Y, _drawWidth, _drawHeight)),
                color);
        }

        public override void Click(UIMouseEvent evt)
        {
            Toggle();
            base.Click(evt);
        }

        public void SetState(bool value)
        {
            _isOn = value;
        }

        public void Toggle()
        {
            _isOn = !_isOn;
        }

        private Texture2D _onTexture;
        private Texture2D _offTexture;

        private int _drawWidth;
        private int _drawHeight;

        private Point _onTextureOffset = Point.Zero;
        private Point _offTextureOffset = Point.Zero;

        private bool _isOn;
    }
}
