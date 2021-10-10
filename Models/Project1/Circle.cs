using GrafikaKomputerowa.Project1.Models;

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
            }
        }
    }
}
