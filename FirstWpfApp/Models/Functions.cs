using System;

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
            return Math.Sin(x) + 5;
        }
        
        public double Function3(double x)
        {
            return Math.Pow(15 + x, x) + 5 * x * x;
        }
        
        public double Function4(double x)
        {
            return 3 * x - 5 * Math.E;
        }
    }
}