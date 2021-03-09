using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class UIInteractableImageFramed : InteractableElement
    {
        public UIInteractableImageFramed(Texture2D texture, Rectangle frame)
        {
            _texture = texture ?? Main.MagicPixel;
            _frame = frame;
            Width.Set(_frame.Width, 0f);
            Height.Set(_frame.Height, 0f);

            constructor = $"(ModContent.GetTexture(\"{_texture.Name}\"), new Rectangle({frame.X}, {frame.Y}, {frame.Width}, {frame.Height}))";
        }

        public void SetImage(Texture2D texture, Rectangle frame)
        {
            _texture = texture;
            _frame = frame;
            Width.Set(_frame.Width, 0f);
            Height.Set(_frame.Height, 0f);
        }

        public void SetFrame(Rectangle frame)
        {
            _frame = frame;
            Width.Set(_frame.Width, 0f);
            Height.Set(_frame.Height, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 position = GetDimensions().Position();
            spriteBatch.Draw(_texture, position, new Rectangle?(_frame), Color);
        }

        private Texture2D _texture;

        private Rectangle _frame;

        public Color Color = Color.White;
    }
}
