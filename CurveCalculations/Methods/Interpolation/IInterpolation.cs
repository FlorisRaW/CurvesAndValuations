using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Methods.Interpolation
{
    public interface IInterpolation
    {
        public abstract double GetValue(double key);
    }
}
