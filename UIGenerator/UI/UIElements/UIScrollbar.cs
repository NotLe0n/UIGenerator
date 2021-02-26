﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace UIGenerator.UI.UIElements
{
    public class UIScrollbar : UIElement
    {
        public float ViewPosition
        {
            get => _viewPosition;
            set => _viewPosition = MathHelper.Clamp(value, 0f, _maxViewSize - _viewSize);
        }

        private float _viewPosition;
        private float _viewSize = 1f;
        private float _maxViewSize = 20f;
        private bool _isDragging;
        private bool _isHoveringOverHandle;
        private float _dragYOffset;
        private Texture2D _texture;
        private Texture2D _innerTexture;

        public UIScrollbar()
        {
            Width.Set(20f, 0f);
            MaxWidth.Set(20f, 0f);
            _texture = Main.instance.Content.Load<Texture2D>("Scrollbar");
            _innerTexture = Main.instance.Content.Load<Texture2D>("ScrollbarInner");
            PaddingTop = 5f;
            PaddingBottom = 5f;
        }

        public void SetView(float viewSize, float maxViewSize)
        {
            viewSize = MathHelper.Clamp(viewSize, 0f, maxViewSize);
            _viewPosition = MathHelper.Clamp(_viewPosition, 0f, maxViewSize - viewSize);
            _viewSize = viewSize;
            _maxViewSize = maxViewSize;
        }

        public float GetValue()
        {
            return _viewPosition;
        }

        private Rectangle GetHandleRectangle()
        {
            CalculatedStyle innerDimensions = GetInnerDimensions();
            if (_maxViewSize == 0f && _viewSize == 0f)
            {
                _viewSize = 1f;
                _maxViewSize = 1f;
            }
            return new Rectangle((int)innerDimensions.X, (int)(innerDimensions.Y + innerDimensions.Height * (_viewPosition / _maxViewSize)) - 3, 20, (int)(innerDimensions.Height * (_viewSize / _maxViewSize)) + 7);
        }

        private void DrawBar(SpriteBatch spriteBatch, Texture2D texture, Rectangle dimensions, Color color)
        {
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y - 6, dimensions.Width, 6), new Rectangle?(new Rectangle(0, 0, texture.Width, 6)), color);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y, dimensions.Width, dimensions.Height), new Rectangle?(new Rectangle(0, 6, texture.Width, 4)), color);
            spriteBatch.Draw(texture, new Rectangle(dimensions.X, dimensions.Y + dimensions.Height, dimensions.Width, 6), new Rectangle?(new Rectangle(0, texture.Height - 6, texture.Width, 6)), color);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            CalculatedStyle innerDimensions = GetInnerDimensions();
            if (_isDragging)
            {
                float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - _dragYOffset;
                _viewPosition = MathHelper.Clamp(num / innerDimensions.Height * _maxViewSize, 0f, _maxViewSize - _viewSize);
            }
            Rectangle handleRectangle = GetHandleRectangle();
            Vector2 mousePosition = UserInterface.ActiveInstance.MousePosition;
            _isHoveringOverHandle = handleRectangle.Contains(new Point((int)mousePosition.X, (int)mousePosition.Y));
            DrawBar(spriteBatch, _texture, dimensions.ToRectangle(), Color.White);
            DrawBar(spriteBatch, _innerTexture, handleRectangle, Color.White * ((_isDragging || _isHoveringOverHandle) ? 1f : 0.85f));
        }

        public override void MouseDown(UIMouseEvent evt)
        {
            base.MouseDown(evt);
            if (evt.Target == this)
            {
                Rectangle handleRectangle = GetHandleRectangle();
                if (handleRectangle.Contains(new Point((int)evt.MousePosition.X, (int)evt.MousePosition.Y)))
                {
                    _isDragging = true;
                    _dragYOffset = evt.MousePosition.Y - handleRectangle.Y;
                    return;
                }
                CalculatedStyle innerDimensions = GetInnerDimensions();
                float num = UserInterface.ActiveInstance.MousePosition.Y - innerDimensions.Y - (handleRectangle.Height >> 1);
                _viewPosition = MathHelper.Clamp(num / innerDimensions.Height * _maxViewSize, 0f, _maxViewSize - _viewSize);
            }
        }

        public override void MouseUp(UIMouseEvent evt)
        {
            base.MouseUp(evt);
            _isDragging = false;
        }
    }
}
