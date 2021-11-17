using System.Windows.Controls;

namespace GrafikaKomputerowa.Views
{
    /// <summary>
    /// Interaction logic for Project6View.xaml
    /// </summary>
    public partial class Project6View : UserControl
    {
        public Project6View()
        {
            InitializeComponent();
        }

        private void AddControlPoint_BT_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ControlPoints_SV.ScrollToEnd();
        }
    }
}
