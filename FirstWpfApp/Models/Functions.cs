using System;
using System.Windows.Media.Animation;

namespace FirstWpfApp.Models
{
    public class Functions
    {
        public double Function1(double x)
        {
            return Math.Pow(x, 2);
        }
        
        public double Function2(double x)
        {
            return x - Math.Pow(5, x) * Math.Pow(Math.Sin(2 * x), 5);
        }
        
        public double Function3(double x)
        {
            return Math.Log(Math.Abs(x)) - Math.Pow(Math.E, x + 3) + Math.Sin(3 * x);
        }
        
        public double Function4(double x)
        {
            return 3 * x - 5 * Math.E;
        }
    }
}