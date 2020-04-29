using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer.CurveTypeTransformer
{
    public class ZeroCurveTypeTransformer : AbstractCurveTypeTransformer
    {
        protected override CurveType _targetCurveType => CurveType.Zero;

        protected override void TransformFromDiscountContinuous(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;

            var newRates = new double[rates.Length];

            for (var i = 0; i < rates.Length; i++)
            {
                newRates[i] = -Math.Log(rates[i] / terms[i]);
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromDiscountKTimesPerYear(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var freq = (int)curve.CompoundingFrequency.Value;

            var newRates = new double[rates.Length];

            for (var i = 0; i < rates.Length; i++)
            {
                newRates[i] = freq / Math.Pow(rates[i], 1 / (freq * terms[i])) - freq;
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromDiscountSimple(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;

            var newRates = new double[rates.Length];

            for (var i = 0; i < rates.Length; i++)
            {
                newRates[i] = (1 / rates[i] - 1) / terms[i];
            }
            curve.Rates = newRates;
        }
    }
}
