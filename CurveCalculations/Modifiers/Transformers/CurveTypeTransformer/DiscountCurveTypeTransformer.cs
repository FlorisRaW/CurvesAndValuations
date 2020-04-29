using CurveCalculations.Methods.Interpolation;
using CurveCalculations.Modifiers.Interpolation;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using static CurveCalculations.Utilities.Miscellaneous;

namespace CurveCalculations.Modifiers.Transformer.CurveTypeTransformer
{
    public class DiscountCurveTypeTransformer : AbstractCurveTypeTransformer
    {
        protected override CurveType _targetCurveType => CurveType.Discount;
        protected override void TransformFromForwardContinuous(Curve curve)
        {
            var rates = curve.Rates;
            var newRates = new double[rates.Length];

            newRates[0] = 1;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = newRates[i - 1] * Math.Exp(-rates[i]);
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromZeroContinuous(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var newRates = new double[rates.Length];

            newRates[0] = 1;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = Math.Exp(-rates[i] * terms[i]);
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromParContinuous(Curve curve)
        {
            throw new NotImplementedException("Transformation from continuously compounding par curve to discount not implemented.");
        }

        protected override void TransformFromDiscountContinuous(Curve curve)
        {
            //do nothing
        }

        protected override void TransformFromForwardKTimesPerYear(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var freq = (int)curve.CompoundingFrequency.Value;
            var newRates = new double[rates.Length];

            newRates[0] = 1;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = newRates[i - 1] / Math.Pow(1 + (rates[i] / freq), freq / 12 / (terms[i] - terms[i-1]));
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromZeroKTimesPerYear(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var freq = (int)curve.CompoundingFrequency.Value;
            var newRates = new double[rates.Length];

            newRates[0] = 1;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = 1 / Math.Pow(1 + rates[i] / freq, terms[i] * freq);
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromParKTimesPerYear(Curve curve)
        {
            var freq = (int)curve.CompoundingFrequency.Value;
            var freqYearFrac = decimal.Divide(1, freq);
            var relevantParCurve = GetParCurveForTransformationToDiscount(curve);
            var relevantDiscountRates = new Dictionary<double, double>();
            foreach (var parTermAndRate in relevantParCurve.TermsAndRates)
            {
                relevantDiscountRates.Add(parTermAndRate.Key, CalculateDiscountFromPar(parTermAndRate.Value, freq, relevantDiscountRates.Values.Sum()));
            }

            var terms = curve.Terms;
            var rates = curve.Rates;
            var newRates = new double[rates.Length];

            for (var i = 0; i < terms.Length; i++)
            {
                var term = terms[i];
                var rate = rates[i];
                newRates[i] = relevantDiscountRates.ContainsKey(term)
                    ? relevantDiscountRates[term]
                    : CalculateDiscountFromPar(rate, freq
                        , relevantDiscountRates.Where(x => x.Key < term).Sum(x => x.Value)
                        , (double)((decimal)term % freqYearFrac / freqYearFrac));
            }
            curve.Rates = newRates;
        }

        //To transform par to discount, get the par rates needed 
        private static Curve GetParCurveForTransformationToDiscount(Curve curve)
        {
            var inputTermsAndRates = curve.TermsAndRates;
            var freqTimesteps = decimal.Divide(1, (decimal)curve.CompoundingFrequency.Value); ;

            var requiredTerms = Array.ConvertAll(CreateArrayConstantIncrement((decimal)inputTermsAndRates.Keys.Max(), freqTimesteps, freqTimesteps), x => (double)x);
            if (requiredTerms.Any(x => !inputTermsAndRates.ContainsKey(x)))
            {
                Log.Info("Par rates have not been given for every cashflow frequency. Linear interpolation will be done on the par rates for missing rates! If you would like to use a different interpolation method, interpolate before calling this method.");
            }

            var interpolator = new InterpolationModifier(InterpolationType.Linear, inputTermsAndRates);
            var interpolatedTermsAndRates = interpolator.Interpolate(requiredTerms.ToArray());
            var interpolatedParCurve = curve.Copy();
            interpolatedParCurve.SetTermsAndRates(interpolatedTermsAndRates);
            return interpolatedParCurve;
        }

        //TODO: is the fractionLastPaymentPeriod the correct solution?
        private static double CalculateDiscountFromPar(double parRate, double paymentFrequency, double sumPrevDfs, double fractionLastPaymentPeriod=1)
        {
            return (1 - (parRate / paymentFrequency * sumPrevDfs)) / (1 + (parRate / paymentFrequency * fractionLastPaymentPeriod));
        }


        protected override void TransformFromDiscountKTimesPerYear(Curve curve)
        {
            //do nothing
        }

        protected override void TransformFromForwardSimple(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var newRates = new double[rates.Length];

            newRates[0] = 1;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = newRates[i - 1] / (1 + (rates[i] * (terms[i] - terms[i - 1])));
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromZeroSimple(Curve curve)
        {
            var rates = curve.Rates;
            var terms = curve.Terms;
            var newRates = new double[rates.Length];

            newRates[0] = 1;
            for (var i = 1; i < rates.Length; i++)
            {
                newRates[i] = 1 / (1 + rates[i] * terms[i]);
            }
            curve.Rates = newRates;
        }

        protected override void TransformFromParSimple(Curve curve)
        {
            throw new NotImplementedException("Transformation from simple compounding par curve to discount not implemented.");
        }

        protected override void TransformFromDiscountSimple(Curve curve)
        {
            //do nothing
        }
    }
}
