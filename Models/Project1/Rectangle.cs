using GrafikaKomputerowa.Project1.Models;
using System;
using System.Windows;

namespace GrafikaKomputerowa.Models.Project1
{
    public class Rectangle : Figure
    {
        private double _height = 100;
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }

        private double _width = 100;
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }

        public override MouseMoveMode GetMouseMoveMode(Point startPoint)
        {
            if(Math.Abs(startPoint.Y - Y) <= Y_TOLLERACNE)
            {
                if (Math.Abs(startPoint.X - X) <= X_TOLLERACNE)
                    return MouseMoveMode.RESIZE_UP_LEFT;

                if(Math.Abs(startPoint.X - Width - X) <= X_TOLLERACNE)
                    return MouseMoveMode.RESIZE_UP_RIGHT;

                return MouseMoveMode.RESIZE_UP;
            }

            if (Math.Abs(startPoint.Y - Height - Y) <= Y_TOLLERACNE)
            {
                if (Math.Abs(startPoint.X - X) <= X_TOLLERACNE)
                    return MouseMoveMode.RESIZE_DOWN_LEFT;

                if (Math.Abs(startPoint.X - Width - X) <= X_TOLLERACNE)
                    return MouseMoveMode.RESIZE_DOWN_RIGHT;

                return MouseMoveMode.RESIZE_DOWN;
            }

            if (Math.Abs(startPoint.X - X) <= X_TOLLERACNE)
                return MouseMoveMode.RESIZE_LEFT;

            if (Math.Abs(startPoint.X - Width - X) <= X_TOLLERACNE)
                return MouseMoveMode.RESIZE_RIGHT;

            return MouseMoveMode.DRAG;
        }

        public override void MouseMove(Point startPoint, Point currentPoint, MouseMoveMode mouseMoveMode)
        {
            double xDiff = currentPoint.X - startPoint.X;
            double yDiff = currentPoint.Y - startPoint.Y;

            switch (mouseMoveMode)
            {
                case MouseMoveMode.RESIZE_UP:
                    if (Height - yDiff > StrokeThickness)
                    {
                        Y += yDiff;
                        Height -= yDiff;
                    }
                    break;

                case MouseMoveMode.RESIZE_UP_LEFT:
                    if (Height - yDiff > StrokeThickness &&
                       Width - xDiff > StrokeThickness)
                    {
                        Y += yDiff;
                        Height -= yDiff;
                        X += xDiff;
                        Width -= xDiff;
                    }
                    break;

                case MouseMoveMode.RESIZE_UP_RIGHT:
                    if (Height - yDiff > StrokeThickness)
                    {
                        Y += yDiff;
                        Height -= yDiff;
                        Width += xDiff;
                    }
                    break;

                case MouseMoveMode.RESIZE_DOWN:
                    Height += yDiff;
                    break;

                case MouseMoveMode.RESIZE_DOWN_LEFT:
                    if (Width - xDiff > StrokeThickness)
                    {
                        Height += yDiff;
                        X += xDiff;
                        Width -= xDiff;
                    }
                    break;

                case MouseMoveMode.RESIZE_DOWN_RIGHT:
                    Height += yDiff;
                    Width += xDiff;
                    break;

                case MouseMoveMode.RESIZE_LEFT:
                    if (Width - xDiff > StrokeThickness)
                    {
                        X += xDiff;
                        Width -= xDiff;
                    }
                    break;

                case MouseMoveMode.RESIZE_RIGHT:
                    Width += xDiff;
                    break;

                case MouseMoveMode.DRAG:
                    X += xDiff;
                    Y += yDiff;
                    break;
            }
        }
    }
}
