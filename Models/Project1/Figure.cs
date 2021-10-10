using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project1;
using System.Windows;
using System.Windows.Media;

namespace GrafikaKomputerowa.Project1.Models
{
    public class Figure : NotifyPropertyChanged
    {
        #region Constants
        public static readonly double X_TOLLERACNE = 10;
        public static readonly double Y_TOLLERACNE = 10;
        #endregion

        #region Properties
        private double _x;
        public double X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                OnPropertyChanged();
            }
        }

        private double _y;
        public double Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnPropertyChanged();
            }
        }

        private Color _fillColor = Colors.Transparent;
        public Color FillColor
        {
            get
            {
                return _fillColor;
            }
            set
            {
                _fillColor = value;
                OnPropertyChanged();
                OnPropertyChanged("FillColorBrush");
            }
        }

        public SolidColorBrush FillColorBrush
        {
            get
            {
                return new SolidColorBrush(_fillColor);
            }
        }

        private Color _strokeColor = Colors.Black;
        public Color StrokeColor
        {
            get
            {
                return _strokeColor;
            }
            set
            {
                _strokeColor = value;
                OnPropertyChanged();
                OnPropertyChanged("StrokeColorBrush");
            }
        }

        public SolidColorBrush StrokeColorBrush
        {
            get
            {
                return new SolidColorBrush(_strokeColor);
            }
        }

        private double _strokeThickness = 2;
        public double StrokeThickness
        {
            get
            {
                return _strokeThickness;
            }
            set
            {
                _strokeThickness = value;
                OnPropertyChanged();
            }
        }
        #endregion

        public virtual MouseMoveMode GetMouseMoveMode(Point startPoint)
        {
            return MouseMoveMode.DRAG;
        }

        public virtual void MouseMove(Point startPoint, Point currentPoint, MouseMoveMode mouseMoveMode)
        {

        }
    }
}
