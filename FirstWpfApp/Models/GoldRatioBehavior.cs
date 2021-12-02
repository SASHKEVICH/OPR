using System;
using System.Collections.Generic;
using FirstWpfApp.Infrastructure;

namespace FirstWpfApp.Models
{
    public class GoldRatioBehavior
    {
        private const double Phi = 1.618;
        private double _leftBound;
        private double _rightBound;
        private readonly double _accuracy;
        private readonly Func<double, double>  _func;
        private double _minPoint;

        public GoldRatioBehavior(double leftBound, double rightBound, double accuracy, Func<double, double> func)
        {
            _leftBound = leftBound;
            _rightBound = rightBound;
            _accuracy = accuracy;
            _func = func;

            AllIterationList = new List<Iteration>();
        }
        
        /// <summary>
        /// Метод находит точку минимума по заданному промежутку с заданной точностью
        /// </summary>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="eps"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public double FindMin()
        {
            while(Math.Abs(_rightBound - _leftBound) > _accuracy)
            {
                var x1 = _rightBound - (_rightBound - _leftBound) / Phi;
                var x2 = _leftBound + (_rightBound - _leftBound) / Phi;

                var y1 = _func(x1);
                var y2 = _func(x2);

                if (y1 >= y2)
                {
                    AllIterationList.Add(new Iteration
                    {
                        MinPointX = x1,
                        LeftBound = _leftBound,
                        RightBound = _rightBound
                    });
                    _leftBound = x1;
                }
                else
                {
                    AllIterationList.Add(new Iteration
                    {
                        MinPointX = x2,
                        LeftBound = _leftBound,
                        RightBound = _rightBound
                    });
                    _rightBound = x2;
                }
            }

            _minPoint = (_leftBound + _rightBound) / 2;
            return _minPoint;
        }

        public double MinValue() => _func(_minPoint);

        public List<Iteration> AllIterationList { get; private set; }
    }
}