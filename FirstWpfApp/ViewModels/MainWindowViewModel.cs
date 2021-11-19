using System;
using System.Net.Security;
using FirstWpfApp.Models;

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
        private double _pointOfMin;
        // private double _minResult;
        
        private FunctionsEnum _checkedFunction = FunctionsEnum.FirstFunction;
        private readonly Functions _functions = new Functions();

        public double PointOfMin => _pointOfMin = GoldRatioBehavior.FindMin(LeftBound, RightBound, Accuracy, ToFunction);

        // public Func<double, double> Function
        // {
        //     get => _function;
        //     set
        //     {
        //         if(FunctionRadioButton1)
        //     }
        // }
        

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

        private Func<double, double> ToFunction
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