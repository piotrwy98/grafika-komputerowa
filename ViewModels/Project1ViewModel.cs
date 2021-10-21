using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project1;
using GrafikaKomputerowa.Project1.Models;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project1ViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand AddFigureCommand { get; set; }
        public ICommand RemoveFigureCommand { get; set; }
        public ICommand CanvasPreviewMouseLeftButtonDownCommand { get; set; }
        public ICommand CanvasMouseLeftButtonDownCommand { get; set; }
        public ICommand CanvasMouseMoveCommand { get; set; }
        public ICommand CanvasMouseLeftButtonUpCommand { get; set; }
        public ICommand CanvasMouseRightButtonDownCommand { get; set; }
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
                IsLineChecked = true;
                CurrentLine = new Line();
                IsFigureTypeChangeEnabled = true;
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
                OnPropertyChanged("RemoveFigureButtonVisibility");
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

        public Visibility RemoveFigureButtonVisibility
        {
            get
            {
                if (_isFigureTypeChangeEnabled)
                    return Visibility.Collapsed;

                return Visibility.Visible;
            }
        }
        #endregion

        #region Variables
        private Figure _draggedFigure;
        private Point _startPoint;
        private MouseMoveMode _mouseMoveMode;
        private bool _isFirstPointDefined;
        private Circle _firstPoint;
        #endregion

        public Project1ViewModel()
        {
            AddFigureCommand = new RelayCommand(AddFigure);
            RemoveFigureCommand = new RelayCommand(RemoveFigure);
            CanvasPreviewMouseLeftButtonDownCommand = new RelayCommand(CanvasPreviewMouseLeftButtonDown);
            CanvasMouseLeftButtonDownCommand = new RelayCommand(CanvasMouseLeftButtonDown);
            CanvasMouseMoveCommand = new RelayCommand(CanvasMouseMove);
            CanvasMouseLeftButtonUpCommand = new RelayCommand(CanvasMouseLeftButtonUp);
            CanvasMouseRightButtonDownCommand = new RelayCommand(CanvasMouseRightButtonDown);

            Figures = new ObservableCollection<Figure>();
            _firstPoint = new Circle
            {
                Radius = 10,
                StrokeColor = Colors.LightSkyBlue,
                FillColor = Colors.LightSkyBlue,
            };
        }

        public async void NewFile()
        {
            var metroDialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Tak",
                NegativeButtonText = "Nie",
                AnimateHide = false
            };

            var dialogCoordinator = (Application.Current.MainWindow.DataContext as MainViewModel).DialogCoordinator;
            var result = await dialogCoordinator.ShowMessageAsync(this, "Uwaga", "Niezapisane zmiany zostaną utracone. Kontynuować?", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
            if (result != MessageDialogResult.Affirmative)
                return;

            Figures = new ObservableCollection<Figure>();
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

        private async void RemoveFigure(object obj = null)
        {
            var metroDialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Tak",
                NegativeButtonText = "Nie",
                AnimateHide = false
            };

            var dialogCoordinator = (Application.Current.MainWindow.DataContext as MainViewModel).DialogCoordinator;
            var result = await dialogCoordinator.ShowMessageAsync(this, "Uwaga", "Czy na pewno chcesz usunąć tę figurę?", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
            if (result != MessageDialogResult.Affirmative)
                return;

            Figures.Remove(CurrentFigure);
            IsLineChecked = true;
            CurrentLine = new Line();
            IsFigureTypeChangeEnabled = true;
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

        private void CanvasMouseRightButtonDown(object obj)
        {
            Canvas canvas = obj as Canvas;
            if (canvas != null)
            {
                Point currentPoint = Mouse.GetPosition(canvas);
                currentPoint.X = Math.Floor(currentPoint.X);
                currentPoint.Y = Math.Floor(currentPoint.Y);

                if (!_isFirstPointDefined)
                {
                    _firstPoint.CenterPointX = currentPoint.X;
                    _firstPoint.CenterPointY = currentPoint.Y;
                    Figures.Add(_firstPoint);
                    _isFirstPointDefined = true;
                }
                else
                {
                    if(IsLineChecked)
                    {
                        Line line = new Line()
                        {
                            X1 = _firstPoint.X,
                            Y1 = _firstPoint.Y,
                            X2 = currentPoint.X,
                            Y2 = currentPoint.Y,
                            StrokeThickness = CurrentLine.StrokeThickness,
                            StrokeColor = CurrentLine.StrokeColor
                        };

                        Figures.Add(line);
                        CurrentLine = line;
                    }
                    else if(IsRectangleChecked)
                    {
                        double width = currentPoint.X - _firstPoint.X;
                        double x = _firstPoint.X;
                        if (width < 0)
                            x = currentPoint.X;

                        double height = currentPoint.Y - _firstPoint.Y;
                        double y = _firstPoint.Y;
                        if (height < 0)
                            y = currentPoint.Y;

                        Rectangle rectangle = new Rectangle()
                        {
                            X = x,
                            Y = y,
                            Width = Math.Abs(width),
                            Height = Math.Abs(height),
                            StrokeThickness = CurrentLine.StrokeThickness,
                            StrokeColor = CurrentLine.StrokeColor,
                            FillColor = CurrentLine.FillColor
                        };

                        Figures.Add(rectangle);
                        CurrentRectangle = rectangle;
                    }
                    else // IsCircleChecked
                    {
                        Circle circle = new Circle()
                        {
                            CenterPointX = _firstPoint.X,
                            CenterPointY = _firstPoint.Y,
                            Radius = Math.Floor(GetDistanceBetweenPoints(currentPoint)),
                            StrokeThickness = CurrentLine.StrokeThickness,
                            StrokeColor = CurrentLine.StrokeColor,
                            FillColor = CurrentLine.FillColor
                        };

                        Figures.Add(circle);
                        CurrentCircle = circle;
                    }

                    Figures.Remove(_firstPoint);
                    _isFirstPointDefined = false;
                    IsFigureTypeChangeEnabled = false;
                }
            }
        }

        private double GetDistanceBetweenPoints(Point currentPoint)
        {
            double distanceX = currentPoint.X - _firstPoint.X;
            double distanceY = currentPoint.Y - _firstPoint.Y;
            return Math.Sqrt(distanceX * distanceX + distanceY * distanceY);
        }

        public void KeyDown(KeyEventArgs args)
        {
            switch(args.Key)
            {
                case Key.Delete:
                    if (CurrentFigure != null && Figures.Contains(CurrentFigure))
                        RemoveFigure();
                    break;
            }
    }
    }
}
