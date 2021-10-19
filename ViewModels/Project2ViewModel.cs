using GrafikaKomputerowa.Models;
using GrafikaKomputerowa.Models.Project2;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
        #endregion

        #region Variables
        private int _index;
        private string _content;
        #endregion

        public void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Wybierz plik do odczytu";
            openFileDialog.Filter = "PPM|*.ppm";

            if (openFileDialog.ShowDialog() == true)
            {
                LoadFile(openFileDialog.FileName);
            }
        }

        public void SaveFile()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Wybierz plik do zapisu";
            saveFileDialog.Filter = "JPG|*.jpg";
            saveFileDialog.FileName = "saved";

            if (saveFileDialog.ShowDialog() == true)
            {

            }
        }

        public void LoadFile(string filePath)
        {
            _index = 0;
            _content = File.ReadAllText(filePath);

            string fileType = GetNextWord();
            PpmFileType ppmFileType;
            if(fileType == "P3")
            {
                ppmFileType = PpmFileType.P3;
            }
            else if(fileType == "P6")
            {
                ppmFileType = PpmFileType.P6;
            }
            else
            {
                throw new InvalidPpmFileException("Błędna definicja typu pliku");
            }

            string widthString = GetNextWord();
            if (!uint.TryParse(widthString, out uint width))
            {
                throw new InvalidPpmFileException("Błędna definicja szerokości obrazu");
            }

            string heightString = GetNextWord();
            if (!uint.TryParse(heightString, out uint height))
            {
                throw new InvalidPpmFileException("Błędna definicja wysokości obrazu");
            }

            //kolory
            GetNextWord();

            using (Bitmap bitmap = new Bitmap((int) width, (int) height))
            {
                int pixelIndex = 0;
                int x = 0;
                int y = 0;

                while(pixelIndex < width * height)
                {
                    string rString = GetNextWord();
                    if (!ushort.TryParse(rString, out ushort r))
                    {
                        throw new InvalidPpmFileException("Błędna definicja składowej R koloru pixela nr " + (pixelIndex + 1));
                    }

                    string gString = GetNextWord();
                    if (!ushort.TryParse(gString, out ushort g))
                    {
                        throw new InvalidPpmFileException("Błędna definicja składowej G koloru pixela nr " + (pixelIndex + 1));
                    }

                    string bString = GetNextWord();
                    if (!ushort.TryParse(bString, out ushort b))
                    {
                        throw new InvalidPpmFileException("Błędna definicja składowej B koloru pixela nr " + (pixelIndex + 1));
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

                MemoryStream memoryStream = new MemoryStream();
                bitmap.Save(memoryStream, ImageFormat.Bmp);
                memoryStream.Position = 0;

                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                BitmapImage = bitmapImage;
            }
        }

        public string GetNextWord()
        {
            while (_index < _content.Length && (char.IsWhiteSpace(_content[_index]) || char.IsControl(_content[_index])))
            {
                _index++;
            }

            if (_index == _content.Length)
                return null;

            if (_content[_index] == '#')
            {
                _index = _content.IndexOf('\n', _index);

                if(_index < 0)
                {
                    return null;
                }

                _index++;

                return GetNextWord();
            }

            int startIndex = _index;

            while (_index < _content.Length && !char.IsWhiteSpace(_content[_index]) && !char.IsControl(_content[_index]))
            {
                _index++;
            }

            if (_index == _content.Length)
                return null;

            return _content.Substring(startIndex, _index - startIndex);
        }
    }
}
