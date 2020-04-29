using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer.CurveTypeTransformer
{
    public class ForwardCurveTypeTransformer : AbstractCurveTypeTransformer
    {
        protected override CurveType _targetCurveType => CurveType.Forward;

        protected override void TransformFromDiscountContinuous(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var newRates = new double[rates.Length];

            newRates[0] = 0;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = 1 / (terms[i] - terms[i-1]) * (Math.Log(rates[i - 1]) - Math.Log(rates[i]));
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromDiscountKTimesPerYear(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var newRates = new double[rates.Length];

            newRates[0] = 0;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = Math.Pow((rates[i - 1] / rates[i]), 1 / (terms[i] - terms[i - 1])) - 1;
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromDiscountSimple(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var newRates = new double[rates.Length];

            newRates[0] = 0;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = 1 / (terms[i] - terms[i - 1]) * ((rates[i - 1] / rates[i]) - 1);
            }
            curve.Rates = newRates;
        }
    }
}
