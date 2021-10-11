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
        public IDialogCoordinator DialogCoordinator;
        public int TabControlSelectedIndex { get; set; }
        public Project1ViewModel Project1VM { get; set; }
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
        }

        private async void NewFile(object obj)
        {
            var metroDialogSettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Tak",
                NegativeButtonText = "Nie",
                AnimateHide = false
            };

            var result = await DialogCoordinator.ShowMessageAsync(this, "Uwaga", "Niezapisane zmiany zostaną utracone. Kontynuować?", MessageDialogStyle.AffirmativeAndNegative, metroDialogSettings);
            if (result != MessageDialogResult.Affirmative)
                return;

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
            }
        }
    }
}
