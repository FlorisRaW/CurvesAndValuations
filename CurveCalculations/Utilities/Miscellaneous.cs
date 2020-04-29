using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Utilities
{
    public static class Miscellaneous
    {
        public static decimal[] CreateArrayConstantIncrement(decimal maxValue, decimal increment, decimal minValue = 0)
        {
            var terms = new decimal[(int)Math.Floor((maxValue - minValue) / increment) + 1];
            for (var i = 0; i < terms.Length; i++)
            {
                terms[i] = minValue + i * increment;
            }
            return terms;
        }

        public static double[] CreateTermArray(MaturityType maturityType, double maxTerm, double minTerm = 0, bool skipMinTerm=false)
        {
            decimal increment;
            switch (maturityType)
            {
                case MaturityType.Annually:
                    increment = 1;
                    break;
                case MaturityType.SemiAnnually:
                    increment = decimal.Divide(1, 2);
                    break;
                case MaturityType.Quarterly:
                    increment = decimal.Divide(1, 4);
                    break;
                case MaturityType.Monthly:
                    increment = decimal.Divide(1, 12);
                    break;
                case MaturityType.Custom:
                default:
                    throw new NotImplementedException($"Cannot determine predefined terms for a curve of maturity type {maturityType}");
            }
            var minTermDecimal = (decimal)minTerm + (skipMinTerm ? increment : 0);
            var maxTermDecimal = (decimal)maxTerm;


            return Array.ConvertAll(CreateArrayConstantIncrement(maxTermDecimal, increment, minTermDecimal), x => (double)x);
        }
    }
}
