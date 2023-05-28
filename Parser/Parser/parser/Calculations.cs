using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser.parser
{
    internal class Calculations
    {
        //_                 _
        //Xn = aXn + (1 - a)Xn-1, где a = 0..1
        const double a = 0.9;
        public void EvaluationCalculation(double Xi, double Xr, double i) 
        {
            double Xn = a * Xi + (1 - a) * Xr;
        }
    }
}
