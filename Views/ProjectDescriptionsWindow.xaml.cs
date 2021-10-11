using GrafikaKomputerowa.ViewModels;
using MahApps.Metro.Controls;

namespace GrafikaKomputerowa.Views
{
    /// <summary>
    /// Interaction logic for ProjectDescriptionsWindow.xaml
    /// </summary>
    public partial class ProjectDescriptionsWindow : MetroWindow
    {
        public ProjectDescriptionsWindow()
        {
            InitializeComponent();
            DataContext = new ProjectDescripionsViewModel();
        }
    }
}
