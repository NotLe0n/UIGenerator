using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace UIGenerator.UI.UIElements.Interactable
{
    public class UIInteractableImageFramed : InteractableElement
    {
        [JsonIgnore]
        public Texture2D Texture
        {
            get => _texture;
            set => SetImage(value, _frame);
        }
        public Rectangle Frame
        {
            get => _frame;
            set => SetFrame(value);
        }

        private Texture2D _texture;
        private Rectangle _frame;
        public Color Color = Color.White;

        public UIInteractableImageFramed(Texture2D texture, Rectangle frame)
        {
            _texture = texture ?? Main.MagicPixel;
            _frame = frame;
            Width.Set(_frame.Width, 0f);
            Height.Set(_frame.Height, 0f);
        }

        public override string GetConstructor()
        {
            return $"(ModContent.GetTexture(\"{_texture.Name}\"), new Rectangle({_frame.X}, {_frame.Y}, {_frame.Width}, {_frame.Height}))";
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
    }
}
