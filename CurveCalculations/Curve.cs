using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Xml.Schema;

namespace CurveCalculations
{
    public class Curve
    {
        public CurveType CurveType { get; set; }
        public CompoundingType CompoundingType { get; set; }
        private MaturityType? _compoundingFrequency;
        public MaturityType? CompoundingFrequency
        {
            get { return CompoundingType == CompoundingType.KTimesPerYear ? _compoundingFrequency : null; }
            set { _compoundingFrequency = value; }
        }
        public MaturityType MaturityType { get; private set; }
        public double[] Terms { get; private set; }
        private double[] _rates;
        public double[] Rates 
        { 
            get { return _rates; } 
            set
            {
                if (_rates.Length != value.Length)
                {
                    throw new InvalidOperationException($"Cannot set rates as it is not the same length as the current rates. If you want to adjust the term structure, use the SetTermsAndRates() function to set both the terms and rates.");
                }
                _rates = value;
            } 
        }
        public IDictionary<double, double> TermsAndRates
        {
            get { return Terms.Zip(Rates, (t, r) => new { t, r }).ToDictionary(item => item.t, item => item.r); }
        }

        public Curve(CurveType curveType, CompoundingType compoundingType, MaturityType? compoundingFrequency, MaturityType maturityType, double[] terms, double[] rates)
        {
            SetTermsAndRates(terms, rates, maturityType);
            CurveType = curveType;
            CompoundingType = compoundingType;
            CompoundingFrequency = compoundingFrequency;
        }

        public void SetTermsAndRates(double[] terms, double[] rates, MaturityType maturityType = MaturityType.Custom)
        {
            Terms = terms;
            MaturityType = maturityType;
            _rates = rates;
        }

        public void SetTermsAndRates(IDictionary<double, double> termsAndRates, MaturityType maturityType = MaturityType.Custom)
        {
            var terms = termsAndRates.Keys.ToArray();
            var rates = termsAndRates.Values.ToArray();
            SetTermsAndRates(terms, rates, maturityType);
        }

        public Curve Copy()
        {
            return new Curve(CurveType, CompoundingType, CompoundingFrequency, MaturityType, Terms, Rates);
        }

    }
}
