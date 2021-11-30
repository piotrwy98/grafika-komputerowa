using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project8;
using Microsoft.Win32;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project8ViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand ApplyMorphologyCommand { get; }
        public ICommand ResetImageCommand { get; }
        #endregion

        #region Properties
        private BitmapImage _bitmapImage;
        public BitmapImage BitmapImage
        {
            get
            {
                return _bitmapImage;
            }
            set
            {
                _bitmapImage = value;
                OnPropertyChanged();
            }
        }

        public string[] Morphologies { get; } = new string[] { "Dylatacja", "Erozja", "Otwarcie", "Domknięcie", "Hir-or-miss (pocienianie)", "Hir-or-miss (pogrubienie)" };

        private int _selectedMorphologyIndex;
        public int SelectedMorphologyIndex
        {
            get
            {
                return _selectedMorphologyIndex;
            }
            set
            {
                _selectedMorphologyIndex = value;
                OnPropertyChanged();
            }
        }

        private bool _isImageLoaded;
        public bool IsImageLoaded
        {
            get
            {
                return _isImageLoaded;
            }
            set
            {
                _isImageLoaded = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Variables
        private Bitmap _originalBitmap;
        private Bitmap _currentBitmap;
        #endregion

        public Project8ViewModel()
        {
            ApplyMorphologyCommand = new RelayCommand(ApplyMorphology);
            ResetImageCommand = new RelayCommand(ResetImage);
        }

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik do odczytu";
            openFileDialog.Filter = "JPG|*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                _originalBitmap = new Bitmap(Image.FromFile(openFileDialog.FileName));

                // konwersja na obraz binarny (biało-czarny)
                for (int x = 0; x < _originalBitmap.Width; x++)
                {
                    for (int y = 0; y < _originalBitmap.Height; y++)
                    {
                        var pixelColor = _originalBitmap.GetPixel(x, y);
                        double grayScale = (pixelColor.R + pixelColor.G + pixelColor.B) / 3.0;
                        var newPixelColor = grayScale < 110 ? Color.Black : Color.White;
                        _originalBitmap.SetPixel(x, y, newPixelColor);
                    }
                }

                _currentBitmap = new Bitmap(_originalBitmap);
                LoadCurrentBitmap();
                IsImageLoaded = true;
            }
        }

        private void ApplyMorphology(object obj)
        {
            switch ((MorphologyType) _selectedMorphologyIndex)
            {
                case MorphologyType.DILATATION:
                    PerformDilatation();
                    break;

                case MorphologyType.EROSION:
                    PerformErosion();
                    break;

                case MorphologyType.OPENING:
                    PerformErosion();
                    PerformDilatation();
                    break;

                case MorphologyType.CLOSING:
                    PerformDilatation();
                    PerformErosion();
                    break;

                case MorphologyType.THINNING:
                    PerformThinning();
                    break;

                case MorphologyType.THICKENING:
                    PerformThickening();
                    break;
            }

            LoadCurrentBitmap();
        }

        private void PerformDilatation()
        {
            var bitmap = new Bitmap(_currentBitmap.Width, _currentBitmap.Height);

            for (int x = 0; x < _currentBitmap.Width; x++)
            {
                for (int y = 0; y < _currentBitmap.Height; y++)
                {
                    bool includesBlack = false;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (x + i >= 0 && x + i < _currentBitmap.Width &&
                               y + j >= 0 && y + j < _currentBitmap.Height &&
                               _currentBitmap.GetPixel(x + i, y + j).R == 0)
                            {
                                includesBlack = true;
                                break;
                            }
                        }
                    }

                    var newPixelColor = includesBlack ? Color.Black : Color.White;
                    bitmap.SetPixel(x, y, newPixelColor);
                }
            }

            _currentBitmap = bitmap;
        }

        private void PerformErosion()
        {
            var bitmap = new Bitmap(_currentBitmap.Width, _currentBitmap.Height);

            for (int x = 0; x < _currentBitmap.Width; x++)
            {
                for (int y = 0; y < _currentBitmap.Height; y++)
                {
                    bool includesWhite = false;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (x + i >= 0 && x + i < _currentBitmap.Width &&
                               y + j >= 0 && y + j < _currentBitmap.Height &&
                               _currentBitmap.GetPixel(x + i, y + j).R == 255)
                            {
                                includesWhite = true;
                                break;
                            }
                        }
                    }

                    var newPixelColor = includesWhite ? Color.White : _currentBitmap.GetPixel(x, y);
                    bitmap.SetPixel(x, y, newPixelColor);
                }
            }

            _currentBitmap = bitmap;
        }

        private void PerformThinning()
        {
            var bitmap = new Bitmap(_currentBitmap.Width, _currentBitmap.Height);

            for (int x = 0; x < _currentBitmap.Width; x++)
            {
                for (int y = 0; y < _currentBitmap.Height; y++)
                {
                    bool isCompatible = true;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (x + i >= 0 && x + i < _currentBitmap.Width &&
                               y + j >= 0 && y + j < _currentBitmap.Height &&
                               _currentBitmap.GetPixel(x + i, y + j) != _currentBitmap.GetPixel(x, y))
                            {
                                isCompatible = false;
                                break;
                            }
                        }
                    }

                    var newPixelColor = isCompatible ? Color.White : _currentBitmap.GetPixel(x, y);
                    bitmap.SetPixel(x, y, newPixelColor);
                }
            }

            _currentBitmap = bitmap;
        }

        private void PerformThickening()
        {
            var bitmap = new Bitmap(_currentBitmap.Width, _currentBitmap.Height);

            for (int x = 0; x < _currentBitmap.Width; x++)
            {
                for (int y = 0; y < _currentBitmap.Height; y++)
                {
                    bool isCompatible = true;

                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            if (x + i >= 0 && x + i < _currentBitmap.Width &&
                               y + j >= 0 && y + j < _currentBitmap.Height &&
                               _currentBitmap.GetPixel(x + i, y + j) != _currentBitmap.GetPixel(x, y))
                            {
                                isCompatible = false;
                                break;
                            }
                        }
                    }

                    var newPixelColor = isCompatible ? Color.Black : _currentBitmap.GetPixel(x, y);
                    bitmap.SetPixel(x, y, newPixelColor);
                }
            }

            _currentBitmap = bitmap;
        }

        private void ResetImage(object obj)
        {
            _currentBitmap = new Bitmap(_originalBitmap);
            LoadCurrentBitmap();
        }

        private void LoadCurrentBitmap()
        {
            using (var memoryStream = new MemoryStream())
            {
                _currentBitmap.Save(memoryStream, ImageFormat.Jpeg);
                memoryStream.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                BitmapImage = bitmapImage;
            }
        }
    }
}
