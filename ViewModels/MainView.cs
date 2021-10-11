using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Views;
using System.Windows.Input;

namespace GrafikaKomputerowa.ViewModels
{
    public class MainView : NotifyPropertyChanged
    {
        #region Commands
        public ICommand OpenFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand ProjectDescriptionsCommand { get; set; }
        #endregion

        #region Properties
        public int TabControlSelectedIndex { get; set; }
        public Project1ViewModel Project1VM { get; set; }
        #endregion

        public MainView()
        {
            OpenFileCommand = new RelayCommand(OpenFile);
            SaveFileCommand = new RelayCommand(SaveFile);
            ProjectDescriptionsCommand = new RelayCommand(ProjectDescriptions);

            Project1VM = new Project1ViewModel();
        }

        private void OpenFile(object obj)
        {
            switch (TabControlSelectedIndex)
            {
                case 0:
                    Project1VM.OpenFile();
                    break;
            }
        }

        private void SaveFile(object obj)
        {
            switch (TabControlSelectedIndex)
            {
                case 0:
                    Project1VM.SaveFile();
                    break;
            }
        }

        private void ProjectDescriptions(object obj)
        {
            ProjectDescriptionsWindow descsWindow = new ProjectDescriptionsWindow();
            descsWindow.Show();
        }
    }
}
