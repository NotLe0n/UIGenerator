using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace UIGenerator.UI.UIElements.Interactable
{
    class UIInteractablePanel : InteractableElement
    {
        public override void OnActivate()
        {
            if (_borderTexture == null)
            {
                _borderTexture = Main.instance.Content.Load<Texture2D>("PanelBorder");
            }
            if (_backgroundTexture == null)
            {
                _backgroundTexture = Main.instance.Content.Load<Texture2D>("PanelBackground");
            }
        }

        public UIInteractablePanel()
        {
            Activate();
            SetPadding(CORNER_SIZE);
        }

        private void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point = new Point((int)dimensions.X, (int)dimensions.Y);
            Point point2 = new Point(point.X + (int)dimensions.Width - CORNER_SIZE, point.Y + (int)dimensions.Height - CORNER_SIZE);
            int width = point2.X - point.X - CORNER_SIZE;
            int height = point2.Y - point.Y - CORNER_SIZE;

            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, 0, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(0, CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, CORNER_SIZE, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point.Y, width, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE, 0, BAR_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point2.Y, width, CORNER_SIZE), new Rectangle?(new Rectangle(CORNER_SIZE, CORNER_SIZE + BAR_SIZE, BAR_SIZE, CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + CORNER_SIZE, CORNER_SIZE, height), new Rectangle?(new Rectangle(0, CORNER_SIZE, CORNER_SIZE, BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + CORNER_SIZE, CORNER_SIZE, height), new Rectangle?(new Rectangle(CORNER_SIZE + BAR_SIZE, CORNER_SIZE, CORNER_SIZE, BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + CORNER_SIZE, point.Y + CORNER_SIZE, width, height), new Rectangle?(new Rectangle(CORNER_SIZE, CORNER_SIZE, BAR_SIZE, BAR_SIZE)), color);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            DrawPanel(spriteBatch, _backgroundTexture, BackgroundColor);
            DrawPanel(spriteBatch, _borderTexture, BorderColor);
        }

        private static int CORNER_SIZE = 12;

        private static int BAR_SIZE = 4;

        private static Texture2D _borderTexture;

        private static Texture2D _backgroundTexture;

        public Color BorderColor = Color.Black;

        public Color BackgroundColor = new Color(63, 82, 151) * 0.7f;
    }
}
