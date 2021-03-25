using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;

namespace UIGenerator.UI.UIStates
{

    public class SceneUIState : UIState
    {
        private Vector2 _scenePos;
        public float SceneScale = 0.5f;
        public Matrix SceneMatrix;
        public int SceneWidth;
        public int SceneHeight;
        private Rectangle _sceneRect;
        public Rectangle SceneRect;
        public Vector2 AnchorPoint;

        public (bool x, bool y) snapElements;
        public float[] snapIntervals;
        public int snapRange;
        public bool drawGrid;
        public bool keepElementsInBounds;
        public bool usePrecent = true;
        public int ElementCount => CountChildren(this);

        [JsonIgnore]
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
            _scenePos = new Vector2(Main.ViewPort.Width / 2, Main.ViewPort.Height / 2);
            SceneWidth = Main.ViewPort.Width;
            SceneHeight = Main.ViewPort.Height + 30;

            SceneMatrix = Matrix.Identity
              * Matrix.CreateTranslation(_scenePos.X, _scenePos.Y, 0)
              * Matrix.CreateScale(SceneScale);

            snapIntervals = new float[] { 0.0f, 0.1f, 0.2f, 0.3f, 0.4f, 0.5f, 0.6f, 0.7f, 0.8f, 0.9f, 1.0f };
            snapRange = 35;
            snapElements = (false, false);

            Id = "this";
        }

        public override void Update(GameTime gameTime)
        {
            // Zooming
            if (Input.scrollwheel != 0 && Main.MouseOverScene)
            {
                SceneScale -= Input.scrollwheel * SceneScale;
                AnchorPoint = Input.mouse.Position.ToVector2();
            }
            SceneScale = Math.Clamp(SceneScale, 0.1f, 10);

            // Moving the scene
            if (Input.mouse.MiddleButton == ButtonState.Pressed)
            {
                _scenePos -= Input.mousedelta / SceneScale;
            }
            _scenePos = Vector2.Clamp(_scenePos, Vector2.Zero, Main.ViewPort.Bounds.VectorSize());

            // Update Camera
            SceneMatrix =
                Matrix.CreateTranslation(new Vector3(_scenePos - AnchorPoint, 0)) *
                Matrix.CreateTranslation(new Vector3(-AnchorPoint, 0)) *
                Matrix.CreateScale(SceneScale) *
                Matrix.CreateTranslation(new Vector3(AnchorPoint, 0));

            Width.Set(SceneWidth, 0);
            Height.Set(SceneHeight, 0);
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _sceneRect = new Rectangle(0, 0, SceneWidth, SceneHeight);
            SceneRect = new Rectangle((int)SceneMatrix.Translation.X, (int)SceneMatrix.Translation.Y, (int)(_sceneRect.Width * SceneScale), (int)(_sceneRect.Height * SceneScale));

            // Draw Background
            DrawBackground(spriteBatch);

            if (drawGrid)
            {
                DrawGrid(spriteBatch);
            }
            base.Draw(spriteBatch);
        }
        public void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Main.MagicPixel, _sceneRect, new Color(88, 88, 88));
            for (int i = 0; i < currentBackground.Length; i++)
            {
                switch (currentBackground[i])
                {
                    case BackgroundID.Default:
                        spriteBatch.Draw(Backgrounds[0], _sceneRect, Color.White);
                        break;
                    case BackgroundID.Hotbar:
                        spriteBatch.Draw(Backgrounds[1], _sceneRect, Color.White);
                        break;
                    case BackgroundID.Minimap:
                        spriteBatch.Draw(Backgrounds[2], _sceneRect, Color.White);
                        break;
                    case BackgroundID.Inventory:
                        if (currentBackground[2] == BackgroundID.Minimap)
                        {
                            spriteBatch.Draw(Backgrounds[3], _sceneRect, Color.White);
                        }
                        else
                        {
                            spriteBatch.Draw(Backgrounds[4], _sceneRect, Color.White);
                        }
                        break;
                    case BackgroundID.NPC:
                        spriteBatch.Draw(Backgrounds[5], _sceneRect, Color.White);
                        break;
                    case BackgroundID.Angler:
                        spriteBatch.Draw(Backgrounds[6], _sceneRect, Color.White);
                        break;
                    case BackgroundID.Shop:
                        spriteBatch.Draw(Backgrounds[7], _sceneRect, Color.White);
                        break;
                }
            }
        }

        public void DrawGrid(SpriteBatch spriteBatch)
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
    }
}
