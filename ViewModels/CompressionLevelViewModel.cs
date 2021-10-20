using GrafikaKomputerowa.Models;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Input;

namespace GrafikaKomputerowa.ViewModels
{
    public class CompressionLevelViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand ApplyCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        #endregion

        #region Properties
        public long _compressionLevel = 100;
        public long CompressionLevel
        {
            get
            {
                return _compressionLevel;
            }
            set
            {
                _compressionLevel = value;
                OnPropertyChanged();
            }
        }

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
        private Bitmap _bitmap;
        #endregion

        public CompressionLevelViewModel(Bitmap bitmap)
        {
            _bitmap = bitmap;

            ApplyCommand = new RelayCommand(Apply);
            CancelCommand = new RelayCommand(Cancel);
        }

        private void Apply(object obj)
        {
            DialogResult = true;

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Wybierz plik do zapisu";
            saveFileDialog.Filter = "JPG|*.jpg";
            saveFileDialog.FileName = "saved";

            if (saveFileDialog.ShowDialog() == true)
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                EncoderParameters encoderParameters = new EncoderParameters(1);
                EncoderParameter qualityParam = new EncoderParameter(Encoder.Quality, CompressionLevel);
                encoderParameters.Param[0] = qualityParam;
                _bitmap.Save(saveFileDialog.FileName, jpgEncoder, encoderParameters);
            }
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }

            return null;
        }

        private void Cancel(object obj)
        {
            DialogResult = false;
        }
    }
}
