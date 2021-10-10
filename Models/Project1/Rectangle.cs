using GrafikaKomputerowa.Project1.Models;

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
    }
}
