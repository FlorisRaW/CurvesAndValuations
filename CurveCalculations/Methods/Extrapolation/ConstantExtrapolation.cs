using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Text;

namespace CurveCalculations.Methods.Extrapolation
{
    public class ConstantExtrapolation : IExtrapolation
    {
        private KeyValuePair<double, double> FinalNode { get; set; }

        public ConstantExtrapolation(KeyValuePair<double, double> finalNode)
        {
            FinalNode = finalNode;
        }

        public double GetValue(double key)
        {
            if (key < FinalNode.Key)
            {
                throw new InvalidDataException($"Cannot perform constant extrapolation, the desired value {key} is smaller than the known node provided for extrapolation!");
            }
            return FinalNode.Value;
        }
    }
}
