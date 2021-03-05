using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using UIGenerator.UI.UIElements;

namespace UIGenerator.UI.UIStates
{
    class Options : UIState
    {
        private UIToggleImage MakeToggle(int id, SceneUIState.BackgroundID bid, bool hide = true)
        {
            var Toggle = new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1));
            Toggle.OnClick += (evt, elm) =>
            {
                Main.SceneUI.currentBackground[id] = Toggle.IsOn == hide ? SceneUIState.BackgroundID.None : bid;
            };
            return Toggle;
        }

        public override void OnInitialize()
        {
            UIPanel panel = new UIPanel();
            panel.Left.Set(0, 0.8f);
            panel.Width.Set(0, 0.2f);
            panel.Height.Set(0, 0.7f);
            Append(panel);

            #region backgroundToggles
            var headerText = new UIText("Backgrounds: ", 1.2f);
            headerText.HAlign = 0.5f;
            headerText.VAlign = 0.01f;
            panel.Append(headerText);

            var textList = new UIList();
            textList.Width.Set(0, 0.4f);
            textList.Height.Set(0, 0.2f);
            textList.Top.Set(0, 0.05f);
            textList.Left.Set(0, 0.25f);
            textList.ListPadding = 2f;
            panel.Append(textList);

            var toggleList = new UIList();
            toggleList.Width.Set(20, 0);
            toggleList.Height.Set(0, 0.2f);
            toggleList.Top.Set(0, 0.05f);
            toggleList.Left.Set(0, 0.75f);
            toggleList.ListPadding = 6f;
            panel.Append(toggleList);

            // Toggles
            textList.Add(new UIText("Hide Background"));
            toggleList.Add(MakeToggle(0, SceneUIState.BackgroundID.Default));

            textList.Add(new UIText("Hide Hotbar"));
            toggleList.Add(MakeToggle(1, SceneUIState.BackgroundID.Hotbar));

            textList.Add(new UIText("Hide Minimap"));
            toggleList.Add(MakeToggle(2, SceneUIState.BackgroundID.Minimap));

            textList.Add(new UIText("Hide Inventory"));
            toggleList.Add(MakeToggle(3, SceneUIState.BackgroundID.Inventory));

            textList.Add(new UIText("Show NPC"));
            toggleList.Add(MakeToggle(4, SceneUIState.BackgroundID.NPC, false));

            textList.Add(new UIText("Show Angler"));
            toggleList.Add(MakeToggle(5, SceneUIState.BackgroundID.Angler, false));

            textList.Add(new UIText("Show Shop"));
            toggleList.Add(MakeToggle(6, SceneUIState.BackgroundID.Shop, false));
            #endregion

            #region sceneToggles
            // Scene size
            var sizeTitle = new UIText("Scene size", 1.2f);
            sizeTitle.Top.Set(0, 0.3f);
            sizeTitle.HAlign = 0.5f;
            panel.Append(sizeTitle);

            var widthInput = new UIDynamicInput(1920);
            widthInput.Top.Set(0, 0.35f);
            widthInput.HAlign = 0.1f;
            widthInput.Width.Set(0, 0.44f);
            widthInput.OnValueChanged += (val, elm) => Main.SceneUI.SceneWidth = (int)val;
            panel.Append(widthInput);

            var heightInput = new UIDynamicInput(1080);
            heightInput.Top.Set(0, 0.35f);
            heightInput.HAlign = 0.9f;
            heightInput.Width.Set(0, 0.44f);
            heightInput.OnValueChanged += (val, elm) => Main.SceneUI.SceneHeight = (int)val;
            panel.Append(heightInput);

            var snapTitle = new UIText("Snapping", 1.2f);
            snapTitle.Top.Set(0, 0.45f);
            snapTitle.HAlign = 0.5f;
            panel.Append(snapTitle);

            // Snapping
            var snapToggleTextX = new UIText("Toggle snapping X:");
            snapToggleTextX.Top.Set(0, 0.5f);
            snapToggleTextX.HAlign = 0.1f;
            panel.Append(snapToggleTextX);

            var snapToggleX = new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1));
            snapToggleX.SetState(Main.SceneUI.snapElements.Item1);
            snapToggleX.Left.Set(0, 0.5f);
            snapToggleX.Top.Set(0, 0.5f);
            snapToggleX.OnClick += (evt, elm) =>
            {
                Main.SceneUI.snapElements.Item1 = snapToggleX.IsOn;
            };
            panel.Append(snapToggleX);

            var snapToggleTextY = new UIText("Y:");
            snapToggleTextY.Top.Set(0, 0.5f);
            snapToggleTextY.HAlign = 0.65f;
            panel.Append(snapToggleTextY);

            var snapToggleY = new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1));
            snapToggleY.SetState(Main.SceneUI.snapElements.Item2);
            snapToggleY.Left.Set(0, 0.7f);
            snapToggleY.Top.Set(0, 0.5f);
            snapToggleY.OnClick += (evt, elm) =>
            {
                Main.SceneUI.snapElements.Item2 = snapToggleY.IsOn;
            };
            panel.Append(snapToggleY);

            var snapIntervalTitle = new UIText("Snap intervals", 1.2f);
            snapIntervalTitle.Top.Set(0, 0.55f);
            snapIntervalTitle.HAlign = 0.1f;
            panel.Append(snapIntervalTitle);

            var snapInterval = new UIDynamicInput(Main.SceneUI.snapIntervals);
            snapInterval.Top.Set(0, 0.6f);
            snapInterval.HAlign = 0.1f;
            snapInterval.Width.Set(0, 0.4f);
            snapInterval.Height.Set(0, 0.2f);
            snapInterval.OnValueChanged += (val, elm) => Main.SceneUI.snapIntervals = (float[])val;
            panel.Append(snapInterval);

            var snapRangeTitle = new UIText("Snap range", 1.2f);
            snapRangeTitle.Top.Set(0, 0.55f);
            snapRangeTitle.HAlign = 0.7f;
            panel.Append(snapRangeTitle);

            var snapRange = new UIDynamicInput(35);
            snapRange.Top.Set(0, 0.6f);
            snapRange.HAlign = 0.9f;
            snapRange.Width.Set(0, 0.44f);
            snapRange.OnValueChanged += (val, elm) => Main.SceneUI.snapRange = (int)val;
            panel.Append(snapRange);

            var gridToggleText = new UIText("Draw Grid: ");
            gridToggleText.Top.Set(0, 0.7f);
            gridToggleText.HAlign = 0.7f;
            panel.Append(gridToggleText);

            var gridToggle = new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1));
            gridToggle.SetState(Main.SceneUI.drawGrid);
            gridToggle.Left.Set(0, 0.8f);
            gridToggle.Top.Set(0, 0.7f);
            gridToggle.OnClick += (evt, elm) =>
            {
                Main.SceneUI.drawGrid = gridToggle.IsOn;
            };
            panel.Append(gridToggle);
            #endregion

            base.OnInitialize();
        }
    }
}
