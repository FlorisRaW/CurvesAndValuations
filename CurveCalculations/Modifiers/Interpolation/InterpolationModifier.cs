using CurveCalculations.Methods.Interpolation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static CurveCalculations.Utilities.Miscellaneous;

namespace CurveCalculations.Modifiers.Interpolation
{
    public class InterpolationModifier
    {
        private IInterpolation Interpolator { get; }
        public InterpolationModifier(InterpolationType interpolationType, IDictionary<double, double> knownNodes)
        {
            var factory = new InterpolationFactory(knownNodes);
            Interpolator = factory.GetInterpolator(interpolationType);
        }

        public void Interpolate(Curve curve, MaturityType maturityType)
        {
            if (curve.MaturityType == maturityType)
            {
                return;
            }
            var desiredTerms = CreateTermArray(maturityType, curve.Terms.Max());
            var interpolatedTermStructure = Interpolate(desiredTerms);
            curve.SetTermsAndRates(interpolatedTermStructure, maturityType);
        }

        public IDictionary<double, double> Interpolate(double[] desiredTerms)
        {
            var interpolatedTermStructure = new SortedList<double, double>();
            foreach (var term in desiredTerms)
            {
                interpolatedTermStructure.Add(term, Interpolator.GetValue(term));
            }
            return interpolatedTermStructure;
        }
    }
}
