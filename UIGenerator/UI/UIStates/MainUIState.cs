﻿using Microsoft.Xna.Framework.Graphics;

namespace UIGenerator.UI.UIStates
{
    class MainUIState : UIState
    {
        public override void OnInitialize()
        {
            var panel = new UIPanel();
            panel.Width.Set(500, 0);
            panel.Height.Set(500, 0);
            Append(panel);

            var img = new UIImage(Main.instance.Content.Load<Texture2D>("test"));
            img.Left.Set(40, 0.5f);
            img.Top.Set(120, 0);
            panel.Append(img);
            base.OnInitialize();
        }
    }
}
