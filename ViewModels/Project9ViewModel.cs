using GrafikaKomputerowa.Models;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project9ViewModel : NotifyPropertyChanged
    {
        private double? _percentOfGreen;
        public double? PercentOfGreen
        {
            get
            {
                return _percentOfGreen;
            }
            set
            {
                _percentOfGreen = value;
                OnPropertyChanged();
            }
        }

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

        private Bitmap _bitmap;

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik do odczytu";
            openFileDialog.Filter = "JPG|*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                _bitmap = new Bitmap(Image.FromFile(openFileDialog.FileName));
                PercentOfGreen = GetPercentOfGreen();

                BitmapImage jpgImage = new BitmapImage();
                jpgImage.BeginInit();
                jpgImage.UriSource = new Uri(openFileDialog.FileName);
                jpgImage.CacheOption = BitmapCacheOption.OnLoad;
                jpgImage.EndInit();

                BitmapImage = jpgImage;
            }
        }

        private double GetPercentOfGreen()
        {
            ulong totalPixelCount = (ulong) _bitmap.Width * (ulong) _bitmap.Height;
            ulong greenPixelCount = 0;

            for (int x = 0; x < _bitmap.Width; x++)
            {
                for (int y = 0; y < _bitmap.Height; y++)
                {
                    var pixelColor = _bitmap.GetPixel(x, y);

                    if(pixelColor.G > 100 &&
                       pixelColor.G > pixelColor.R &&
                       pixelColor.G > pixelColor.B)
                    {
                        greenPixelCount++;
                    }
                }
            }

            return 100.0 * greenPixelCount / totalPixelCount;
        }
    }
}
