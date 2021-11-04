using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project4;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project4ViewModel : NotifyPropertyChanged
    {
        #region Commands
        public ICommand ApplyTransformationCommand { get; set; }

        public ICommand ApplyFilterCommand { get; set; }

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

        public string[] Transformations { get; } = new string[] { "Dodawanie", "Odejmowanie", "Mnożenie", "Dzielenie", "Zmiana jasności (%)", "Skala szarości v1", "Skala szarości v2" };

        public string[] Filters { get; } = new string[] { "Wygładzający (uśredniający)", "Medianowy", "Wykrywania krawędzi (sobel)", "Górnoprzepustowy wyostrzający", "Rozmycie gaussowskie" };

        private int _selectedTransformationIndex;
        public int SelectedTransformationIndex
        {
            get
            {
                return _selectedTransformationIndex;
            }
            set
            {
                _selectedTransformationIndex = value;
                OnPropertyChanged();

                IsTransformationValueVisible = value != (int) TransformationType.GRAYSCALE_V1 &&
                                               value != (int) TransformationType.GRAYSCALE_V2;
            }
        }

        private int _selectedFilterIndex;
        public int SelectedFilterIndex
        {
            get
            {
                return _selectedFilterIndex;
            }
            set
            {
                _selectedFilterIndex = value;
                OnPropertyChanged();
            }
        }

        private int _transformationValue;
        public int TransformationValue
        {
            get
            {
                return _transformationValue;
            }
            set
            {
                _transformationValue = value;
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

        private bool _isTransformationValueVisible = true;
        public bool IsTransformationValueVisible
        {
            get
            {
                return _isTransformationValueVisible;
            }
            set
            {
                _isTransformationValueVisible = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Variables
        private Bitmap _originalBitmap;
        private Bitmap _currentBitmap;
        #endregion

        public Project4ViewModel()
        {
            ApplyTransformationCommand = new RelayCommand(ApplyTransformation);
            ApplyFilterCommand = new RelayCommand(ApplyFiter);
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

        private void ApplyTransformation(object obj)
        {
            int pixelIndex = 0;
            int x = 0;
            int y = 0;

            while (pixelIndex < _currentBitmap.Width * _currentBitmap.Height)
            {
                var currentPixel = _currentBitmap.GetPixel(x, y);
                byte r = currentPixel.R;
                byte g = currentPixel.G;
                byte b = currentPixel.B;

                switch ((TransformationType) SelectedTransformationIndex)
                {
                    case TransformationType.ADD:
                        r = (byte)((r + _transformationValue) % 256);
                        g = (byte)((g + _transformationValue) % 256);
                        b = (byte)((b + _transformationValue) % 256);
                        break;

                    case TransformationType.SUBTRACT:
                        r = (byte)((r - _transformationValue) % 256);
                        g = (byte)((g - _transformationValue) % 256);
                        b = (byte)((b - _transformationValue) % 256);
                        break;

                    case TransformationType.MULTIPLY:
                        r = (byte)((r * _transformationValue) % 256);
                        g = (byte)((g * _transformationValue) % 256);
                        b = (byte)((b * _transformationValue) % 256);
                        break;

                    case TransformationType.DIVIDE:
                        r = (byte)((r / _transformationValue) % 256);
                        g = (byte)((g / _transformationValue) % 256);
                        b = (byte)((b / _transformationValue) % 256);
                        break;

                    case TransformationType.BRIGHTNESS:
                        int rBright = (int) (r * (_transformationValue / 100.0));
                        int gBright = (int) (g * (_transformationValue / 100.0));
                        int bBright = (int) (b * (_transformationValue / 100.0));

                        if(rBright < 0)
                        {
                            r = 0;
                        }
                        else if(rBright > 255)
                        {
                            r = 255;
                        }
                        else
                        {
                            r = (byte) rBright;
                        }

                        if (gBright < 0)
                        {
                            g = 0;
                        }
                        else if (gBright > 255)
                        {
                            g = 255;
                        }
                        else
                        {
                            g = (byte) gBright;
                        }

                        if (bBright < 0)
                        {
                            b = 0;
                        }
                        else if (bBright > 255)
                        {
                            b = 255;
                        }
                        else
                        {
                            b = (byte) bBright;
                        }

                        break;

                    case TransformationType.GRAYSCALE_V1:
                        r = (byte)((r + g + b) / 3);
                        g = r;
                        b = r;
                        break;

                    case TransformationType.GRAYSCALE_V2:
                        r = (byte)(r * 0.2126 + g * 0.7152 + b * 0.0722);
                        g = r;
                        b = r;
                        break;
                }

                _currentBitmap.SetPixel(x, y, Color.FromArgb(r, g, b));

                pixelIndex++;
                x++;

                if (x >= _currentBitmap.Width)
                {
                    x = 0;
                    y++;
                }
            }

            LoadCurrentBitmap();
        }

        private void ApplyFiter(object obj)
        {
            var bitmap = new Bitmap(_currentBitmap.Width, _currentBitmap.Height);
            int pixelIndex = 0;
            int x = 0;
            int y = 0;

            while (pixelIndex < _currentBitmap.Width * _currentBitmap.Height)
            {
                List<byte?> maskValuesR = new List<byte?>();
                List<byte?> maskValuesG = new List<byte?>();
                List<byte?> maskValuesB = new List<byte?>();

                for(int maskY = -1; maskY <= 1; maskY++)
                {
                    for (int maskX = -1; maskX <= 1; maskX++)
                    {
                        if(x + maskX >= 0 && x + maskX < _currentBitmap.Width &&
                           y + maskY >= 0 && y + maskY < _currentBitmap.Height)
                        {
                            var maskCurrentPixel = _currentBitmap.GetPixel(x + maskX, y + maskY);
                            maskValuesR.Add(maskCurrentPixel.R);
                            maskValuesG.Add(maskCurrentPixel.G);
                            maskValuesB.Add(maskCurrentPixel.B);
                        }
                        else
                        {
                            maskValuesR.Add(null);
                            maskValuesG.Add(null);
                            maskValuesB.Add(null);
                        }
                    }
                }

                int[] mask = null;

                switch ((FilterType) SelectedFilterIndex)
                {
                    case FilterType.AVERAGE:
                        mask = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 };
                        break;

                    case FilterType.SOBEL:
                        mask = new int[] { -1, -2, -1, 0, 0, 0, 1, 2, 1 };
                        break;

                    case FilterType.HIGH_PASS:
                        mask = new int[] { -1, -1, -1, -1, 9, -1, -1, -1, -1 };
                        break;

                    case FilterType.GAUSSIAN:
                        mask = new int[] { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
                        break;
                }

                byte r = GetFilteredValue(mask, maskValuesR);
                byte g = GetFilteredValue(mask, maskValuesG);
                byte b = GetFilteredValue(mask, maskValuesB);

                bitmap.SetPixel(x, y, Color.FromArgb(r, g, b));

                pixelIndex++;
                x++;

                if (x >= _currentBitmap.Width)
                {
                    x = 0;
                    y++;
                }
            }

            _currentBitmap = bitmap;
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

        private byte GetFilteredValue(int[] mask, List<byte?> values)
        {
            if(mask != null)
            {
                int sum = 0;
                int divisor = 0;

                for (int i = 0; i < 9; i++)
                {
                    if (values[i] != null)
                    {
                        sum += mask[i] * (int)values[i];
                        divisor += mask[i];
                    }
                }

                if (divisor != 0)
                {
                    return (byte)((sum / divisor) % 256);
                }

                return (byte)(sum % 256);
            }

            byte?[] notNullValues = values.FindAll(x => x.HasValue).ToArray();
            Array.Sort(notNullValues);

            int size = notNullValues.Length;
            int mid = size / 2;
            byte median = (size % 2 != 0) ? (byte) notNullValues[mid] : (byte)((notNullValues[mid] + notNullValues[mid - 1]) / 2);
            return median;
        }
    }
}
