using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project2;
using GrafikaKomputerowa.Views;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace GrafikaKomputerowa.ViewModels
{
    public class Project2ViewModel : NotifyPropertyChanged
    {
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

        private string _filePath;
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                _filePath = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Variables
        MemoryStream _memoryStream;
        private int _index;
        private byte[] _bytes;
        #endregion

        public async void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik do odczytu";
            openFileDialog.Filter = "PPM, JPG|*.ppm;*.jpg";

            if (openFileDialog.ShowDialog() == true)
            {
                if (openFileDialog.FileName.EndsWith(".jpg"))
                {
                    BitmapImage jpgImage = new BitmapImage();
                    jpgImage.BeginInit();
                    jpgImage.UriSource = new Uri(openFileDialog.FileName);
                    jpgImage.EndInit();

                    BitmapImage = jpgImage;
                }
                else
                {
                    try
                    {
                        _index = 0;
                        _bytes = File.ReadAllBytes(openFileDialog.FileName);

                        string fileType = GetNextString();
                        PpmFileType ppmFileType;
                        if (fileType == "P3")
                        {
                            ppmFileType = PpmFileType.P3;
                        }
                        else if (fileType == "P6")
                        {
                            ppmFileType = PpmFileType.P6;
                        }
                        else
                        {
                            throw new InvalidPpmFileException("Błędna definicja typu pliku");
                        }

                        string widthString = GetNextString();
                        if (!uint.TryParse(widthString, out uint width))
                        {
                            throw new InvalidPpmFileException("Błędna definicja szerokości obrazu");
                        }

                        string heightString = GetNextString();
                        if (!uint.TryParse(heightString, out uint height))
                        {
                            throw new InvalidPpmFileException("Błędna definicja wysokości obrazu");
                        }

                        string maxValueString = GetNextString();
                        if (!ushort.TryParse(maxValueString, out ushort maxValue))
                        {
                            throw new InvalidPpmFileException("Błędna definicja wartości maksymalnej");
                        }

                        if (ppmFileType == PpmFileType.P3)
                        {
                            LoadP3((int)width, (int)height, maxValue);
                        }
                        else // P6
                        {
                            LoadP6((int)width, (int)height);
                        }
                    }
                    catch (InvalidPpmFileException ex)
                    {
                        var dialogCoordinator = (Application.Current.MainWindow.DataContext as MainViewModel).DialogCoordinator;
                        await dialogCoordinator.ShowMessageAsync(this, "Błąd", ex.Message);
                        return;
                    }

                    BitmapImage ppmImage = new BitmapImage();
                    ppmImage.BeginInit();
                    ppmImage.StreamSource = _memoryStream;
                    ppmImage.EndInit();

                    BitmapImage = ppmImage;
                }

                FilePath = openFileDialog.FileName;
            }
        }

        public async void SaveFile()
        {
            if(BitmapImage == null)
            {
                var dialogCoordinator = (Application.Current.MainWindow.DataContext as MainViewModel).DialogCoordinator;
                await dialogCoordinator.ShowMessageAsync(this, "Błąd", "Brak pliku do zapisania");
                return;
            }

            Bitmap bitmap;

            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder bitmapEncoder = new BmpBitmapEncoder();
                bitmapEncoder.Frames.Add(BitmapFrame.Create(BitmapImage));
                bitmapEncoder.Save(outStream);
                Bitmap bitmapFromStream = new Bitmap(outStream);
                bitmap = new Bitmap(bitmapFromStream);
            }

            CompressionLevelWindow compressionLevelWindow = new CompressionLevelWindow(bitmap);
            compressionLevelWindow.ShowDialog();
        }

        private void LoadP3(int width, int height, ushort maxValue)
        {
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                int pixelIndex = 0;
                int x = 0;
                int y = 0;

                while (pixelIndex < width * height)
                {
                    string rString = GetNextString();
                    if (!ushort.TryParse(rString, out ushort r))
                    {
                        throw new InvalidPpmFileException("Błędna definicja składowej R koloru pixela nr " + (pixelIndex + 1));
                    }

                    string gString = GetNextString();
                    if (!ushort.TryParse(gString, out ushort g))
                    {
                        throw new InvalidPpmFileException("Błędna definicja składowej G koloru pixela nr " + (pixelIndex + 1));
                    }

                    string bString = GetNextString();
                    if (!ushort.TryParse(bString, out ushort b))
                    {
                        throw new InvalidPpmFileException("Błędna definicja składowej B koloru pixela nr " + (pixelIndex + 1));
                    }

                    if (maxValue == 65535)
                    {
                        r = (ushort)(r >> 8);
                        g = (ushort)(g >> 8);
                        b = (ushort)(b >> 8);
                    }

                    bitmap.SetPixel(x, y, Color.FromArgb(100, r, g, b));

                    pixelIndex++;
                    x++;

                    if (x >= width)
                    {
                        x = 0;
                        y++;
                    }
                }

                _memoryStream = new MemoryStream();
                bitmap.Save(_memoryStream, ImageFormat.Bmp);
                _memoryStream.Position = 0;
            }
        }

        private string GetNextString()
        {
            while (_index < _bytes.Length && (char.IsWhiteSpace((char) _bytes[_index]) || char.IsControl((char) _bytes[_index])))
            {
                _index++;
            }

            if (_index == _bytes.Length)
            {
                return null;
            }

            if (_bytes[_index] == '#')
            {
                while (_index < _bytes.Length && _bytes[_index] != '\n')
                {
                    _index++;
                }

                if (_index == _bytes.Length)
                {
                    return null;
                }

                _index++;

                return GetNextString();
            }

            int startIndex = _index;

            while (_index < _bytes.Length && !char.IsWhiteSpace((char) _bytes[_index]) && !char.IsControl((char) _bytes[_index]))
            {
                _index++;
            }

            if (_index == _bytes.Length)
            {
                return null;
            }

            return Encoding.ASCII.GetString(_bytes, startIndex, _index - startIndex);
        }

        private void LoadP6(int width, int height)
        {
            using (Bitmap bitmap = new Bitmap(width, height))
            {
                int pixelIndex = 0;
                int x = 0;
                int y = 0;

                _index++;

                while (pixelIndex < width * height)
                {
                    if (_index >= _bytes.Length)
                    {
                        throw new InvalidPpmFileException("Niepełna definicja pixeli");
                    }

                    byte r = _bytes[_index];
                    byte g = _bytes[_index + 1];
                    byte b = _bytes[_index + 2];

                    bitmap.SetPixel(x, y, Color.FromArgb(100, r, g, b));

                    _index = _index + 3;
                    pixelIndex++;
                    x++;

                    if (x >= width)
                    {
                        x = 0;
                        y++;
                    }
                }

                _memoryStream = new MemoryStream();
                bitmap.Save(_memoryStream, ImageFormat.Bmp);
                _memoryStream.Position = 0;
            }
        }
    }
}
