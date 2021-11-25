using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project6;
using GrafikaKomputerowa.Models.Project7;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;

namespace GrafikaKomputerowa.ViewModels
{
    public class AddEditPolygonViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand AddPointCommand { get; }
        public ICommand RemovePointCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        #endregion

        #region Properties
        public Color FillColor { get; set; }
        public ObservableCollection<Point> Points { get; }
        public string WindowTitle { get; }
        public Polygon Polygon { get; }

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get
            {
                return _dialogResult;
            }
            set
            {
                _dialogResult = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Variables
        private IDialogCoordinator _dialogCoordinator;
        #endregion

        public AddEditPolygonViewModel(IDialogCoordinator dialogCoordinator, Polygon polygon)
        {
            _dialogCoordinator = dialogCoordinator;

            Points = new ObservableCollection<Point>();

            if (polygon == null)
            {
                WindowTitle = "Dodaj wielokąt";
                Polygon = new Polygon();
                Points.Add(new Point(10, 10));
                Points.Add(new Point(150, 120));
                Points.Add(new Point(300, 400));
            }
            else
            {
                WindowTitle = "Edytuj wielokąt";
                Polygon = polygon;

                foreach(var point in polygon.Points)
                {
                    Points.Add(new Point(point.X, point.Y));
                }
            }

            FillColor = Polygon.FillColor;

            AddPointCommand = new RelayCommand(AddPoint);
            RemovePointCommand = new RelayCommand(RemovePoint);
            SaveCommand = new RelayCommand(Save);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void AddPoint(object obj)
        {
            Points.Add(new Point(0, 0));
        }

        private async void RemovePoint(object obj)
        {
            var metroDialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Tak",
                NegativeButtonText = "Nie",
                AnimateHide = false
            };

            var result = await _dialogCoordinator.ShowMessageAsync(this, "Uwaga", "Czy na pewno chcesz usunąć ten punkt?", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
            if (result == MessageDialogResult.Affirmative)
            {
                var point = (Point)obj;
                Points.Remove(point);
            }
        }

        private async void Save(object obj)
        {
            if(Points.Count < 3)
            {
                var metroDialogSettings = new MetroDialogSettings()
                {
                    AnimateHide = false
                };

                var result = await _dialogCoordinator.ShowMessageAsync(this, "Błąd", "Zdefiniuj co najmniej 3 punkty");
                return;
            }

            var pointCollection = new PointCollection();

            foreach(var point in Points)
            {
                pointCollection.Add(new System.Windows.Point(point.X, point.Y));
            }

            Polygon.Points = pointCollection;
            Polygon.FillColor = FillColor;
            DialogResult = true;
        }

        private void Cancel(object obj)
        {
            DialogResult = false;
        }
    }
}
