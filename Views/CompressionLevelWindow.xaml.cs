using GrafikaKomputerowa.ViewModels;
using MahApps.Metro.Controls;
using System.Drawing;

namespace GrafikaKomputerowa.Views
{
    /// <summary>
    /// Interaction logic for CompressionLevelWindow.xaml
    /// </summary>
    public partial class CompressionLevelWindow : MetroWindow
    {
        public CompressionLevelWindow(Bitmap bitmap)
        {
            InitializeComponent();
            DataContext = new CompressionLevelViewModel(bitmap);
        }
    }
}
