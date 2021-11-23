using System;
using System.Net.Security;
using System.Windows.Input;
using System.Windows.Media;
using FirstWpfApp.Infrastructure.Commands;
using FirstWpfApp.Models;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace FirstWpfApp.ViewModels
{
    public enum FunctionsEnum
    {
        FirstFunction,
        SecondFunction,
        ThirdFunction,
        FourthFunction
    }

    public class MainWindowViewModel : BaseViewModel
    {
        private double _leftBound;
        private double _rightBound;
        private double _accuracy;
        public double PointOfMin { get; private set; }
        public double MinValueOfFunction { get; private set; }

        private FunctionsEnum _checkedFunction = FunctionsEnum.FirstFunction;
        private readonly Functions _functions = new Functions();

        private GoldRatioBehavior _goldRatio;

        public ICommand PerformCalculationCommand { get; }
        public ICommand ClearAllFieldsCommand { get; }

        private bool CanPerformCalculationCommandExecute(object p) => true;
        private bool CanClearAllFieldsCommandCommandExecute(object p) => true;

        private void OnPerformCalcultaionCommandExecuted(object p)
        {
            _goldRatio = new GoldRatioBehavior();

            Func<double, double> pickedFunction = MathFunction;
            
            PointOfMin = _goldRatio.FindMin(LeftBound, RightBound, Accuracy, pickedFunction);
            MinValueOfFunction = _goldRatio.MinValue(PointOfMin, pickedFunction);
            
            var points = new ChartValues<ObservablePoint>();

            for (var x = LeftBound; x < RightBound; x += 0.1)
            {
                points.Add(new ObservablePoint(x, pickedFunction(x)));
            }
            
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    PointGeometry = null,
                    Fill = Brushes.Transparent,
                    DataLabels = false,
                    Values = points,
                },
                new LineSeries
                {
                    Fill = Brushes.Transparent,
                    DataLabels = false,
                    Values = new ChartValues<ObservablePoint> {new ObservablePoint(PointOfMin, MinValueOfFunction)}
                }
            };
            
            OnPropertyChanged(nameof(PointOfMin));
            OnPropertyChanged(nameof(MinValueOfFunction));
            OnPropertyChanged(nameof(SeriesCollection));
        }

        private void OnClearAllFieldsCommandCommandExecute(object p)
        {
            LeftBound = 0;
            RightBound = 0;
            Accuracy = 0;
            PointOfMin = 0;
            MinValueOfFunction = 0;
            OnPropertyChanged(nameof(LeftBound));
            OnPropertyChanged(nameof(RightBound));
            OnPropertyChanged(nameof(Accuracy));
            OnPropertyChanged(nameof(PointOfMin));
            OnPropertyChanged(nameof(MinValueOfFunction));
        }
        
        public MainWindowViewModel()
        {
            PerformCalculationCommand = new LambdaCommand(OnPerformCalcultaionCommandExecuted, 
                CanPerformCalculationCommandExecute);
            
            ClearAllFieldsCommand = new LambdaCommand(OnClearAllFieldsCommandCommandExecute,
                CanClearAllFieldsCommandCommandExecute);
        }
        
        public FunctionsEnum CheckedFunction
        {
            get => _checkedFunction;
            set
            {
                if (_checkedFunction == value)
                    return;
                
                _checkedFunction = value;
                OnPropertyChanged(nameof(CheckedFunction));
                OnPropertyChanged(nameof(IsFirstFunction));
                OnPropertyChanged(nameof(IsSecondFunction));
                
            }
        }
        
        public SeriesCollection SeriesCollection { get; private set; }

        public bool IsFirstFunction
        {
            get => _checkedFunction == FunctionsEnum.FirstFunction;
            set => _checkedFunction = value ? FunctionsEnum.FirstFunction : _checkedFunction;
        }
        
        public bool IsSecondFunction
        {
            get => _checkedFunction == FunctionsEnum.SecondFunction;
            set => _checkedFunction = value ? FunctionsEnum.SecondFunction : _checkedFunction;
        }
        
        public bool IsThirdFunction
        {
            get => _checkedFunction == FunctionsEnum.ThirdFunction;
            set => _checkedFunction = value ? FunctionsEnum.ThirdFunction : _checkedFunction;
        }
        
        public bool IsFourthFunction
        {
            get => _checkedFunction == FunctionsEnum.FourthFunction;
            set => _checkedFunction = value ? FunctionsEnum.FourthFunction : _checkedFunction;
        }

        private Func<double, double> MathFunction
        {
            get
            {
                switch (CheckedFunction)
                {
                    case FunctionsEnum.FirstFunction:
                        return _functions.Function1;
                    case FunctionsEnum.SecondFunction:
                        return _functions.Function2;
                    case FunctionsEnum.ThirdFunction:
                        return _functions.Function3;
                    case FunctionsEnum.FourthFunction:
                        return _functions.Function4;
                    
                    default:
                        return _functions.Function1;
                }
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