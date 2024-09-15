using System.ComponentModel;

namespace ImageSlider
{
    public class ImageItem : INotifyPropertyChanged
    {
        private string _pathImage;
        public string PathImage
        {
            get => _pathImage;
            set
            {
                if (_pathImage != value)
                {
                    _pathImage = value;
                    OnPropertyChanged(nameof(PathImage));
                }
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}