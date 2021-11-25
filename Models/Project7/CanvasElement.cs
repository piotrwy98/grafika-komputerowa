using Newtonsoft.Json;

namespace GrafikaKomputerowa.Models.Project7
{
    public abstract class CanvasElement : NotifyPropertyChanged
    {
        private double _x;
        [JsonIgnore]
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
        [JsonIgnore]
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

        private bool _isSelected;
        [JsonIgnore]
        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }
    }
}
