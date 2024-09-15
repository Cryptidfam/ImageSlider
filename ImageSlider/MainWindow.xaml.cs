using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;

namespace ImageSlider
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<ImageItem> AvailableImages { get; set; }
        public ObservableCollection<ImageItem> SelectedImages { get; set; }
        private readonly ImagePage _imagePage;

        public MainWindow()
        {
            InitializeComponent();
            AvailableImages = new ObservableCollection<ImageItem>();
            SelectedImages = new ObservableCollection<ImageItem>();
            DataContext = this;

            _imagePage = new ImagePage();
            ImageSlider.Navigate(_imagePage);
            UpdateImagePage();
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    AvailableImages.Add(new ImageItem { PathImage = fileName });
                }
            }
        }

        private void AvailableImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is ImageItem imageItem)
            {
                AvailableImages.Remove(imageItem);
                SelectedImages.Add(imageItem);
                UpdateImagePage();
            }
        }

        private void SelectedImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is ImageItem imageItem)
            {
                SelectedImages.Remove(imageItem);
                AvailableImages.Add(imageItem);
                UpdateImagePage();
            }
        }

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            var selectedAvailableImages = AvailableImages.Where(img => img.IsSelected).ToList();
            var selectedSelectedImages = SelectedImages.Where(img => img.IsSelected).ToList();

            foreach (var image in selectedAvailableImages)
            {
                AvailableImages.Remove(image);
            }

            foreach (var image in selectedSelectedImages)
            {
                SelectedImages.Remove(image);
            }

            UpdateImagePage();
        }

        private void UpdateImagePage()
        {
            _imagePage.SetImages(SelectedImages);
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedImages.Count == 0)
            {
                MessageBox.Show("Please select images before starting the slideshow.");
                return;
            }

            _imagePage.SetImages(SelectedImages);

            // Set up the timer
            System.Timers.Timer timer = new System.Timers.Timer(2000); // 1000 milliseconds = 1 second
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
            timer.Enabled = true;

            // Store the timer so you can stop it later
            _imagePage.SetTimer(timer);

            UpdateImagePage();
        }

        private void SetUpTimer(object sender, EventArgs e)
        {
            //// Set up the timer
            //System.Timers.Timer timer = new System.Timers.Timer(2000); // 1000 milliseconds = 1 second
            //timer.Elapsed += Timer_Elapsed;
            //timer.AutoReset = true;
            //timer.Enabled = true;

            //// Store the timer so you can stop it later
            //_imagePage.SetTimer(timer);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                _imagePage.NextImage();
                UpdateImagePage(); //Without this images update at different time intervals
            });
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _imagePage.StopTimer();
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            _imagePage.PreviousImage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _imagePage.NextImage();
        }

        private void PlayRandomCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            int randomIndex = random.Next(0, SelectedImages.Count);
            _imagePage.DisplayImage(randomIndex);
        }
    }
}