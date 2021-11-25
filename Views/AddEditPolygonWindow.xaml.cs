using GrafikaKomputerowa.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;

namespace GrafikaKomputerowa.Views
{
    /// <summary>
    /// Interaction logic for AddEditPolygonWindow.xaml
    /// </summary>
    public partial class AddEditPolygonWindow : MetroWindow
    {
        public AddEditPolygonWindow(Models.Project7.Polygon polygon = null)
        {
            InitializeComponent();
            DataContext = new AddEditPolygonViewModel(DialogCoordinator.Instance, polygon);
        }

        private void AddPoint_BT_Click(object sender, RoutedEventArgs e)
        {
            Points_SV.ScrollToEnd();
        }
    }
}
