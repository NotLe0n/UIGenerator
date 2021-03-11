using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class UIInteractableToggleImage : InteractableElement
    {
        public bool IsOn => _isOn;

        public UIInteractableToggleImage(Texture2D texture)
        {
            _onTexture = texture ?? Main.MagicPixel;
            _offTexture = texture ?? Main.MagicPixel;
            _offTextureOffset = new Point(17, 1);
            _onTextureOffset = new Point(0, 0);
            _drawWidth = 13;
            _drawHeight = 13;
            Width.Set(13, 0f);
            Height.Set(13, 0f);
        }
        public override string GetConstructor()
        {
            return $"(TextureManager.Load(\"Images/UI/{(_onTexture ?? Main.MagicPixel).Name}\"), 13, 13, new Point(17, 1), new Point(0, 0))";
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
