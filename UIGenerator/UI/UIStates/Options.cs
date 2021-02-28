﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using UIGenerator.UI.UIElements;

namespace UIGenerator.UI.UIStates
{
    class Options : UIState
    {
        private UIToggleImage MakeToggle(int id, Main.BackgroundID bid, bool hide = true)
        {
            var Toggle = new UIToggleImage(Main.toggle, 13, 13, new Point(17, 1), new Point(1, 1));
            Toggle.OnClick += (evt, elm) =>
            {
                Main.currentBackground[id] = Toggle.IsOn == hide ? Main.BackgroundID.None : bid;
            };
            return Toggle;
        }

        public override void OnInitialize()
        {
            var panel = new UIPanel();
            panel.Left.Set(0, 0.8f);
            panel.Width.Set(0, 0.2f);
            panel.Height.Set(0, 0.2f);
            Append(panel);

            var headerText = new UIText("Backgrounds: ", 1.2f);
            headerText.HAlign = 0.5f;
            headerText.VAlign = 0.05f;
            panel.Append(headerText);

            var textList = new UIList();
            textList.Width.Set(0, 0.4f);
            textList.Height.Set(0, 1f);
            textList.Top.Set(0, 0.2f);
            textList.Left.Set(0, 0.25f);
            textList.ListPadding = 2f;
            panel.Append(textList);

            var toggleList = new UIList();
            toggleList.Width.Set(20, 0);
            toggleList.Height.Set(0, 1f);
            toggleList.Top.Set(0, 0.2f);
            toggleList.Left.Set(0, 0.75f);
            toggleList.ListPadding = 7f;
            panel.Append(toggleList);

            // Toggles
            textList.Add(new UIText("Hide Background"));
            toggleList.Add(MakeToggle(0, Main.BackgroundID.Default));

            textList.Add(new UIText("Hide Hotbar"));
            toggleList.Add(MakeToggle(1, Main.BackgroundID.Hotbar));

            textList.Add(new UIText("Hide Minimap"));
            toggleList.Add(MakeToggle(2, Main.BackgroundID.Minimap));

            textList.Add(new UIText("Hide Inventory"));
            toggleList.Add(MakeToggle(3, Main.BackgroundID.Inventory));

            textList.Add(new UIText("Show NPC"));
            toggleList.Add(MakeToggle(4, Main.BackgroundID.NPC, false));

            textList.Add(new UIText("Show Angler"));
            toggleList.Add(MakeToggle(5, Main.BackgroundID.Angler, false));

            textList.Add(new UIText("Show Shop"));
            toggleList.Add(MakeToggle(6, Main.BackgroundID.Shop, false));

            base.OnInitialize();
        }
    }
}