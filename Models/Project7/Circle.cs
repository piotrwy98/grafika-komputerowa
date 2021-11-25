namespace GrafikaKomputerowa.Models.Project7
{
    public class Circle : CanvasElement
    {
        public double Diameter { get; set; } = 15;

        private double _centerPointX;
        public double CenterPointX
        {
            get
            {
                return _centerPointX;
            }
            set
            {
                _centerPointX = value;
                X = value - (Diameter / 2);
                OnPropertyChanged();
            }
        }

        private double _centerPointY;
        public double CenterPointY
        {
            get
            {
                return _centerPointY;
            }
            set
            {
                _centerPointY = value;
                Y = value - (Diameter / 2);
                OnPropertyChanged();
            }
        }
    }
}
