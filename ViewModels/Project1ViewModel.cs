using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project1;
using GrafikaKomputerowa.Project1.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project1ViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand AddFigureCommand { get; set; }
        public ICommand CanvasMouseLeftButtonDownCommand { get; set; }
        #endregion

        #region Properties
        public ObservableCollection<Figure> Figures { get; set; }

        private Figure _currentFigure;
        public Figure CurrentFigure
        {
            get
            {
                return _currentFigure;
            }
            set
            {
                _currentFigure = value;
                OnPropertyChanged();
            }
        }

        private Line _currentLine;
        public Line CurrentLine
        {
            get
            {
                return _currentLine;
            }
            set
            {
                _currentLine = value;
                CurrentFigure = value;
                OnPropertyChanged();
            }
        }

        private Rectangle _currentRectangle;
        public Rectangle CurrentRectangle
        {
            get
            {
                return _currentRectangle;
            }
            set
            {
                _currentRectangle = value;
                CurrentFigure = value;
                OnPropertyChanged();
            }
        }

        private Circle _currentCircle;
        public Circle CurrentCircle
        {
            get
            {
                return _currentCircle;
            }
            set
            {
                _currentCircle = value;
                CurrentFigure = value;
                OnPropertyChanged();
            }
        }

        private bool _isLineChecked;
        public bool IsLineChecked
        {
            get
            {
                return _isLineChecked;
            }
            set
            {
                if (!_isRectangleChecked && !_isCircleChecked)
                    return;

                _isLineChecked = value;
                OnPropertyChanged();
                OnPropertyChanged("LineParametersVisibility");

                if (value)
                {
                    IsRectangleChecked = false;
                    IsCircleChecked = false;
                }

                CurrentLine = new Line();
            }
        }

        private bool _isRectangleChecked = true;
        public bool IsRectangleChecked
        {
            get
            {
                return _isRectangleChecked;
            }
            set
            {
                if (!_isLineChecked && !_isCircleChecked)
                    return;

                _isRectangleChecked = value;
                OnPropertyChanged();
                OnPropertyChanged("FillColorVisibility");
                OnPropertyChanged("RectangleParametersVisibility");

                if(value)
                {
                    IsLineChecked = false;
                    IsCircleChecked = false;
                }

                CurrentRectangle = new Rectangle();
            }
        }

        private bool _isCircleChecked;
        public bool IsCircleChecked
        {
            get
            {
                return _isCircleChecked;
            }
            set
            {
                if (!_isLineChecked && !_isRectangleChecked)
                    return;

                _isCircleChecked = value;
                OnPropertyChanged();
                OnPropertyChanged("FillColorVisibility");
                OnPropertyChanged("CircleParametersVisibility");

                if (value)
                {
                    IsLineChecked = false;
                    IsRectangleChecked = false;
                }

                CurrentCircle = new Circle();
            }
        }

        public Visibility FillColorVisibility
        {
            get
            {
                if (_isRectangleChecked || _isCircleChecked)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
        }

        public Visibility LineParametersVisibility
        {
            get
            {
                if (_isLineChecked)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
        }

        public Visibility RectangleParametersVisibility
        {
            get
            {
                if (_isRectangleChecked)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
        }

        public Visibility CircleParametersVisibility
        {
            get
            {
                if (_isCircleChecked)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
        }
        #endregion

        public Project1ViewModel()
        {
            AddFigureCommand = new RelayCommand(AddFigure);
            CanvasMouseLeftButtonDownCommand = new RelayCommand(CanvasMouseLeftButtonDown);

            Figures = new ObservableCollection<Figure>();
            IsLineChecked = true;
        }

        private void AddFigure(object obj)
        {
            if (_isLineChecked)
            {
                Figures.Add(CurrentLine);
                CurrentLine = new Line();
            }
            else if (_isRectangleChecked)
            {
                Figures.Add(CurrentRectangle);
                CurrentRectangle = new Rectangle();
            }
            else
            {
                Figures.Add(CurrentCircle);
                CurrentCircle = new Circle();
            }
        }

        private void CanvasMouseLeftButtonDown(object obj)
        {
            var args = obj as MouseButtonEventArgs;
            if (args == null)
                return;

            if(args.OriginalSource is Line)
            {

            }
            else if (args.OriginalSource is Rectangle)
            {

            }
            else if (args.OriginalSource is Circle)
            {

            }
            else
            {
                
            }
        }
    }
}
