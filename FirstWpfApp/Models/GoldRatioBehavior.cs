

using System;

namespace FirstWpfApp.Models
{
    public class GoldRatioBehavior
    {
        private const double Phi = 1.618;

        /// <summary>
        /// Метод находит точку минимума по заданному промежутку с заданной точностью
        /// </summary>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="eps"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public double FindMin(double leftBound, double rightBound, double eps, Func<double, double> func)
        {
            while(Math.Abs(rightBound - leftBound) > eps)
            {
                double x1 = rightBound - (rightBound - leftBound) / Phi;
                double x2 = leftBound + (rightBound - leftBound) / Phi;

                double y1 = func(x1);
                double y2 = func(x2);

                if (y1 >= y2)
                {
                    leftBound = x1;
                }
                else
                {
                    rightBound = x2;
                }
            }

            return (leftBound + rightBound) / 2;
        }

        public double MinValue(double pointOfMin, Func<double, double> func) => func(pointOfMin);
    }
}