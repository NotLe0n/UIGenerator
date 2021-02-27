using System;
using System.Collections.Generic;
using System.Text;
using UIGenerator.UI.UIElements;
using System.Reflection;

namespace UIGenerator.UI.UIStates
{
    class SelectElement : UIState
    {
        public override void OnInitialize()
        {
            var list = new UIList();
            list.Width.Set(0, 1f);
            list.Height.Set(0, 1f);
            list.Top.Set(0, 0.02f);
            list.ListPadding = 20f;
            Append(list);

            var scrollbar = new UIScrollbar();
            scrollbar.Left.Set(0, 0.93f);
            scrollbar.Top.Set(0, 0.1f);
            scrollbar.Width.Set(20, 0);
            scrollbar.Height.Set(0, 0.8f);
            scrollbar.ViewPosition = 0;
            list.SetScrollbar(scrollbar);
            Append(scrollbar);

            var fields = Main.SelectedElement.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            var properties = Main.SelectedElement.GetType().GetProperties();

            for (int i = 0; i < fields.Length; i++)
            {
                var text = new UIText(fields[i].Name + "\n");
                text.HAlign = 0.3f;
                list.Add(text);
            }
            for (int i = 0; i < properties.Length; i++)
            {
                var text = new UIText(properties[i].Name);
                text.HAlign = 0.3f;
                list.Add(text);
            }
            base.OnInitialize();
        }
        public override void Recalculate()
        {
            Width.Set(Main.SidebarArea.Width, 0);
            base.Recalculate();
        }
    }
}
