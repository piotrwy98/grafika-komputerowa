using GrafikaKomputerowa.Project1.Models;
using System;
using System.Windows;

namespace GrafikaKomputerowa.Models.Project1
{
    public class Line : Figure
    {
        private double _x1;
        public double X1
        {
            get
            {
                return _x1;
            }
            set
            {
                _x1 = value;
                OnPropertyChanged();
            }
        }

        private double _y1;
        public double Y1
        {
            get
            {
                return _y1;
            }
            set
            {
                _y1 = value;
                OnPropertyChanged();
            }
        }

        private double _x2 = 100;
        public double X2
        {
            get
            {
                return _x2;
            }
            set
            {
                _x2 = value;
                OnPropertyChanged();
            }
        }

        private double _y2 = 100;
        public double Y2
        {
            get
            {
                return _y2;
            }
            set
            {
                _y2 = value;
                OnPropertyChanged();
            }
        }

        public override MouseMoveMode GetMouseMoveMode(Point startPoint)
        {
            if (Math.Abs(startPoint.Y - Y1) <= Y_TOLLERACNE &&
                Math.Abs(startPoint.X - X1) <= X_TOLLERACNE)
                return MouseMoveMode.RESIZE_DOWN_LEFT;

            if (Math.Abs(startPoint.Y - Y2) <= Y_TOLLERACNE &&
                Math.Abs(startPoint.X - X2) <= X_TOLLERACNE)
                return MouseMoveMode.RESIZE_DOWN_RIGHT;

            return MouseMoveMode.DRAG;
        }

        public override void MouseMove(Point startPoint, Point currentPoint, MouseMoveMode mouseMoveMode)
        {
            double xDiff = currentPoint.X - startPoint.X;
            double yDiff = currentPoint.Y - startPoint.Y;

            switch (mouseMoveMode)
            {
                case MouseMoveMode.RESIZE_DOWN_LEFT:
                    X1 += xDiff;
                    Y1 += yDiff;
                    break;

                case MouseMoveMode.RESIZE_DOWN_RIGHT:
                    X2 += xDiff;
                    Y2 += yDiff;
                    break;

                case MouseMoveMode.DRAG:
                    X1 += xDiff;
                    Y1 += yDiff;
                    X2 += xDiff;
                    Y2 += yDiff;
                    break;
            }
        }
    }
}
