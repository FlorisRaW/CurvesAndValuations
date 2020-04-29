using CurveCalculations.Methods.Extrapolation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static CurveCalculations.Utilities.Miscellaneous;

namespace CurveCalculations.Modifiers.Extrapolation
{
    public class ExtrapolationModifier
    {
        private IExtrapolation Extrapolator { get; }
        public ExtrapolationModifier(ExtrapolationType extrapolationType, IDictionary<double, double> knownNodes)
        {
            var factory = new ExtrapolationFactory(knownNodes);
            Extrapolator = factory.GetExtrapolator(extrapolationType);
        }

        public void Extrapolate(Curve curve, double maxMaturity)
        {
            var maturityType = curve.MaturityType;
            var knownTermStructure = curve.TermsAndRates;
            var maxKnownMaturity = knownTermStructure.Keys.Max();
            var desiredTerms = CreateTermArray(maturityType, maxMaturity, maxKnownMaturity, true);
            var extrapolatedTermStructure = Extrapolate(knownTermStructure, desiredTerms);
            curve.SetTermsAndRates(extrapolatedTermStructure, maturityType);
        }

        public IDictionary<double,double> Extrapolate(IDictionary<double,double> knownTermsAndRates, double[] desiredNewTerms)
        {
            var termStructure = new Dictionary<double, double>(knownTermsAndRates);
            foreach (var term in desiredNewTerms)
            {
                termStructure.Add(term, Extrapolator.GetValue(term));
            }
            return termStructure;
        }
    }
}
