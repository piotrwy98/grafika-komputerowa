using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Views;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;

namespace GrafikaKomputerowa.ViewModels
{
    public class MainViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand NewFileCommand { get; set; }
        public ICommand OpenFileCommand { get; set; }
        public ICommand SaveFileCommand { get; set; }
        public ICommand ProjectDescriptionsCommand { get; set; }
        public ICommand WindowKeyDownCommand { get; set; }
        #endregion

        #region Properties
        public IDialogCoordinator DialogCoordinator { get; set; }
        public int TabControlSelectedIndex { get; set; }
        public Project1ViewModel Project1VM { get; set; }
        public Project2ViewModel Project2VM { get; set; }
        public Project3ViewModel Project3VM { get; set; }
        public Project4ViewModel Project4VM { get; set; }
        public Project5ViewModel Project5VM { get; set; }
        public Project6ViewModel Project6VM { get; set; }
        public Project7ViewModel Project7VM { get; set; }
        public Project8ViewModel Project8VM { get; set; }
        #endregion

        public MainViewModel(IDialogCoordinator dialogCoordinator)
        {
            DialogCoordinator = dialogCoordinator;

            NewFileCommand = new RelayCommand(NewFile);
            OpenFileCommand = new RelayCommand(OpenFile);
            SaveFileCommand = new RelayCommand(SaveFile);
            ProjectDescriptionsCommand = new RelayCommand(ProjectDescriptions);
            WindowKeyDownCommand = new RelayCommand(WindowKeyDown);

            Project1VM = new Project1ViewModel();
            Project2VM = new Project2ViewModel();
            Project3VM = new Project3ViewModel();
            Project4VM = new Project4ViewModel();
            Project5VM = new Project5ViewModel();
            Project6VM = new Project6ViewModel();
            Project7VM = new Project7ViewModel();
            Project8VM = new Project8ViewModel();
        }

        private void NewFile(object obj)
        {
            switch (TabControlSelectedIndex)
            {
                case 0:
                    Project1VM.NewFile();
                    break;
            }
        }

        private void OpenFile(object obj)
        {
            switch (TabControlSelectedIndex)
            {
                case 0:
                    Project1VM.OpenFile();
                    break;

                case 1:
                    Project2VM.OpenFile();
                    break;

                case 3:
                    Project4VM.OpenFile();
                    break;

                case 4:
                    Project5VM.OpenFile();
                    break;

                case 6:
                    Project7VM.OpenFile();
                    break;

                case 7:
                    Project8VM.OpenFile();
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

                case 1:
                    Project2VM.SaveFile();
                    break;

                case 6:
                    Project7VM.SaveFile();
                    break;
            }
        }

        private void ProjectDescriptions(object obj)
        {
            ProjectDescriptionsWindow descsWindow = new ProjectDescriptionsWindow();
            descsWindow.Show();
        }

        private void WindowKeyDown(object obj)
        {
            var args = obj as KeyEventArgs;
            if (args == null)
                return;

            switch (TabControlSelectedIndex)
            {
                case 0:
                    Project1VM.KeyDown(args);
                    break;

                case 6:
                    Project7VM.KeyDown(args);
                    break;
            }
        }
    }
}
