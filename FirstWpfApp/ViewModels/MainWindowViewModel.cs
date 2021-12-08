using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using FirstWpfApp.Infrastructure;
using FirstWpfApp.Infrastructure.Commands;
using FirstWpfApp.Models;
using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

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
        private double _accuracy = 0.001;
        public double PointOfMin { get; private set; }
        public double MinValueOfFunction { get; private set; }

        private FunctionsEnum _checkedFunction = FunctionsEnum.FirstFunction;
        private readonly Functions _functions = new Functions();

        private GoldRatioBehavior _goldRatio;
        private List<Iteration> _allIterationsList;
        private Func<double, double> _pickedFunction;

        public ICommand PerformCalculationCommand { get; }
        public ICommand ClearAllFieldsCommand { get; }

        private bool CanPerformCalculationCommandExecute(object p) => true;
        private bool CanClearAllFieldsCommandCommandExecute(object p) => true;
        
        [STAThread]
        private async void OnPerformCalcultaionCommandExecuted(object p)
        {   
            _pickedFunction = MathFunction;
            _goldRatio = new GoldRatioBehavior(LeftBound, RightBound, Accuracy, _pickedFunction);

            PointOfMin = Math.Round(_goldRatio.FindMin(), 4);
            MinValueOfFunction = Math.Round(_goldRatio.MinValue(), 4);
            
            _allIterationsList = _goldRatio.AllIterationList;

            Model.Series.Clear();
            Model.Annotations.Clear();

            var myFunctionLineSeries = new FunctionSeries(_pickedFunction, LeftBound-5, RightBound+5, 0.001)
            {
                Color = OxyColor.FromRgb(58, 0x8B, 0xED)
            };
            Model.Series.Add(myFunctionLineSeries);

            await CreateVisualizationOnPlot();
            
            Model.InvalidatePlot(true);
            OnPropertyChanged(nameof(PointOfMin));
            OnPropertyChanged(nameof(MinValueOfFunction));
            OnPropertyChanged(nameof(Model));
        }

        private async Task CreateVisualizationOnPlot()
        {
            Model.InvalidatePlot(true);
            var pointExtremum = new PointAnnotation
            {
                X = PointOfMin,
                Y = MinValueOfFunction,
                Fill = OxyColors.Red,
                Size = 4,
            };

            var leftBoundAnnotation = new LineAnnotation
            {
                LineStyle = LineStyle.Dash,
                X = LeftBound,
                Type = LineAnnotationType.Vertical,
                Color = OxyColors.Green,
                StrokeThickness = 1,
                MaximumY = _pickedFunction(LeftBound) + 
                           _pickedFunction(_allIterationsList[0].MinPointX - _allIterationsList[0].LeftBound) + 25,
                MinimumY = _pickedFunction(LeftBound) + 
                           _pickedFunction(_allIterationsList[0].MinPointX - _allIterationsList[0].LeftBound) - 25,
            };
            
            var rightBoundAnnotation = new LineAnnotation
            {
                LineStyle = LineStyle.Dash,
                X = RightBound,
                Type = LineAnnotationType.Vertical,
                Color = OxyColors.Green,
                StrokeThickness = 1,
                MaximumY = _pickedFunction(RightBound) + 
                           _pickedFunction(_allIterationsList[0].MinPointX - _allIterationsList[0].RightBound) + 25,
                MinimumY = _pickedFunction(RightBound) + 
                           _pickedFunction(_allIterationsList[0].MinPointX - _allIterationsList[0].RightBound) - 25,
            };
            
            Model.Annotations.Add(leftBoundAnnotation);
            Model.Annotations.Add(rightBoundAnnotation);
            Model.InvalidatePlot(true);
            
            foreach (var iteration in _allIterationsList)
            {
                var iterationAnnotation = new LineAnnotation
                {
                    LineStyle = LineStyle.Dash,
                    X = iteration.MinPointX,
                    Type = LineAnnotationType.Vertical,
                    Color = OxyColors.Black,
                    MaximumY = _pickedFunction(iteration.MinPointX) + 4 * _pickedFunction(iteration.MinPointX),
                    MinimumY = _pickedFunction(iteration.MinPointX) - 4 * _pickedFunction(iteration.MinPointX)
                };
                await Task.Delay(100);
                Model.Annotations.Add(iterationAnnotation);
                Model.InvalidatePlot(true);
                
                var startArrowPoint = CalculateStartArrowPoint(iteration);
                var myArrowAnnotation = new ArrowAnnotation
                {
                    StartPoint = startArrowPoint,
                    EndPoint = new DataPoint(iteration.MinPointX, _pickedFunction(iteration.MinPointX)),
                    LineStyle = LineStyle.Dash,
                    Color = OxyColors.Black,
                    HeadLength = 5.0,
                    HeadWidth = 1.5,
                    StrokeThickness = 1
                };
                await Task.Delay(100);
                Model.Annotations.Add(myArrowAnnotation);
                Model.InvalidatePlot(true);
            }

            Model.Annotations.Add(pointExtremum);
            Model.InvalidatePlot(true);
        }

        private DataPoint CalculateStartArrowPoint(Iteration iteration)
        {
            return Math.Abs(iteration.LeftBound - iteration.MinPointX) >=
                   Math.Abs(iteration.RightBound - iteration.MinPointX)
                ? new DataPoint(iteration.LeftBound, _pickedFunction(iteration.MinPointX))
                : new DataPoint(iteration.RightBound, _pickedFunction(iteration.MinPointX));
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

            Model = new PlotModel();
            Model.Series.Add(new FunctionSeries(Math.Sin, 0, 1, 0.001));
        }
        
        public PlotModel Model { get; set; }
        
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