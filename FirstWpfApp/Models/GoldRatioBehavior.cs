

using System;

namespace FirstWpfApp.Models
{
    public class GoldRatioBehavior
    {
        private static double _FI = 1.618;

        public GoldRatioBehavior() {}

        /// <summary>
        /// Метод находит точку минимума по заданному промежутку с заданной точностью
        /// </summary>
        /// <param name="leftBound"></param>
        /// <param name="rightBound"></param>
        /// <param name="eps"></param>
        /// <returns></returns>
        public static double FindMin(double leftBound, double rightBound, double eps, Func<double, double> Func)
        {
            while(Math.Abs(eps - leftBound) > eps)
            {
                double x1 = rightBound - (rightBound - leftBound) / _FI;
                double x2 = eps + (rightBound - leftBound) / _FI;

                double y1 = Func(x1);
                double y2 = Func(x2);

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
    }
}