using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project1;
using GrafikaKomputerowa.Project1.Models;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project1ViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand AddFigureCommand { get; set; }
        public ICommand CanvasPreviewMouseLeftButtonDownCommand { get; set; }
        public ICommand CanvasMouseLeftButtonDownCommand { get; set; }
        public ICommand CanvasMouseMoveCommand { get; set; }
        public ICommand CanvasMouseLeftButtonUpCommand { get; set; }
        #endregion

        #region Properties
        private ObservableCollection<Figure> _figures;
        public ObservableCollection<Figure> Figures
        {
            get
            {
                return _figures;
            }
            set
            {
                _figures = value;
                OnPropertyChanged();
            }
        }

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

        private bool _isFigureTypeChangeEnabled = true;
        public bool IsFigureTypeChangeEnabled
        {
            get
            {
                return _isFigureTypeChangeEnabled;
            }
            set
            {
                _isFigureTypeChangeEnabled = value;
                OnPropertyChanged();
                OnPropertyChanged("AddFigureButtonVisibility");
            }
        }

        public Visibility AddFigureButtonVisibility
        {
            get
            {
                if (_isFigureTypeChangeEnabled)
                    return Visibility.Visible;

                return Visibility.Collapsed;
            }
        }
        #endregion

        #region Variables
        private Figure _draggedFigure;
        private Point _startPoint;
        private MouseMoveMode _mouseMoveMode;
        #endregion

        public Project1ViewModel()
        {
            AddFigureCommand = new RelayCommand(AddFigure);
            CanvasPreviewMouseLeftButtonDownCommand = new RelayCommand(CanvasPreviewMouseLeftButtonDown);
            CanvasMouseLeftButtonDownCommand = new RelayCommand(CanvasMouseLeftButtonDown);
            CanvasMouseMoveCommand = new RelayCommand(CanvasMouseMove);
            CanvasMouseLeftButtonUpCommand = new RelayCommand(CanvasMouseLeftButtonUp);

            Figures = new ObservableCollection<Figure>();
            IsLineChecked = true;
        }

        public async void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik do odczytu";
            openFileDialog.Filter = "JSON|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                };

                try
                {
                    await Task.Run(() =>
                    {
                        string json = File.ReadAllText(openFileDialog.FileName);
                        Figures = JsonConvert.DeserializeObject<ObservableCollection<Figure>>(json, jsonSerializerSettings);
                    });
                }
                catch
                {
                    MessageBox.Show("Błąd wczytywania pliku JSON", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public async void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Wybierz plik do zapisu";
            saveFileDialog.Filter = "JSON|*.json";
            saveFileDialog.FileName = "figures";

            if (saveFileDialog.ShowDialog() == true)
            {
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    Formatting = Newtonsoft.Json.Formatting.Indented
                };

                try
                {
                    await Task.Run(() =>
                    {
                        string json = JsonConvert.SerializeObject(Figures, jsonSerializerSettings);
                        File.WriteAllText(saveFileDialog.FileName, json);
                    });
                }
                catch
                {
                    MessageBox.Show("Błąd zapisu pliku JSON", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddFigure(object obj)
        {
            if (_isLineChecked)
                Figures.Add(CurrentLine);
            else if (_isRectangleChecked)
                Figures.Add(CurrentRectangle);
            else
                Figures.Add(CurrentCircle);

            IsFigureTypeChangeEnabled = false;
        }

        private void CanvasPreviewMouseLeftButtonDown(object obj)
        {
            Canvas canvas = obj as Canvas;
            if (canvas != null)
                _startPoint = Mouse.GetPosition(canvas);
        }

        private void CanvasMouseLeftButtonDown(object obj)
        {
            var args = obj as MouseButtonEventArgs;
            if (args == null)
                return;

            if(args.OriginalSource is System.Windows.Shapes.Shape)
            {
                IsFigureTypeChangeEnabled = false;

                // ustalenie typu figury
                if (args.OriginalSource is System.Windows.Shapes.Line)
                {
                    IsLineChecked = true;
                    CurrentLine = (args.OriginalSource as FrameworkElement).DataContext as Line;
                    _draggedFigure = CurrentLine;
                }
                else if (args.OriginalSource is System.Windows.Shapes.Rectangle)
                {
                    IsRectangleChecked = true;
                    CurrentRectangle = (args.OriginalSource as FrameworkElement).DataContext as Rectangle;
                    _draggedFigure = CurrentRectangle;
                }
                else // System.Windows.Shapes.Ellipse
                {
                    IsCircleChecked = true;
                    CurrentCircle = (args.OriginalSource as FrameworkElement).DataContext as Circle;
                    _draggedFigure = CurrentCircle;
                }

                // ustalenie trybu ruchu myszy
                _mouseMoveMode = _draggedFigure.GetMouseMoveMode(_startPoint);

                switch(_mouseMoveMode)
                {
                    case MouseMoveMode.DRAG:
                        Mouse.OverrideCursor = Cursors.SizeAll;
                        break;

                    case MouseMoveMode.RESIZE_UP:
                    case MouseMoveMode.RESIZE_DOWN:
                        Mouse.OverrideCursor = Cursors.SizeNS;
                        break;

                    case MouseMoveMode.RESIZE_LEFT:
                    case MouseMoveMode.RESIZE_RIGHT:
                        Mouse.OverrideCursor = Cursors.SizeWE;
                        break;

                    case MouseMoveMode.RESIZE_UP_LEFT:
                    case MouseMoveMode.RESIZE_DOWN_RIGHT:
                        Mouse.OverrideCursor = Cursors.SizeNWSE;
                        break;

                    case MouseMoveMode.RESIZE_UP_RIGHT:
                    case MouseMoveMode.RESIZE_DOWN_LEFT:
                        Mouse.OverrideCursor = Cursors.SizeNESW;
                        break;
                }
            }
            else
            {
                if(!IsFigureTypeChangeEnabled)
                {
                    IsLineChecked = true;
                    IsFigureTypeChangeEnabled = true;
                    _draggedFigure = null;
                }
            }
        }

        private void CanvasMouseMove(object obj)
        {
            if(_draggedFigure != null)
            {
                Canvas canvas = obj as Canvas;
                if (canvas != null)
                {
                    Point currentPoint = Mouse.GetPosition(canvas);
                    _draggedFigure.MouseMove(_startPoint, currentPoint, _mouseMoveMode);
                    _startPoint = currentPoint;
                }
            }
        }

        private void CanvasMouseLeftButtonUp(object obj)
        {
            _draggedFigure = null;
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
