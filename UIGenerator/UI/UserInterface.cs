using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace UIGenerator.UI
{
    public class UserInterface
    {
        public UIState CurrentState => _currentState;
        private UIState _currentState;

        private const double DOUBLE_CLICK_TIME = 500.0;
        private const double STATE_CHANGE_CLICK_DISABLE_TIME = 200.0;
        private const int MAX_HISTORY_SIZE = 32;
        private const int HISTORY_PRUNE_SIZE = 4;

        public static UserInterface ActiveInstance = new UserInterface();
        private List<UIState> _history = new List<UIState>();

        public Vector2 MousePosition;
        private bool _wasMouseDown;
        private bool _wasMouseRightDown;
        private bool _wasMouseMiddleDown;
        private bool _wasMouseXButton1Down;
        private bool _wasMouseXButton2Down;

        private UIElement _lastElementHover;
        private UIElement _lastElementDown;
        private UIElement _lastElementRightDown;
        private UIElement _lastElementMiddleDown;
        private UIElement _lastElementXButton1Down;
        private UIElement _lastElementXButton2Down;
        private UIElement _lastElementClicked;
        private UIElement _lastElementRightClicked;
        private UIElement _lastElementMiddleClicked;
        private UIElement _lastElementXButton1Clicked;
        private UIElement _lastElementXButton2Clicked;

        private double _lastMouseDownTime;
        private double _lastMouseRightDownTime;
        private double _lastMouseMiddleDownTime;
        private double _lastMouseXButton1DownTime;
        private double _lastMouseXButton2DownTime;
        private double _clickDisabledTimeRemaining;
        public bool IsVisible;

        public UserInterface()
        {
            ActiveInstance = this;
        }

        public void ResetLasts()
        {
            _lastElementHover = null;
            _lastElementDown = null;
            _lastElementRightDown = null;
            _lastElementClicked = null;
            _lastElementRightClicked = null;
        }

        public void Use()
        {
            if (ActiveInstance != this)
            {
                ActiveInstance = this;
                Recalculate();
                return;
            }
            ActiveInstance = this;
        }

        private void ResetState()
        {
            MousePosition = new Vector2(Main.mouseX, Main.mouseY);
            _wasMouseDown = Main.mouseLeft;
            _wasMouseRightDown = Main.mouseRight;
            _wasMouseMiddleDown = Main.mouseMiddle;
            _wasMouseXButton1Down = Main.mouseXButton1;
            _wasMouseXButton2Down = Main.mouseXButton2;
            _lastElementHover?.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));
            _lastElementHover = null;
            _lastElementDown = null;
            _lastElementRightDown = null;
            _lastElementClicked = null;
            _lastElementRightClicked = null;
            _lastMouseDownTime = 0.0;
            _lastMouseRightDownTime = 0.0;
            _lastMouseMiddleDownTime = 0.0;
            _lastMouseXButton1DownTime = 0.0;
            _lastMouseXButton2DownTime = 0.0;
            _clickDisabledTimeRemaining = Math.Max(_clickDisabledTimeRemaining, 200.0);
        }

        public void Update(GameTime time)
        {
            if (_currentState == null)
            {
                return;
            }

            MousePosition = new Vector2(Main.mouseX, Main.mouseY);
            bool mouseLeftDown = Main.mouseLeft && Main.hasFocus;
            bool mouseRightDown = Main.mouseRight && Main.hasFocus;
            bool mouseMiddleDown = Main.mouseMiddle && Main.hasFocus;
            bool mouseXButton1Down = Main.mouseXButton1 && Main.hasFocus;
            bool mouseXButton2Down = Main.mouseXButton2 && Main.hasFocus;

            UIElement uIElement = Main.hasFocus ? _currentState.GetElementAt(MousePosition) : null;
            _clickDisabledTimeRemaining = Math.Max(0.0, _clickDisabledTimeRemaining - time.ElapsedGameTime.TotalMilliseconds);
            bool clickAble = _clickDisabledTimeRemaining > 0.0;

            if (uIElement != _lastElementHover)
            {
                if (_lastElementHover != null)
                {
                    _lastElementHover.MouseOut(new UIMouseEvent(_lastElementHover, MousePosition));
                }
                if (uIElement != null)
                {
                    uIElement.MouseOver(new UIMouseEvent(uIElement, MousePosition));
                }
                _lastElementHover = uIElement;
            }

            if (mouseLeftDown && !_wasMouseDown && uIElement != null && !clickAble)
            {
                _lastElementDown = uIElement;
                uIElement.MouseDown(new UIMouseEvent(uIElement, MousePosition));
                if (_lastElementClicked == uIElement && time.TotalGameTime.TotalMilliseconds - _lastMouseDownTime < 500.0)
                {
                    uIElement.DoubleClick(new UIMouseEvent(uIElement, MousePosition));
                    _lastElementClicked = null;
                }
                _lastMouseDownTime = time.TotalGameTime.TotalMilliseconds;
            }
            else if (!mouseLeftDown && _wasMouseDown && _lastElementDown != null && !clickAble)
            {
                UIElement lastElementDown = _lastElementDown;
                if (lastElementDown.ContainsPoint(MousePosition))
                {
                    lastElementDown.Click(new UIMouseEvent(lastElementDown, MousePosition));
                    _lastElementClicked = _lastElementDown;
                }
                lastElementDown.MouseUp(new UIMouseEvent(lastElementDown, MousePosition));
                _lastElementDown = null;
            }

            if (mouseRightDown && !_wasMouseRightDown && uIElement != null && !clickAble)
            {
                _lastElementRightDown = uIElement;
                uIElement.RightMouseDown(new UIMouseEvent(uIElement, MousePosition));
                if (_lastElementRightClicked == uIElement && time.TotalGameTime.TotalMilliseconds - _lastMouseRightDownTime < 500.0)
                {
                    uIElement.RightDoubleClick(new UIMouseEvent(uIElement, MousePosition));
                    _lastElementRightClicked = null;
                }
                _lastMouseRightDownTime = time.TotalGameTime.TotalMilliseconds;
            }
            else if (!mouseRightDown && _wasMouseRightDown && _lastElementRightDown != null && !clickAble)
            {
                UIElement lastElementRightDown = _lastElementRightDown;
                if (lastElementRightDown.ContainsPoint(MousePosition))
                {
                    lastElementRightDown.RightClick(new UIMouseEvent(lastElementRightDown, MousePosition));
                    _lastElementRightClicked = _lastElementRightDown;
                }
                lastElementRightDown.RightMouseUp(new UIMouseEvent(lastElementRightDown, MousePosition));
                _lastElementRightDown = null;
            }

            if (mouseMiddleDown && !_wasMouseMiddleDown && uIElement != null && !clickAble)
            {
                _lastElementMiddleDown = uIElement;
                uIElement.MiddleMouseDown(new UIMouseEvent(uIElement, MousePosition));
                if (_lastElementMiddleClicked == uIElement && time.TotalGameTime.TotalMilliseconds - _lastMouseMiddleDownTime < 500.0)
                {
                    uIElement.MiddleDoubleClick(new UIMouseEvent(uIElement, MousePosition));
                    _lastElementMiddleClicked = null;
                }
                _lastMouseMiddleDownTime = time.TotalGameTime.TotalMilliseconds;
            }
            else if (!mouseMiddleDown && _wasMouseMiddleDown && _lastElementMiddleDown != null && !clickAble)
            {
                UIElement lastElementMiddleDown = _lastElementMiddleDown;
                if (lastElementMiddleDown.ContainsPoint(MousePosition))
                {
                    lastElementMiddleDown.MiddleClick(new UIMouseEvent(lastElementMiddleDown, MousePosition));
                    _lastElementMiddleClicked = _lastElementMiddleDown;
                }
                lastElementMiddleDown.MiddleMouseUp(new UIMouseEvent(lastElementMiddleDown, MousePosition));
                _lastElementMiddleDown = null;
            }

            if (mouseXButton1Down && !_wasMouseXButton1Down && uIElement != null && !clickAble)
            {
                _lastElementXButton1Down = uIElement;
                uIElement.XButton1MouseDown(new UIMouseEvent(uIElement, MousePosition));
                if (_lastElementXButton1Clicked == uIElement && time.TotalGameTime.TotalMilliseconds - _lastMouseXButton1DownTime < 500.0)
                {
                    uIElement.XButton1DoubleClick(new UIMouseEvent(uIElement, MousePosition));
                    _lastElementXButton1Clicked = null;
                }
                _lastMouseXButton1DownTime = time.TotalGameTime.TotalMilliseconds;
            }
            else if (!mouseXButton1Down && _wasMouseXButton1Down && _lastElementXButton1Down != null && !clickAble)
            {
                UIElement lastElementXButton1Down = _lastElementXButton1Down;
                if (lastElementXButton1Down.ContainsPoint(MousePosition))
                {
                    lastElementXButton1Down.XButton1Click(new UIMouseEvent(lastElementXButton1Down, MousePosition));
                    _lastElementXButton1Clicked = _lastElementXButton1Down;
                }
                lastElementXButton1Down.XButton1MouseUp(new UIMouseEvent(lastElementXButton1Down, MousePosition));
                _lastElementXButton1Down = null;
            }

            if (mouseXButton2Down && !_wasMouseXButton2Down && uIElement != null && !clickAble)
            {
                _lastElementXButton2Down = uIElement;
                uIElement.XButton2MouseDown(new UIMouseEvent(uIElement, MousePosition));
                if (_lastElementXButton2Clicked == uIElement && time.TotalGameTime.TotalMilliseconds - _lastMouseXButton2DownTime < 500.0)
                {
                    uIElement.XButton2DoubleClick(new UIMouseEvent(uIElement, MousePosition));
                    _lastElementXButton2Clicked = null;
                }
                _lastMouseXButton2DownTime = time.TotalGameTime.TotalMilliseconds;
            }
            else if (!mouseXButton2Down && _wasMouseXButton2Down && _lastElementXButton2Down != null && !clickAble)
            {
                UIElement lastElementXButton2Down = _lastElementXButton2Down;
                if (lastElementXButton2Down.ContainsPoint(MousePosition))
                {
                    lastElementXButton2Down.XButton2Click(new UIMouseEvent(lastElementXButton2Down, MousePosition));
                    _lastElementXButton2Clicked = _lastElementXButton2Down;
                }
                lastElementXButton2Down.XButton2MouseUp(new UIMouseEvent(lastElementXButton2Down, MousePosition));
                _lastElementXButton2Down = null;
            }

            if (Main.scrollwheel != 0 && uIElement != null)
            {
                uIElement.ScrollWheel(new UIScrollWheelEvent(uIElement, MousePosition, Main.scrollwheel));
            }
            _wasMouseDown = mouseLeftDown;
            _wasMouseRightDown = mouseRightDown;
            _wasMouseMiddleDown = mouseMiddleDown;
            _wasMouseXButton1Down = mouseXButton1Down;
            _wasMouseXButton2Down = mouseXButton2Down;

            if (_currentState != null)
            {
                _currentState.Update(time);
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime time)
        {
            Use();
            if (_currentState != null)
            {
                _currentState.Draw(spriteBatch);
            }
        }

        public void SetState(UIState state)
        {
            if (state != null)
            {
                AddToHistory(state);
            }
            if (_currentState != null)
            {
                _currentState.Deactivate();
            }
            _currentState = state;
            ResetState();
            if (state != null)
            {
                state.Activate();
                state.Recalculate();
            }
        }

        public void GoBack()
        {
            if (_history.Count >= 2)
            {
                UIState state = _history[^2];
                _history.RemoveRange(_history.Count - 2, 2);
                SetState(state);
            }
        }

        private void AddToHistory(UIState state)
        {
            _history.Add(state);
            if (_history.Count > 32)
            {
                _history.RemoveRange(0, 4);
            }
        }

        public void Recalculate()
        {
            if (_currentState != null)
            {
                _currentState.Recalculate();
            }
        }

        public CalculatedStyle GetDimensions()
        {
            return new CalculatedStyle(0f, 0f, Main.ViewPort.Width, Main.ViewPort.Height);
        }

        internal void RefreshState()
        {
            if (_currentState != null)
            {
                _currentState.Deactivate();
            }
            ResetState();
            _currentState.Activate();
            _currentState.Recalculate();
        }

        public bool IsElementUnderMouse()
        {
            return IsVisible && _lastElementHover != null && !(_lastElementHover is UIState);
        }
    }
}
