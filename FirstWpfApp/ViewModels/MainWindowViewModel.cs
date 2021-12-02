using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using FirstWpfApp.Infrastructure;
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
            var pickedFunction = MathFunction;
            _goldRatio = new GoldRatioBehavior(LeftBound, RightBound, Accuracy, pickedFunction);

            PointOfMin = Math.Round(_goldRatio.FindMin(), 4);
            MinValueOfFunction = Math.Round(_goldRatio.MinValue(), 4);
            
            var allIterationList = _goldRatio.AllIterationList;
            
            CreateSeriesCollection(pickedFunction);
            // ChartVisualElements = new VisualElementsCollection();

            ChartVisualElements.Clear();
            CreateVisualizationOnChart(allIterationList, pickedFunction);
            // ChartVisualElements.Clear();
            
            OnPropertyChanged(nameof(PointOfMin));
            OnPropertyChanged(nameof(MinValueOfFunction));
            OnPropertyChanged(nameof(SeriesCollection));
        }

        private void CreateVisualizationOnChart(List<Iteration> allIterationList, Func<double, double> pickedFunction)
        {
            foreach (var iteration in allIterationList)
            {
                ChartVisualElements.Add(new VisualElement
                {
                    X = iteration.MinPointX,
                    Y = pickedFunction(iteration.MinPointX),
                    UIElement = new Line
                    {
                        Stroke = Brushes.Black,
                        // X1 = iteration.MinPointX,
                        Y1 = pickedFunction(iteration.MinPointX) + 10,
                        // X2 = iteration.MinPointX,
                        Y2 = pickedFunction(iteration.MinPointX) - 10,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Center,
                        StrokeThickness = 1,
                    }
                });
                OnPropertyChanged(nameof(ChartVisualElements));
            }
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
            
            ChartVisualElements = new VisualElementsCollection();
            
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Пример функции",
                    Values = new ChartValues<int> {5, 3, 6, 1}
                }
            };
            
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
        
        public VisualElementsCollection ChartVisualElements { get; private set; }

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

        private void CreateSeriesCollection(Func<double, double> pickedFunction)
        {
            var functionPoints = new ChartValues<ObservablePoint>();

            for (var x = LeftBound - 5; x < RightBound + 5; x += 0.1)
            {
                double roundedX = Math.Round(x, 4);
                functionPoints.Add(new ObservablePoint(roundedX, pickedFunction(roundedX)));
            }
            
            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "График функции",
                    Stroke = new SolidColorBrush(Color.FromRgb(58, 0x8b, 0xED)),
                    PointGeometry = null,
                    Fill = Brushes.Transparent,
                    Values = functionPoints,
                    DataLabels = false,
                },
                new LineSeries
                {
                    Title = "Граничные точки",
                    Fill = Brushes.Transparent,
                    Stroke = new SolidColorBrush(Colors.LightGreen),
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(LeftBound, pickedFunction(LeftBound)),
                        new ObservablePoint(double.NaN, double.NaN),
                        new ObservablePoint(RightBound, pickedFunction(RightBound)), 
                    },
                },
                new LineSeries
                {
                    Stroke = new SolidColorBrush(Colors.Tomato),
                    Title = "Точка минимума",
                    Fill = Brushes.Transparent,
                    DataLabels = false,
                    Values = new ChartValues<ObservablePoint> {new ObservablePoint(PointOfMin, MinValueOfFunction)}
                }
            };
        }
    }
}