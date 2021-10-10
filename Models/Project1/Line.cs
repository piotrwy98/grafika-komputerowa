using GrafikaKomputerowa.Project1.Models;

namespace GrafikaKomputerowa.Models.Project1
{
    public class Line : Figure
    {
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
    }
}
