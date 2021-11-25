using Newtonsoft.Json;
using System.Windows.Media;

namespace GrafikaKomputerowa.Models.Project7
{
    public class Polygon : CanvasElement
    {
        private PointCollection _points;
        public PointCollection Points
        {
            get
            {
                return _points;
            }
            set
            {
                _points = value;
                OnPropertyChanged();
            }
        }

        private Color _fillColor;
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
                OnPropertyChanged("FillBrush");
            }
        }

        [JsonIgnore]
        public SolidColorBrush FillBrush
        {
            get
            {
                return new SolidColorBrush(_fillColor);
            }
        }

        public Polygon()
        {
            Points = new PointCollection();
            FillColor = Colors.IndianRed;
        }
    }
}
