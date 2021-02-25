using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace UIGenerator.UI
{
    public class UIList : UIElement
    {
        public int Count => _items.Count;

        public UIList()
        {
            _innerList.OverflowHidden = false;
            _innerList.Width.Set(0f, 1f);
            _innerList.Height.Set(0f, 1f);
            OverflowHidden = true;
            Append(_innerList);
        }

        public float GetTotalHeight()
        {
            return _innerListHeight;
        }

        public void Goto(ElementSearchMethod searchMethod)
        {
            for (int num = 0; num < _items.Count; num++)
            {
                if (searchMethod(_items[num]))
                {
                    _scrollbar.ViewPosition = _items[num].Top.Pixels;
                    return;
                }
            }
        }

        public float ViewPosition
        {
            get => _scrollbar.ViewPosition;
            set => _scrollbar.ViewPosition = value;
        }

        public virtual void Add(UIElement item)
        {
            _items.Add(item);
            _innerList.Append(item);
            UpdateOrder();
            _innerList.Recalculate();
        }

        public virtual void AddRange(IEnumerable<UIElement> items)
        {
            _items.AddRange(items);
            foreach (UIElement item in items)
            {
                _innerList.Append(item);
            }
            UpdateOrder();
            _innerList.Recalculate();
        }

        public virtual bool Remove(UIElement item)
        {
            _innerList.RemoveChild(item);
            UpdateOrder();
            return _items.Remove(item);
        }

        public virtual void Clear()
        {
            _innerList.RemoveAllChildren();
            _items.Clear();
        }

        public override void Recalculate()
        {
            base.Recalculate();
            UpdateScrollbar();
        }

        public override void ScrollWheel(UIScrollWheelEvent evt)
        {
            base.ScrollWheel(evt);
            if (_scrollbar != null)
            {
                _scrollbar.ViewPosition -= evt.ScrollWheelValue;
            }
        }

        public override void RecalculateChildren()
        {
            base.RecalculateChildren();
            float num = 0f;
            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].Top.Set(num, 0f);
                _items[i].Recalculate();
                CalculatedStyle outerDimensions = _items[i].GetOuterDimensions();
                num += outerDimensions.Height + ListPadding;
            }
            _innerListHeight = num;
        }

        private void UpdateScrollbar()
        {
            if (_scrollbar != null)
            {
                _scrollbar.SetView(GetInnerDimensions().Height, _innerListHeight);
            }
        }

        public void SetScrollbar(UIScrollbar scrollbar)
        {
            _scrollbar = scrollbar;
            UpdateScrollbar();
        }

        public void UpdateOrder()
        {
            _items.Sort(new Comparison<UIElement>(SortMethod));
            UpdateScrollbar();
        }

        public int SortMethod(UIElement item1, UIElement item2)
        {
            return item1.CompareTo(item2);
        }

        public override List<SnapPoint> GetSnapPoints()
        {
            List<SnapPoint> list = new List<SnapPoint>();
            if (GetSnapPoint(out SnapPoint point))
            {
                list.Add(point);
            }
            foreach (UIElement item in _items)
            {
                list.AddRange(item.GetSnapPoints());
            }
            return list;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            if (_scrollbar != null)
            {
                _innerList.Top.Set(0f - _scrollbar.GetValue(), 0f);
            }
            Recalculate();
        }

        public List<UIElement> _items = new List<UIElement>();
        protected UIScrollbar _scrollbar;
        internal UIElement _innerList = new UIInnerList();
        private float _innerListHeight;
        public float ListPadding = 5f;
        public delegate bool ElementSearchMethod(UIElement element);

        private class UIInnerList : UIElement
        {
            public override bool ContainsPoint(Vector2 point)
            {
                return true;
            }

            protected override void DrawChildren(SpriteBatch spriteBatch)
            {
                Vector2 position = Parent.GetDimensions().Position();
                Vector2 dimensions = new Vector2(Parent.GetDimensions().Width, Parent.GetDimensions().Height);
                foreach (UIElement element in Elements)
                {
                    Vector2 position2 = element.GetDimensions().Position();
                    Vector2 dimensions2 = new Vector2(element.GetDimensions().Width, element.GetDimensions().Height);
                    if (Helper.CheckAABBvAABBCollision(position, dimensions, position2, dimensions2))
                    {
                        element.Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
