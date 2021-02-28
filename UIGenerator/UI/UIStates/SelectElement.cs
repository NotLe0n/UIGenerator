using System.Reflection;
using UIGenerator.UI.UIElements;

namespace UIGenerator.UI.UIStates
{
    class SelectElement : UIState
    {
        public override void OnInitialize()
        {
            var list = new UIList();
            list.Width.Set(0, 1f);
            list.Height.Set(0, 0.9f);
            list.Top.Set(0, 0.1f);
            list.ListPadding = 20f;
            Append(list);

            var scrollbar = new UIScrollbar();
            scrollbar.Left.Set(0, 0.93f);
            scrollbar.Top.Set(0, 0.1f);
            scrollbar.Width.Set(20, 0);
            scrollbar.Height.Set(0, 0.8f);
            list.SetScrollbar(scrollbar);
            Append(scrollbar);

            var type = Main.SelectedElement.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            var properties = type.GetProperties();

            var typeText = new UIText(type.Name, 1, true);
            typeText.HAlign = 0.5f;
            typeText.VAlign = 0.05f;
            Append(typeText);

            for (int i = 0; i < fields.Length; i++)
            {
                var fieldText = new UIText(fields[i].Name);
                fieldText.HAlign = 0.3f;
                list.Add(fieldText);

                var fieldInput = new UIInput<string>(fields[i].GetValue(Main.SelectedElement).ToString());
                fieldInput.HAlign = 0.3f;
                fieldInput.Width.Set(0, 0.5f);
                //input.OnTextChange += (evt, elm) => fields[i].SetValue(Main.SelectedElement, value);
                list.Add(fieldInput);
            }
            for (int i = 0; i < properties.Length; i++)
            {
                var propertyText = new UIText(properties[i].Name);
                propertyText.HAlign = 0.3f;
                list.Add(propertyText);

                var propertyInput = new UIInput<string>(properties[i].GetValue(Main.SelectedElement).ToString());
                propertyInput.HAlign = 0.3f;
                propertyInput.Width.Set(0, 0.5f);
                list.Add(propertyInput);
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
