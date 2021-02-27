using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using UIGenerator.UI.UIElements;

namespace UIGenerator.UI
{
    public class UIElement : IComparable
    {
        public string Id = "";
        public UIElement Parent;
        internal List<UIElement> Elements = new List<UIElement>();

        public StyleDimension Top;
        public StyleDimension Left;
        public StyleDimension Width;
        public StyleDimension Height;
        public StyleDimension MaxWidth = StyleDimension.Fill;
        public StyleDimension MaxHeight = StyleDimension.Fill;
        public StyleDimension MinWidth = StyleDimension.Empty;
        public StyleDimension MinHeight = StyleDimension.Empty;

        private bool _isInitialized;

        public bool OverflowHidden;

        public float PaddingTop;
        public float PaddingLeft;
        public float PaddingRight;
        public float PaddingBottom;

        public float MarginTop;
        public float MarginLeft;
        public float MarginRight;
        public float MarginBottom;
        public float HAlign;
        public float VAlign;

        private CalculatedStyle _innerDimensions;
        private CalculatedStyle _dimensions;
        private CalculatedStyle _outerDimensions;

        private static RasterizerState _overflowHiddenRasterizerState;

        protected bool _useImmediateMode;

        private SnapPoint _snapPoint;

        private bool _isMouseHovering;

        public delegate void MouseEvent(UIMouseEvent evt, UIElement listeningElement);

        public delegate void ScrollWheelEvent(UIScrollWheelEvent evt, UIElement listeningElement);

        public bool IsMouseHovering => _isMouseHovering;

        #region events
        public event MouseEvent OnMouseDown;
        public event MouseEvent OnMouseUp;
        public event MouseEvent OnClick;
        public event MouseEvent OnMouseOver;
        public event MouseEvent OnMouseOut;
        public event MouseEvent OnDoubleClick;
        public event MouseEvent OnRightMouseDown;
        public event MouseEvent OnRightMouseUp;
        public event MouseEvent OnRightClick;
        public event MouseEvent OnRightDoubleClick;
        public event MouseEvent OnMiddleMouseDown;
        public event MouseEvent OnMiddleMouseUp;
        public event MouseEvent OnMiddleClick;
        public event MouseEvent OnMiddleDoubleClick;
        public event MouseEvent OnXButton1MouseDown;
        public event MouseEvent OnXButton1MouseUp;
        public event MouseEvent OnXButton1Click;
        public event MouseEvent OnXButton1DoubleClick;
        public event MouseEvent OnXButton2MouseDown;
        public event MouseEvent OnXButton2MouseUp;
        public event MouseEvent OnXButton2Click;
        public event MouseEvent OnXButton2DoubleClick;
        public event ScrollWheelEvent OnScrollWheel;
        #endregion

        // Token: 0x06000A2D RID: 2605 RVA: 0x003C02AC File Offset: 0x003BE4AC
        public UIElement()
        {
            if (_overflowHiddenRasterizerState == null)
            {
                _overflowHiddenRasterizerState = new RasterizerState
                {
                    CullMode = CullMode.None,
                    ScissorTestEnable = true
                };
            }
        }

        public void SetSnapPoint(string name, int id, Vector2? anchor = null, Vector2? offset = null)
        {
            if (anchor == null)
            {
                anchor = new Vector2?(new Vector2(0.5f));
            }
            if (offset == null)
            {
                offset = new Vector2?(Vector2.Zero);
            }
            _snapPoint = new SnapPoint(name, id, anchor.Value, offset.Value);
        }

        public bool GetSnapPoint(out SnapPoint point)
        {
            point = _snapPoint;
            if (_snapPoint != null)
            {
                _snapPoint.Calculate(this);
            }
            return _snapPoint != null;
        }

        protected virtual void DrawSelf(SpriteBatch spriteBatch)
        {
        }

        protected virtual void DrawChildren(SpriteBatch spriteBatch)
        {
            foreach (UIElement uielement in Elements)
            {
                uielement.Draw(spriteBatch);
            }
        }

        public void Append(UIElement element)
        {
            element.Remove();
            element.Parent = this;
            Elements.Add(element);
            element.Recalculate();
        }

        public void Remove()
        {
            Parent?.RemoveChild(this);
        }

        public void RemoveChild(UIElement child)
        {
            Elements.Remove(child);
            child.Parent = null;
        }

        public void RemoveAllChildren()
        {
            foreach (UIElement uielement in Elements)
            {
                uielement.Parent = null;
            }
            Elements.Clear();
        }

        public bool HasChild(UIElement child)
        {
            return Elements.Contains(child);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            bool overflowHidden = OverflowHidden;
            bool useImmediateMode = _useImmediateMode;
            Rectangle scissorRectangle = spriteBatch.GraphicsDevice.ScissorRectangle;
            SamplerState anisotropicClamp = SamplerState.AnisotropicClamp;

            if (useImmediateMode)
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, _overflowHiddenRasterizerState, null, Main.UIScaleMatrix);
                DrawSelf(spriteBatch);
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, _overflowHiddenRasterizerState, null, Main.UIScaleMatrix);
            }
            else
            {
                DrawSelf(spriteBatch);
            }

            if (overflowHidden)
            {
                spriteBatch.End();
                Rectangle adjustedClippingRectangle = Rectangle.Intersect(GetClippingRectangle(spriteBatch), spriteBatch.GraphicsDevice.ScissorRectangle);
                spriteBatch.GraphicsDevice.ScissorRectangle = adjustedClippingRectangle;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, _overflowHiddenRasterizerState, null, Main.UIScaleMatrix);
            }

            DrawChildren(spriteBatch);

            if (overflowHidden)
            {
                RasterizerState rasterizerState = spriteBatch.GraphicsDevice.RasterizerState;
                spriteBatch.End();
                spriteBatch.GraphicsDevice.ScissorRectangle = scissorRectangle;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, anisotropicClamp, DepthStencilState.None, rasterizerState, null, Main.UIScaleMatrix);
            }
            //spriteBatch.Draw(Main.MagicPixel, _dimensions.ToRectangle(), Color.Blue * 0.05f);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (UIElement element in Elements)
            {
                element.Update(gameTime);
            }
        }

        public Rectangle GetClippingRectangle(SpriteBatch spriteBatch)
        {
            Vector2 vector = new Vector2(_innerDimensions.X, _innerDimensions.Y);
            Vector2 position = new Vector2(_innerDimensions.Width, _innerDimensions.Height) + vector;

            vector = Vector2.Transform(vector, Main.UIScaleMatrix);
            position = Vector2.Transform(position, Main.UIScaleMatrix);

            Rectangle result = new Rectangle((int)vector.X, (int)vector.Y, (int)(position.X - vector.X), (int)(position.Y - vector.Y));

            int width = spriteBatch.GraphicsDevice.Viewport.Width;
            int height = spriteBatch.GraphicsDevice.Viewport.Height;

            result.X = Math.Clamp(result.X, 0, width);
            result.Y = Math.Clamp(result.Y, 0, height);
            result.Width = Math.Clamp(result.Width, 0, width - result.X);
            result.Height = Math.Clamp(result.Height, 0, height - result.Y);
            return result;
        }

        public virtual List<SnapPoint> GetSnapPoints()
        {
            List<SnapPoint> list = new List<SnapPoint>();

            if (GetSnapPoint(out SnapPoint point))
            {
                list.Add(point);
            }

            foreach (UIElement element in Elements)
            {
                list.AddRange(element.GetSnapPoints());
            }
            return list;
        }

        public virtual void Recalculate()
        {
            CalculatedStyle parent = (Parent == null) ? UserInterface.ActiveInstance.GetDimensions() : Parent.GetInnerDimensions();

            if (Parent != null && Parent is UIList)
            {
                parent.Height = float.MaxValue;
            }

            CalculatedStyle style = default;

            style.X = Left.GetValue(parent.Width) + parent.X;
            style.Y = Top.GetValue(parent.Height) + parent.Y;

            float minWidth = MinWidth.GetValue(parent.Width);
            float maxWidth = MaxWidth.GetValue(parent.Width);
            float minHeight = MinHeight.GetValue(parent.Height);
            float maxHeight = MaxHeight.GetValue(parent.Height);

            style.Width = MathHelper.Clamp(Width.GetValue(parent.Width), minWidth, maxWidth);
            style.Height = MathHelper.Clamp(Height.GetValue(parent.Height), minHeight, maxHeight);

            style.Width += MarginLeft + MarginRight;
            style.Height += MarginTop + MarginBottom;

            style.X += parent.Width * HAlign - style.Width * HAlign;
            style.Y += parent.Height * VAlign - style.Height * VAlign;

            _outerDimensions = style;

            style.X += MarginLeft;
            style.Y += MarginTop;

            style.Width -= MarginLeft + MarginRight;
            style.Height -= MarginTop + MarginBottom;

            _dimensions = style;

            style.X += PaddingLeft;
            style.Y += PaddingTop;

            style.Width -= PaddingLeft + PaddingRight;
            style.Height -= PaddingTop + PaddingBottom;
            _innerDimensions = style;
            RecalculateChildren();
        }

        public UIElement GetElementAt(Vector2 point)
        {
            UIElement uIElement = null;
            foreach (UIElement element in Elements)
            {
                if (element.ContainsPoint(point))
                {
                    uIElement = element;
                    break;
                }
            }
            if (uIElement != null)
            {
                return uIElement.GetElementAt(point);
            }
            if (ContainsPoint(point))
            {
                return this;
            }
            return null;
        }

        public virtual bool ContainsPoint(Vector2 point)
        {
            return point.X > _dimensions.X && point.Y > _dimensions.Y && point.X < _dimensions.X + _dimensions.Width && point.Y < _dimensions.Y + _dimensions.Height;
        }

        public void SetPadding(float pixels)
        {
            PaddingBottom = pixels;
            PaddingLeft = pixels;
            PaddingRight = pixels;
            PaddingTop = pixels;
        }

        public virtual void RecalculateChildren()
        {
            foreach (UIElement uielement in Elements)
            {
                uielement.Recalculate();
            }
        }

        public CalculatedStyle GetInnerDimensions()
        {
            return _innerDimensions;
        }

        public CalculatedStyle GetDimensions()
        {
            return _dimensions;
        }

        public CalculatedStyle GetOuterDimensions()
        {
            return _outerDimensions;
        }

        public void CopyStyle(UIElement element)
        {
            Top = element.Top;
            Left = element.Left;
            Width = element.Width;
            Height = element.Height;
            PaddingBottom = element.PaddingBottom;
            PaddingLeft = element.PaddingLeft;
            PaddingRight = element.PaddingRight;
            PaddingTop = element.PaddingTop;
            HAlign = element.HAlign;
            VAlign = element.VAlign;
            MinWidth = element.MinWidth;
            MaxWidth = element.MaxWidth;
            MinHeight = element.MinHeight;
            MaxHeight = element.MaxHeight;
            Recalculate();
        }

        #region events
        public virtual void MouseDown(UIMouseEvent evt)
        {
            OnMouseDown?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.MouseDown(evt);
            }
        }

        public virtual void MouseUp(UIMouseEvent evt)
        {
            OnMouseUp?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.MouseUp(evt);
            }
        }

        public virtual void MouseOver(UIMouseEvent evt)
        {
            _isMouseHovering = true;
            OnMouseOver?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.MouseOver(evt);
            }
        }

        public virtual void MouseOut(UIMouseEvent evt)
        {
            _isMouseHovering = false;
            OnMouseOut?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.MouseOut(evt);
            }
        }

        public virtual void Click(UIMouseEvent evt)
        {
            OnClick?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.Click(evt);
            }
        }

        public virtual void DoubleClick(UIMouseEvent evt)
        {
            OnDoubleClick?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.DoubleClick(evt);
            }
        }

        public virtual void RightMouseDown(UIMouseEvent evt)
        {
            OnRightMouseDown?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.RightMouseDown(evt);
            }
        }

        public virtual void RightMouseUp(UIMouseEvent evt)
        {
            OnRightMouseUp?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.RightMouseUp(evt);
            }
        }

        public virtual void RightClick(UIMouseEvent evt)
        {
            OnRightClick?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.RightClick(evt);
            }
        }

        public virtual void RightDoubleClick(UIMouseEvent evt)
        {
            OnRightDoubleClick?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.RightDoubleClick(evt);
            }
        }

        public virtual void MiddleMouseDown(UIMouseEvent evt)
        {
            OnMiddleMouseDown?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.MiddleMouseDown(evt);
            }
        }

        public virtual void MiddleMouseUp(UIMouseEvent evt)
        {
            OnMiddleMouseUp?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.MiddleMouseUp(evt);
            }
        }

        public virtual void MiddleClick(UIMouseEvent evt)
        {
            OnMiddleClick?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.MiddleClick(evt);
            }
        }

        public virtual void MiddleDoubleClick(UIMouseEvent evt)
        {
            OnMiddleDoubleClick?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.MiddleDoubleClick(evt);
            }
        }

        public virtual void XButton1MouseDown(UIMouseEvent evt)
        {
            OnXButton1MouseDown?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.XButton1MouseDown(evt);
            }
        }

        public virtual void XButton1MouseUp(UIMouseEvent evt)
        {
            OnXButton1MouseUp?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.XButton1MouseUp(evt);
            }
        }

        public virtual void XButton1Click(UIMouseEvent evt)
        {
            OnXButton1Click?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.XButton1Click(evt);
            }
        }

        public virtual void XButton1DoubleClick(UIMouseEvent evt)
        {
            OnXButton1DoubleClick?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.XButton1DoubleClick(evt);
            }
        }

        public virtual void XButton2MouseDown(UIMouseEvent evt)
        {
            OnXButton2MouseDown?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.XButton2MouseDown(evt);
            }
        }

        public virtual void XButton2MouseUp(UIMouseEvent evt)
        {
            OnXButton2MouseUp?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.XButton2MouseUp(evt);
            }
        }

        public virtual void XButton2Click(UIMouseEvent evt)
        {
            OnXButton2Click?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.XButton2Click(evt);
            }
        }

        public virtual void XButton2DoubleClick(UIMouseEvent evt)
        {
            OnXButton2DoubleClick?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.XButton2DoubleClick(evt);
            }
        }

        public virtual void ScrollWheel(UIScrollWheelEvent evt)
        {
            OnScrollWheel?.Invoke(evt, this);
            if (Parent != null)
            {
                Parent.ScrollWheel(evt);
            }
        }
        #endregion

        public void Activate()
        {
            if (!_isInitialized)
            {
                Initialize();
            }
            OnActivate();
            foreach (UIElement uielement in Elements)
            {
                uielement.Activate();
            }
        }

        public virtual void OnActivate()
        {
        }

        public void Deactivate()
        {
            OnDeactivate();
            foreach (UIElement uielement in Elements)
            {
                uielement.Deactivate();
            }
        }

        public virtual void OnDeactivate()
        {
        }

        public void Initialize()
        {
            OnInitialize();
            _isInitialized = true;
        }

        public virtual void OnInitialize()
        {
        }

        public virtual int CompareTo(object obj)
        {
            return 0;
        }
    }
}
