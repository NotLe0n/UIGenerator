using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace UIGenerator.UI.UIStates
{

    public class SceneUIState : UIState
    {
        public Vector2 ScenePos;
        public float SceneScale = 0.5f;
        public Matrix SceneMatrix;
        public int SceneWidth;
        public int SceneHeight;
        public Rectangle SceneRect;

        public (bool x, bool y) snapElements;
        public float[] snapIntervals;
        public int snapRange;
        public bool drawGrid;
        public bool keepElementsInBounds;
        public bool usePrecent = true;

        public Texture2D[] Backgrounds;
        public BackgroundID[] currentBackground =
        {
            BackgroundID.Default,
            BackgroundID.Hotbar,
            BackgroundID.Minimap,
            BackgroundID.Inventory,
            BackgroundID.None,
            BackgroundID.None,
            BackgroundID.None
        };

        public enum BackgroundID
        {
            None = 0,
            Default = 1,
            Hotbar = 2,
            Minimap = 3,
            Inventory = 4,
            NPC = 5,
            Angler = 6,
            Shop = 7
        }

        public SceneUIState()
        {
            ScenePos = new Vector2(Main.ViewPort.Width / 2, Main.ViewPort.Height / 2);
            SceneWidth = Main.ViewPort.Width;
            SceneHeight = Main.ViewPort.Height + 30;

            SceneMatrix = Matrix.Identity
              * Matrix.CreateTranslation(ScenePos.X, ScenePos.Y, 0)
              * Matrix.CreateScale(SceneScale);

            snapIntervals = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
            snapRange = 35;
            snapElements = (false, false);

            Id = "this";
        }

        public override void Update(GameTime gameTime)
        {
            // Zooming
            if (Main.scrollwheel != 0 && Main.MouseOverScene)
            {
                SceneScale -= Main.scrollwheel;
            }
            SceneScale = Math.Clamp(SceneScale, 0.1f, 10);

            // Moving the scene
            if (Main.mouse.MiddleButton == ButtonState.Pressed)
            {
                ScenePos -= Main.mousedelta / SceneScale;
            }
            ScenePos = Vector2.Clamp(ScenePos,
                Main.ViewPort.Bounds.VectorSize() / -4 * SceneScale,
                Main.ViewPort.Bounds.VectorSize() / 2 / SceneScale);

            // Update Camera
            SceneMatrix =
                Matrix.CreateTranslation(new Vector3(ScenePos.X, ScenePos.Y, 0)) *
                Matrix.CreateScale(SceneScale);

            Width.Set(SceneWidth, 0);
            Height.Set(SceneHeight, 0);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SceneRect = new Rectangle(0, 0, SceneWidth, SceneHeight);
            // Draw Background elements
            spriteBatch.Draw(Main.MagicPixel, SceneRect, new Color(88, 88, 88));
            for (int i = 0; i < currentBackground.Length; i++)
            {
                switch (currentBackground[i])
                {
                    case BackgroundID.Default:
                        spriteBatch.Draw(Backgrounds[0], SceneRect, Color.White);
                        break;
                    case BackgroundID.Hotbar:
                        spriteBatch.Draw(Backgrounds[1], SceneRect, Color.White);
                        break;
                    case BackgroundID.Minimap:
                        spriteBatch.Draw(Backgrounds[2], SceneRect, Color.White);
                        break;
                    case BackgroundID.Inventory:
                        if (currentBackground[2] == BackgroundID.Minimap)
                        {
                            spriteBatch.Draw(Backgrounds[3], SceneRect, Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(Backgrounds[4], SceneRect, Color.White);
                        }
                        break;
                    case BackgroundID.NPC:
                        spriteBatch.Draw(Backgrounds[5], SceneRect, Color.White);
                        break;
                    case BackgroundID.Angler:
                        spriteBatch.Draw(Backgrounds[6], SceneRect, Color.White);
                        break;
                    case BackgroundID.Shop:
                        spriteBatch.Draw(Backgrounds[7], SceneRect, Color.White);
                        break;
                }
            }

            if (drawGrid)
            {
                for (int i = 0; i < snapIntervals.Length; i++)
                {
                    if (snapElements.x == true)
                    {
                        spriteBatch.Draw(Main.MagicPixel, new Rectangle((int)(SceneWidth * snapIntervals[i]), 0, 2, SceneHeight), Color.MediumPurple);
                        spriteBatch.Draw(Main.MagicPixel, new Rectangle((int)(SceneWidth * snapIntervals[i]), 0, snapRange, SceneHeight), Color.Purple * 0.1f);
                    }
                    if (snapElements.y == true)
                    {
                        spriteBatch.Draw(Main.MagicPixel, new Rectangle(0, (int)(SceneHeight * snapIntervals[i]), SceneWidth, 2), Color.MediumPurple);
                        spriteBatch.Draw(Main.MagicPixel, new Rectangle(0, (int)(SceneHeight * snapIntervals[i]), SceneWidth, snapRange), Color.Purple * 0.1f);
                    }
                }
            }
            base.Draw(spriteBatch);
        }
    }
}
