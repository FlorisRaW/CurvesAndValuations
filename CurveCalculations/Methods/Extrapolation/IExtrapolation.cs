using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Methods.Extrapolation
{
    public interface IExtrapolation
    {
        public abstract double GetValue(double key);
    }
}
