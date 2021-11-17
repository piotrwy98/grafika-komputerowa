using GrafikaKomputerowa.Models;
using MahApps.Metro.Controls.Dialogs;
using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Point = GrafikaKomputerowa.Models.Project6.Point;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project6ViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand RemoveControlPointCommand { get; }
        public ICommand AddControlPointCommand { get; }
        public ICommand DrawCurveCommand { get; }
        #endregion

        #region Properties
        public ObservableCollection<Point> ControlPoints { get; }
        public List<DataPoint> CurvePoints { get; }
        public PlotModel PlotModel { get; }
        #endregion

        public Project6ViewModel()
        {
            RemoveControlPointCommand = new RelayCommand(RemoveControlPoint);
            AddControlPointCommand = new RelayCommand(AddControlPoint);
            DrawCurveCommand = new RelayCommand(DrawCurve);

            ControlPoints = new ObservableCollection<Point>();
            ControlPoints.Add(new Point(2, 3));
            ControlPoints.Add(new Point(4, 3));
            ControlPoints.Add(new Point(3, 5));
            ControlPoints.Add(new Point(0, 5));

            CurvePoints = new List<DataPoint>();

            LineSeries controlPointsSeries = new LineSeries
            {
                ItemsSource = ControlPoints,
                DataFieldX = "X",
                DataFieldY = "Y",
                Color = OxyColors.Black,
                MarkerType = MarkerType.Circle,
                MarkerSize = 5
            };

            LineSeries curvePointsSeries = new LineSeries
            {
                ItemsSource = CurvePoints,
                DataFieldX = "X",
                DataFieldY = "Y",
                Color = OxyColors.ForestGreen,
                MarkerType = MarkerType.None
            };

            PlotModel = new PlotModel();
            PlotModel.Series.Add(controlPointsSeries);
            PlotModel.Series.Add(curvePointsSeries);
        }

        private async void RemoveControlPoint(object obj)
        {
            var point = obj as Point;
            if(point != null)
            {
                var metroDialogSettings = new MetroDialogSettings()
                {
                    AffirmativeButtonText = "Tak",
                    NegativeButtonText = "Nie",
                    AnimateHide = false
                };

                var dialogCoordinator = (Application.Current.MainWindow.DataContext as MainViewModel).DialogCoordinator;
                var result = await dialogCoordinator.ShowMessageAsync(this, "Uwaga", "Czy na pewno chcesz usunąć ten punkt?", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
                if (result == MessageDialogResult.Affirmative)
                {
                    ControlPoints.Remove(point);
                }
            }
        }

        private void AddControlPoint(object obj)
        {
            ControlPoints.Add(new Point(0, 0));
        }

        private void DrawCurve(object obj)
        {
            CurvePoints.Clear();

            for (double t = 0; t <= 1; t += 1.0/200)
            {
                double x = 0;
                double y = 0;

                for(int i = 0; i < ControlPoints.Count; i++)
                {
                    double bernstein = Bernstein((uint) i, (uint) ControlPoints.Count - 1, t);
                    x += bernstein * ControlPoints[i].X;
                    y += bernstein * ControlPoints[i].Y;
                }

                CurvePoints.Add(new DataPoint(x, y));
            }

            PlotModel.InvalidatePlot(true);
        }

        private double Bernstein(uint i, uint n, double t)
        {
            return Newton(n, i) * Math.Pow(t, i) * Math.Pow(1 - t, n - i);
        }

        private uint Newton(uint n, uint k)
        {
            if (k == 0 || k == n)
            {
                return 1;
            }

            return Newton(n - 1, k - 1) + Newton(n - 1, k);
        }
    }
}
