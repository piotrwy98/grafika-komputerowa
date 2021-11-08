using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project5;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project5ViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand ApplyHistogramCommand { get; set; }

        public ICommand ApplyBinarizationCommand { get; set; }

        public ICommand ResetImageCommand { get; set; }
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

        public string[] Histograms { get; } = new string[] { "Rozszerzenie", "Wyrównanie" };

        public string[] Binarizations { get; } = new string[] { "Ręczny próg", "Procentowa selekcja czarnego", "Selekcja entropii" };

        private int _selectedHistogramIndex;
        public int SelectedHistogramIndex
        {
            get
            {
                return _selectedHistogramIndex;
            }
            set
            {
                _selectedHistogramIndex = value;
                OnPropertyChanged();
            }
        }

        private int _selectedBinarizationIndex;
        public int SelectedBinarizationIndex
        {
            get
            {
                return _selectedBinarizationIndex;
            }
            set
            {
                _selectedBinarizationIndex = value;
                OnPropertyChanged();

                IsBinarizationValueVisible = value < 2;
            }
        }

        private byte _binarizationValue;
        public byte BinarizationValue
        {
            get
            {
                return _binarizationValue;
            }
            set
            {
                _binarizationValue = value;
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

        private bool _isBinarizationValueVisible = true;
        public bool IsBinarizationValueVisible
        {
            get
            {
                return _isBinarizationValueVisible;
            }
            set
            {
                _isBinarizationValueVisible = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Variables
        private Bitmap _originalBitmap;
        private Bitmap _currentBitmap;

        private int[] _rHistogram;
        private int[] _gHistogram;
        private int[] _bHistogram;
        #endregion

        public Project5ViewModel()
        {
            ApplyHistogramCommand = new RelayCommand(ApplyHistogram);
            ApplyBinarizationCommand = new RelayCommand(ApplyBinarization);
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
                _currentBitmap = new Bitmap(_originalBitmap);
                LoadCurrentBitmap();
                IsImageLoaded = true;
            }
        }

        private void ApplyHistogram(object obj)
        {
            GetHistograms();

            int[] rLut = null;
            int[] gLut = null;
            int[] bLut = null;

            switch ((HistogramType) SelectedHistogramIndex)
            {
                case HistogramType.EXPANSION:
                    rLut = GetExpansionLut(_rHistogram);
                    gLut = GetExpansionLut(_gHistogram);
                    bLut = GetExpansionLut(_bHistogram);
                    break;

                case HistogramType.EQUALIZATION:
                    rLut = GetEqualizationLut(_rHistogram);
                    gLut = GetEqualizationLut(_gHistogram);
                    bLut = GetEqualizationLut(_bHistogram);
                    break;
            }

            for (int x = 0; x < _currentBitmap.Width; x++)
            {
                for (int y = 0; y < _currentBitmap.Height; y++)
                {
                    var pixelColor = _currentBitmap.GetPixel(x, y);
                    var newPixelColor = Color.FromArgb(rLut[pixelColor.R], gLut[pixelColor.G], bLut[pixelColor.B]);
                    _currentBitmap.SetPixel(x, y, newPixelColor);
                }
            }

            LoadCurrentBitmap();
        }

        private void ApplyBinarization(object obj)
        {
            switch((BinarizationType) _selectedBinarizationIndex)
            {
                case BinarizationType.MANUAL_SELECTION:
                    for (int x = 0; x < _currentBitmap.Width; x++)
                    {
                        for (int y = 0; y < _currentBitmap.Height; y++)
                        {
                            var pixelColor = _currentBitmap.GetPixel(x, y);
                            double grayScale = (pixelColor.R + pixelColor.G + pixelColor.B) / 3.0;
                            var newPixelColor = grayScale < _binarizationValue ? Color.Black : Color.White;
                            _currentBitmap.SetPixel(x, y, newPixelColor);
                        }
                    }
                    break;

                case BinarizationType.PERCENT_BLACK_SELECTION:
                    if(_binarizationValue > 100)
                    {
                        BinarizationValue = 100;
                    }

                    var bitmap = new Bitmap(_currentBitmap.Width, _currentBitmap.Height);
                    var grayHistogram = new int[256];

                    for (int x = 0; x < _currentBitmap.Width; x++)
                    {
                        for (int y = 0; y < _currentBitmap.Height; y++)
                        {
                            var pixelColor = _currentBitmap.GetPixel(x, y);
                            byte grayScale = (byte)((pixelColor.R + pixelColor.G + pixelColor.B) / 3.0);
                            bitmap.SetPixel(x, y, Color.FromArgb(grayScale, grayScale, grayScale));
                            grayHistogram[grayScale]++;
                        }
                    }

                    double maxBlackPixels = _binarizationValue * 0.01 * _currentBitmap.Width * _currentBitmap.Height;
                    double blackPixelsSum = 0;
                    byte treshold = 0;

                    for(int i = 0; i < grayHistogram.Length; i++)
                    {
                        treshold = (byte) i;
                        blackPixelsSum += grayHistogram[i];

                        if(blackPixelsSum >= maxBlackPixels)
                        {
                            break;
                        }
                    }

                    for (int x = 0; x < _currentBitmap.Width; x++)
                    {
                        for (int y = 0; y < _currentBitmap.Height; y++)
                        {
                            var pixelColor = bitmap.GetPixel(x, y);
                            var newPixelColor = pixelColor.R < treshold ? Color.Black : Color.White;
                            _currentBitmap.SetPixel(x, y, newPixelColor);
                        }
                    }
                    break;

                case BinarizationType.ENTROPY_SELECTION:
                    bitmap = new Bitmap(_currentBitmap.Width, _currentBitmap.Height);
                    grayHistogram = new int[256];

                    for (int x = 0; x < _currentBitmap.Width; x++)
                    {
                        for (int y = 0; y < _currentBitmap.Height; y++)
                        {
                            var pixelColor = _currentBitmap.GetPixel(x, y);
                            byte grayScale = (byte)((pixelColor.R + pixelColor.G + pixelColor.B) / 3.0);
                            bitmap.SetPixel(x, y, Color.FromArgb(grayScale, grayScale, grayScale));
                            grayHistogram[grayScale]++;
                        }
                    }

                    double entropyTreshold = 0;
                    double pixelCount = _currentBitmap.Width * _currentBitmap.Height;

                    for (int i = 0; i < grayHistogram.Length; i++)
                    {
                        double probability = grayHistogram[i] / pixelCount;

                        if(probability != 0)
                        {
                            entropyTreshold += probability * Math.Log(probability);
                        }
                    }

                    entropyTreshold = -entropyTreshold;

                    for (int x = 0; x < _currentBitmap.Width; x++)
                    {
                        for (int y = 0; y < _currentBitmap.Height; y++)
                        {
                            var pixelColor = bitmap.GetPixel(x, y);
                            var newPixelColor = pixelColor.R < entropyTreshold ? Color.Black : Color.White;
                            _currentBitmap.SetPixel(x, y, newPixelColor);
                        }
                    }
                    break;
            }

            LoadCurrentBitmap();
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

        private void GetHistograms()
        {
            _rHistogram = new int[256];
            _gHistogram = new int[256];
            _bHistogram = new int[256];

            for (int x = 0; x < _currentBitmap.Width; x++)
            {
                for (int y = 0; y < _currentBitmap.Height; y++)
                {
                    var pixelColor = _currentBitmap.GetPixel(x, y);
                    _rHistogram[pixelColor.R]++;
                    _gHistogram[pixelColor.G]++;
                    _bHistogram[pixelColor.B]++;
                }
            }
        }

        private int[] GetExpansionLut(int[] values)
        {
            int minValue = 0;
            for (int i = 0; i < 256; i++)
            {
                if (values[i] != 0)
                {
                    minValue = i;
                    break;
                }
            }

            int maxValue = 255;
            for (int i = 255; i >= 0; i--)
            {
                if (values[i] != 0)
                {
                    maxValue = i;
                    break;
                }
            }

            int[] result = new int[256];
            double a = 255.0 / (maxValue - minValue);
            for (int i = 0; i < 256; i++)
            {
                result[i] = (int)(a * (i - minValue));
            }

            return result;
        }

        private int[] GetEqualizationLut(int[] values)
        {
            double minValue = 0;
            for (int i = 0; i < 256; i++)
            {
                if (values[i] != 0)
                {
                    minValue = values[i];
                    break;
                }
            }

            int[] result = new int[256];
            double sum = 0;
            for (int i = 0; i < 256; i++)
            {
                sum += values[i];
                result[i] = (int)(((sum - minValue) 
                    / (_currentBitmap.Width * _currentBitmap.Height - minValue)) * 255.0);
            }

            return result;
        }
    }
}
