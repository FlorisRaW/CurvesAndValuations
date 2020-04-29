using CurveCalculations.Modifiers.Interpolation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Methods.Interpolation
{
    public class InterpolationFactory
    {
        private IDictionary<double, double> KnownNodes { get; set; }

        public InterpolationFactory(IDictionary<double, double> knownNodes)
        {
            KnownNodes = knownNodes;
        }

        public IInterpolation GetInterpolator(InterpolationType interpolationType)
        {
            switch (interpolationType)
            {
                case InterpolationType.Linear:
                    return new LinearInterpolation(KnownNodes);
                case InterpolationType.Spline:
                    return new SplineInterpolation(KnownNodes);
                default:
                    throw new NotImplementedException($"Interpolation type {interpolationType} not implemented!");
            }
        }
    }
}
