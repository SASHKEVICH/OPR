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
        public double Leftbound
        {
            get => _leftBound;
            set
            {
                _leftBound = value;
                OnPropertyChanged(nameof(Leftbound));
            }
        }

        public double Rightbound
        {
            get => _rightBound;
            set
            {
                _rightBound = value;
                OnPropertyChanged(nameof(Rightbound));
            }
        }

        public double Accuracy
        {
            get => _accuracy;
            set
            {
                _accuracy = value;
                OnPropertyChanged(nameof(Accuracy));
            }
        }
        
    }
}