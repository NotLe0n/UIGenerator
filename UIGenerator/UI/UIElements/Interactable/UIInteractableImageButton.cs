using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class UIInteractableImageButton : InteractableElement
    {
        [JsonIgnore]
        public Texture2D Texture
        {
            get => _texture;
            set => SetImage(value);
        }
        private Texture2D _texture;
        private float _visibilityActive = 1f;
        private float _visibilityInactive = 0.4f;

        public UIInteractableImageButton(Texture2D texture)
        {
            _texture = texture ?? Main.MagicPixel;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }

        public override string GetConstructor()
        {
            return $"(ModContent.GetTexture(\"{_texture.Name}\"))";
        }

        public void SetImage(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 position = GetDimensions().Position();
            spriteBatch.Draw(_texture, position, Color.White * (IsMouseHovering ? _visibilityActive : _visibilityInactive));
        }

        public override void MouseOver(UIMouseEvent evt)
        {
            base.MouseOver(evt);
        }

        public void SetVisibility(float whenActive, float whenInactive)
        {
            _visibilityActive = MathHelper.Clamp(whenActive, 0f, 1f);
            _visibilityInactive = MathHelper.Clamp(whenInactive, 0f, 1f);
        }
    }
}
