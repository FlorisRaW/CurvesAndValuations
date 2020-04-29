using CurveCalculations.Modifiers.Extrapolation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveCalculations.Methods.Extrapolation
{
    public class ExtrapolationFactory
    {
        private IDictionary<double, double> KnownNodes { get; set; }

        public ExtrapolationFactory(IDictionary<double, double> knownNodes)
        {
            KnownNodes = knownNodes;
        }

        public IExtrapolation GetExtrapolator(ExtrapolationType extrapolationType)
        {
            switch (extrapolationType)
            {
                case ExtrapolationType.Constant:
                    return new ConstantExtrapolation(KnownNodes.LastOrDefault());
                default:
                    throw new NotImplementedException($"Interpolation type {extrapolationType} not implemented!");
            }
        }
    }
}