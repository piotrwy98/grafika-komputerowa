using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project7;
using GrafikaKomputerowa.Views;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project7ViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand AddPolygonCommand { get; }
        public ICommand EditPolygonCommand { get; }
        public ICommand RemovePolygonCommand { get; }

        public ICommand CanvasMouseRightButtonDownCommand { get; }
        public ICommand CanvasMouseLeftButtonDownCommand { get; }
        public ICommand CanvasMouseMoveCommand { get; }
        public ICommand CanvasMouseLeftButtonUpCommand { get; }
        public ICommand CanvasMouseWheelCommand { get; }

        public ICommand TranslateCommand { get; }
        public ICommand RotateCommand { get; }
        public ICommand ScaleCommand { get; }
        #endregion

        #region Properties
        private ObservableCollection<CanvasElement> _canvasElements;
        public ObservableCollection<CanvasElement> CanvasElements
        {
            get
            {
                return _canvasElements;
            }
            set
            {
                _canvasElements = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Polygon> _polygons;
        public ObservableCollection<Polygon> Polygons
        {
            get
            {
                return _polygons;
            }
            set
            {
                _polygons = value;
                OnPropertyChanged();
            }
        }

        private Polygon _selectedPolygon;
        public Polygon SelectedPolygon
        {
            get
            {
                return _selectedPolygon;
            }
            set
            {
                _selectedPolygon = value;
                OnPropertyChanged();
                OnPropertyChanged("IsPolygonSelected");
            }
        }

        public bool IsPolygonSelected
        {
            get
            {
                return _selectedPolygon != null;
            }
        }

        public double TranslationX { get; set; }
        public double TranslationY { get; set; }

        private double _rotationX;
        public double RotationX
        {
            get
            {
                return _rotationX;
            }
            set
            {
                _rotationX = value;
                OnPropertyChanged();
            }
        }

        private double _rotationY;
        public double RotationY
        {
            get
            {
                return _rotationY;
            }
            set
            {
                _rotationY = value;
                OnPropertyChanged();
            }
        }

        public double RotationAngle { get; set; }

        private double _scalingX;
        public double ScalingX
        {
            get
            {
                return _scalingX;
            }
            set
            {
                _scalingX = value;
                OnPropertyChanged();
            }
        }

        private double _scalingY;
        public double ScalingY
        {
            get
            {
                return _scalingY;
            }
            set
            {
                _scalingY = value;
                OnPropertyChanged();
            }
        }

        public double ScalingK { get; set; }
        #endregion

        #region Variables
        private List<Circle> _circles;
        private CanvasElement _draggedCanvasElement;
        private Point _startPoint;
        private bool _isDraggingCorner;
        #endregion

        public Project7ViewModel()
        {
            AddPolygonCommand = new RelayCommand(AddPolygon);
            EditPolygonCommand = new RelayCommand(EditPolygon);
            RemovePolygonCommand = new RelayCommand(RemovePolygon);

            CanvasMouseRightButtonDownCommand = new RelayCommand(CanvasMouseRightButtonDown);
            CanvasMouseLeftButtonDownCommand = new RelayCommand(CanvasMouseLeftButtonDown);
            CanvasMouseMoveCommand = new RelayCommand(CanvasMouseMove);
            CanvasMouseLeftButtonUpCommand = new RelayCommand(CanvasMouseLeftButtonUp);
            CanvasMouseWheelCommand = new RelayCommand(CanvasMouseWheel);

            TranslateCommand = new RelayCommand(Translate);
            RotateCommand = new RelayCommand(Rotate);
            ScaleCommand = new RelayCommand(Scale);

            Polygons = new ObservableCollection<Polygon>();
            CanvasElements = new ObservableCollection<CanvasElement>();
            _circles = new List<Circle>();
        }

        public async void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Wybierz plik do zapisu";
            saveFileDialog.Filter = "JSON|*.json";
            saveFileDialog.FileName = "polygons";

            if (saveFileDialog.ShowDialog() == true)
            {
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    Formatting = Formatting.Indented
                };

                try
                {
                    string json = JsonConvert.SerializeObject(Polygons, jsonSerializerSettings);
                    File.WriteAllText(saveFileDialog.FileName, json);
                }
                catch
                {
                    var dialogCoordinator = (Application.Current.MainWindow.DataContext as MainViewModel).DialogCoordinator;
                    var result = await dialogCoordinator.ShowMessageAsync(this, "Błąd", "Nie udało się zapisać do pliku JSON");
                }
            }
        }

        public async void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik do odczytu";
            openFileDialog.Filter = "JSON|*.json";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string json = File.ReadAllText(openFileDialog.FileName);
                    Polygons = JsonConvert.DeserializeObject<ObservableCollection<Polygon>>(json);

                    _circles.Clear();
                    CanvasElements.Clear();

                    foreach(var polygon in Polygons)
                    {
                        CanvasElements.Add(polygon);
                    }
                }
                catch
                {
                    var dialogCoordinator = (Application.Current.MainWindow.DataContext as MainViewModel).DialogCoordinator;
                    var result = await dialogCoordinator.ShowMessageAsync(this, "Błąd", "Nie udało się otworzyć pliku JSON");
                }
            }
        }

        private void AddPolygon(object obj)
        {
            var addPolygonWindow = new AddEditPolygonWindow();

            if(addPolygonWindow.ShowDialog() == true)
            {
                var polygon = (addPolygonWindow.DataContext as AddEditPolygonViewModel).Polygon;
                SetPolygon(polygon);
            }
        }

        private void SetPolygon(Polygon polygon)
        {
            SelectedPolygon = polygon;
            polygon.IsSelected = true;
            Polygons.Add(polygon);
            CanvasElements.Add(polygon);
        }

        private void EditPolygon(object obj)
        {
            var polygon = obj as Polygon;
            if (polygon == null)
            {
                return;
            }

            var addPolygonWindow = new AddEditPolygonWindow(polygon);
            addPolygonWindow.ShowDialog();
        }

        private async void RemovePolygon(object obj)
        {
            var polygon = obj as Polygon;
            if(polygon == null)
            {
                return;
            }

            var metroDialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Tak",
                NegativeButtonText = "Nie",
                AnimateHide = false
            };

            var dialogCoordinator = (Application.Current.MainWindow.DataContext as MainViewModel).DialogCoordinator;
            var result = await dialogCoordinator.ShowMessageAsync(this, "Uwaga", "Czy na pewno chcesz usunąć ten wielokąt?", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
            if (result == MessageDialogResult.Affirmative)
            {
                SelectedPolygon = null;
                Polygons.Remove(polygon);
                CanvasElements.Remove(polygon);
            }
        }

        private void CanvasMouseRightButtonDown(object obj)
        {
            Canvas canvas = obj as Canvas;
            if (canvas != null)
            {
                var currentPoint = Mouse.GetPosition(canvas);
                currentPoint.X = Math.Floor(currentPoint.X);
                currentPoint.Y = Math.Floor(currentPoint.Y);

                var circle = new Circle() { CenterPointX = currentPoint.X, CenterPointY = currentPoint.Y };
                _circles.Add(circle);
                CanvasElements.Add(circle);

                RotationX = currentPoint.X;
                RotationY = currentPoint.Y;
                ScalingX = currentPoint.X;
                ScalingY = currentPoint.Y;
            }
        }

        private void CanvasMouseLeftButtonDown(object obj)
        {
            var args = obj as MouseButtonEventArgs;
            if (args != null)
            {
                if (args.OriginalSource is System.Windows.Shapes.Ellipse)
                {
                    _draggedCanvasElement = (args.OriginalSource as FrameworkElement).DataContext as Circle;
                }
                else if (args.OriginalSource is System.Windows.Shapes.Polygon)
                {
                    var polygon = (args.OriginalSource as FrameworkElement).DataContext as Polygon;
                    _draggedCanvasElement = polygon;

                    polygon.IsSelected = true;
                    SelectedPolygon = polygon;

                    _isDraggingCorner = false;

                    foreach (var point in polygon.Points)
                    {
                        if (Math.Abs(_startPoint.Y - point.Y) <= 20 &&
                           Math.Abs(_startPoint.X - point.X) <= 20)
                        {
                            _isDraggingCorner = true;
                            break;
                        }
                    }
                }
                else
                {
                    _draggedCanvasElement = null;

                    if(SelectedPolygon != null)
                    {
                        SelectedPolygon.IsSelected = false;
                        SelectedPolygon = null;
                    }
                }
            }
        }

        private void CanvasMouseMove(object obj)
        {
            Canvas canvas = obj as Canvas;
            if (canvas != null)
            {
                var currentPoint = Mouse.GetPosition(canvas);
                currentPoint.X = Math.Floor(currentPoint.X);
                currentPoint.Y = Math.Floor(currentPoint.Y);

                if (_draggedCanvasElement != null)
                {
                    double xDiff = currentPoint.X - _startPoint.X;
                    double yDiff = currentPoint.Y - _startPoint.Y;

                    if (_draggedCanvasElement is Circle circle)
                    {
                        circle.CenterPointX += xDiff;
                        circle.CenterPointY += yDiff;

                        RotationX = circle.CenterPointX;
                        RotationY = circle.CenterPointY;
                        ScalingX = circle.CenterPointX;
                        ScalingY = circle.CenterPointY;
                    }
                    else if (_draggedCanvasElement is Polygon polygon)
                    {
                        if(_isDraggingCorner)
                        {
                            var angle = Math.Sqrt(xDiff * xDiff + yDiff * yDiff) / 4;
                            RotatePolygon(polygon, RotationX, RotationY, angle);
                            Mouse.OverrideCursor = Cursors.Hand;
                        }
                        else
                        {
                            TranslatePolygon(polygon, xDiff, yDiff);
                            Mouse.OverrideCursor = Cursors.SizeAll;
                        }
                    }
                }

                _startPoint = currentPoint;
            }
        }

        private void CanvasMouseLeftButtonUp(object obj)
        {
            _draggedCanvasElement = null;
            Mouse.OverrideCursor = null;
        }

        private void CanvasMouseWheel(object obj)
        {
            var args = obj as MouseWheelEventArgs;
            if(args != null)
            {
                if (args.OriginalSource is System.Windows.Shapes.Polygon)
                {
                    var polygon = (args.OriginalSource as FrameworkElement).DataContext as Polygon;
                    var k = 1 + args.Delta / 1000.0;
                    ScalePolygon(polygon, _startPoint.X, _startPoint.Y, k);
                }
            }
        }

        public void KeyDown(KeyEventArgs args)
        {
            switch (args.Key)
            {
                case Key.Enter:
                    if(_circles.Count > 2)
                    {
                        var polygon = new Polygon();

                        foreach(var circle in _circles)
                        {
                            polygon.Points.Add(new Point(circle.CenterPointX, circle.CenterPointY));
                        }

                        SetPolygon(polygon);
                        ClearCircles();
                    }
                    break;

                case Key.Escape:
                    ClearCircles();
                    break;

                case Key.Delete:
                    if(_selectedPolygon != null)
                    {
                        RemovePolygon(_selectedPolygon);
                    }
                    break;
            }
        }

        private void ClearCircles()
        {
            _circles.Clear();

            foreach (var canvasElement in CanvasElements.Reverse())
            {
                if (canvasElement is Circle)
                {
                    CanvasElements.Remove(canvasElement);
                }
            }
        }

        private void Translate(object obj)
        {
            TranslatePolygon(_selectedPolygon, TranslationX, TranslationY);
        }

        private void Rotate(object obj)
        {
            RotatePolygon(_selectedPolygon, RotationX, RotationY, RotationAngle);
        }

        private void Scale(object obj)
        {
            ScalePolygon(_selectedPolygon, ScalingX, ScalingY, ScalingK);
        }

        private void TranslatePolygon(Polygon polygon, double vectorX, double vectorY)
        {
            var pointCollection = new PointCollection();

            foreach (var point in polygon.Points)
            {
                double x = point.X + vectorX;
                double y = point.Y + vectorY;
                pointCollection.Add(new Point(x, y));
            }

            polygon.Points = pointCollection;
        }

        private void RotatePolygon(Polygon polygon, double startX, double startY, double angle)
        {
            var pointCollection = new PointCollection();
            double radians = angle * (Math.PI / 180);

            foreach (var point in polygon.Points)
            {
                double x = startX + (point.X - startX) * Math.Cos(radians) - (point.Y - startY) * Math.Sin(radians);
                double y = startY + (point.X - startX) * Math.Sin(radians) + (point.Y - startY) * Math.Cos(radians);
                pointCollection.Add(new Point(x, y));
            }

            polygon.Points = pointCollection;
        }

        private void ScalePolygon(Polygon polygon, double startX, double startY, double k)
        {
            var pointCollection = new PointCollection();

            foreach (var point in polygon.Points)
            {
                double x = point.X * k + (1 - k) * startX;
                double y = point.Y * k + (1 - k) * startY;
                pointCollection.Add(new Point(x, y));
            }

            polygon.Points = pointCollection;
        }
    }
}
