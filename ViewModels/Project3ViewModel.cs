using GrafikaKomputerowa.Models;
using System;
using System.Windows.Media;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project3ViewModel : NotifyPropertyChanged
    {
        #region Properties
        private Color _selectedColor;
        public Color SelectedColor
        {
            get
            {
                return _selectedColor;
            }
            set
            {
                _selectedColor = value;
                OnPropertyChanged();
                OnPropertyChanged("SelectedColorBrush");

                R = _selectedColor.R;
                G = _selectedColor.G;
                B = _selectedColor.B;

                RefreshCmyk();
            }
        }

        public SolidColorBrush SelectedColorBrush
        {
            get
            {
                return new SolidColorBrush(_selectedColor);
            }
        }

        private byte _r;
        public byte R
        {
            get
            {
                return _r;
            }
            set
            {
                _r = value;
                OnPropertyChanged();

                _selectedColor.R = _r;
                _selectedColor.A = 0xFF;
                OnPropertyChanged("SelectedColor");
                OnPropertyChanged("SelectedColorBrush");

                RefreshCmyk();
            }
        }

        private byte _g;
        public byte G
        {
            get
            {
                return _g;
            }
            set
            {
                _g = value;
                OnPropertyChanged();

                _selectedColor.G = _g;
                _selectedColor.A = 0xFF;
                OnPropertyChanged("SelectedColor");
                OnPropertyChanged("SelectedColorBrush");

                RefreshCmyk();
            }
        }

        private byte _b;
        public byte B
        {
            get
            {
                return _b;
            }
            set
            {
                _b = value;
                OnPropertyChanged();

                _selectedColor.B = _b;
                _selectedColor.A = 0xFF;
                OnPropertyChanged("SelectedColor");
                OnPropertyChanged("SelectedColorBrush");

                RefreshCmyk();
            }
        }

        private float _c;
        public float C
        {
            get
            {
                return _c;
            }
            set
            {
                _c = value;
                OnPropertyChanged();

                RefreshRgb();
            }
        }

        private float _m;
        public float M
        {
            get
            {
                return _m;
            }
            set
            {
                _m = value;
                OnPropertyChanged();

                RefreshRgb();
            }
        }

        private float _y;
        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                OnPropertyChanged();

                RefreshRgb();
            }
        }

        private float _k;
        public float K
        {
            get
            {
                return _k;
            }
            set
            {
                _k = value;
                OnPropertyChanged();

                RefreshRgb();
            }
        }
        #endregion

        public Project3ViewModel()
        {
            SelectedColor = Color.FromRgb(0, 0, 0);
        }

        public void RefreshCmyk()
        {
            float newR = (float) R / 255;
            float newG = (float) G / 255;
            float newB = (float) B / 255;

            _k = Helper(1 - Math.Max(Math.Max(newR, newG), newB));
            _c = Helper((1 - newR - K) / (1 - K));
            _m = Helper((1 - newG - K) / (1 - K));
            _y = Helper((1 - newB - K) / (1 - K));

            OnPropertyChanged("K");
            OnPropertyChanged("C");
            OnPropertyChanged("M");
            OnPropertyChanged("Y");
        }

        public void RefreshRgb()
        {
            _r = (byte) (255 * (1 - C) * (1 - K));
            _g = (byte) (255 * (1 - M) * (1 - K));
            _b = (byte) (255 * (1 - Y) * (1 - K));

            OnPropertyChanged("R");
            OnPropertyChanged("G");
            OnPropertyChanged("B");

            _selectedColor = Color.FromRgb(_r, _g, _b);

            OnPropertyChanged("SelectedColor");
            OnPropertyChanged("SelectedColorBrush");
        }

        public float Helper(float value)
        {
            if (value < 0 || float.IsNaN(value))
            {
                return 0;
            }

            return value;
        }
    }
}
