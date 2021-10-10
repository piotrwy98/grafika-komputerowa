using GrafikaKomputerowa.Project1.Models;
using System;
using System.Windows;

namespace GrafikaKomputerowa.Models.Project1
{
    public class Circle : Figure
    {
        private double _radius = 50;
        public double Radius
        {
            get
            {
                return _radius;
            }
            set
            {
                _radius = value;
                OnPropertyChanged();

                X = _centerPointX - value;
                Y = _centerPointY - value;
            }
        }

        private double _centerPointX = 50;
        public double CenterPointX
        {
            get
            {
                return _centerPointX;
            }
            set
            {
                _centerPointX = value;
                X = value - Radius;
                OnPropertyChanged();
            }
        }

        private double _centerPointY = 50;
        public double CenterPointY
        {
            get
            {
                return _centerPointY;
            }
            set
            {
                _centerPointY = value;
                Y = value - Radius;
                OnPropertyChanged();
            }
        }

        public override MouseMoveMode GetMouseMoveMode(Point startPoint)
        {
            double topX = _centerPointX;
            double topY = _centerPointY + _radius;
            double bottomX = _centerPointX;
            double bottomY = _centerPointY - _radius;
            double rightX = _centerPointX + _radius;
            double rightY = _centerPointY;
            double leftX = _centerPointX - _radius;
            double leftY = _centerPointY;

            if ((Math.Abs(startPoint.X - topX) <= X_TOLLERACNE &&
                Math.Abs(startPoint.Y - topY) <= Y_TOLLERACNE) ||
                (Math.Abs(startPoint.X - bottomX) <= X_TOLLERACNE &&
                Math.Abs(startPoint.Y - bottomY) <= Y_TOLLERACNE))
                return MouseMoveMode.RESIZE_DOWN;

            if ((Math.Abs(startPoint.X - rightX) <= X_TOLLERACNE &&
                Math.Abs(startPoint.Y - rightY) <= Y_TOLLERACNE) ||
                (Math.Abs(startPoint.X - leftX) <= X_TOLLERACNE &&
                Math.Abs(startPoint.Y - leftY) <= Y_TOLLERACNE))
                return MouseMoveMode.RESIZE_LEFT;

            return MouseMoveMode.DRAG;
        }

        public override void MouseMove(Point startPoint, Point currentPoint, MouseMoveMode mouseMoveMode)
        {
            double xDiff = currentPoint.X - startPoint.X;
            double yDiff = currentPoint.Y - startPoint.Y;

            switch (mouseMoveMode)
            {
                case MouseMoveMode.RESIZE_DOWN:
                    if(Radius + yDiff >= StrokeThickness)
                        Radius += yDiff;
                    break;

                case MouseMoveMode.RESIZE_LEFT:
                    if (Radius + xDiff >= StrokeThickness)
                        Radius += xDiff;
                    break;

                case MouseMoveMode.DRAG:
                    CenterPointX += xDiff;
                    CenterPointY += yDiff;
                    break;
            }
        }
    }
}
