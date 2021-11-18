namespace FirstWpfApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private double _leftBound;
        private double _rightBound;
        private double _accuracy;
        private string _synchronizedText;

        public string SynchronizedText
        {
            get => _synchronizedText;
            set
            {
                _synchronizedText = value;
                OnPropertyChanged(nameof(SynchronizedText));
            }
        }
        public double LeftBound
        {
            get => _leftBound;
            set => Set(ref _leftBound, value);
        }

        public double RightBound
        {
            get => _rightBound;
            set => Set(ref _rightBound, value);
        }

        public double Accuracy
        {
            get => _accuracy;
            set => Set(ref _accuracy, value);
            
        }
        
    }
}