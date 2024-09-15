using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ImageSlider
{
    public partial class ImagePage : Page
    {
        private ObservableCollection<ImageItem> _images;
        private int _currentIndex = 0;
        private System.Timers.Timer _timer;

        public ImagePage()
        {
            InitializeComponent();

        }

        public void SetImages(ObservableCollection<ImageItem> images)
        {
            _images = images;
            if (_images.Count > 0)
            {
                DisplayImage(_currentIndex);
            }
        }

        public void NextImage()
        {
            if (_images != null && _images.Count > 0)
            {
                _currentIndex = (_currentIndex + 1) % _images.Count;
                DisplayImage(_currentIndex);
            }
        }

        public void DisplayImage(int index)
        {
            if (_images != null && index >= 0 && index < _images.Count)
            {
                DisplayedImage.Source = new BitmapImage(new Uri(_images[index].PathImage));
            }
        }

        public void PreviousImage()
        {
            if (_images != null && _currentIndex > 0)
            {
                DisplayImage(_currentIndex - 1);
            }
        }

        public void SetTimer(System.Timers.Timer timer)
        {
            _timer = timer;
        }

        public void StopTimer()
        {
            _timer?.Stop();
            _timer = null;
        }
    }
}