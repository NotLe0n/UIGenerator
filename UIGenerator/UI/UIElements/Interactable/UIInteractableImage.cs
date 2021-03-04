using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class UIInteractableImage : InteractableElement
    {
        private Texture2D _texture;
        public float ImageScale = 1f;

        public UIInteractableImage(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }

        public void SetImage(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 position = GetDimensions().Position() + _texture.Size() * (1f - ImageScale) / 2f;
            spriteBatch.Draw(_texture, position, null, Color.White, 0f, Vector2.Zero, ImageScale, SpriteEffects.None, 0f);
        }
    }
}
