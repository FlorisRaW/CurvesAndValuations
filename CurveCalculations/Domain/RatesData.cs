using CurveCalculations.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Domain
{
    public class RatesData : CsvableBase
    {
        public string CurveName { get; set; }
        public double Term { get; set; }
        public double Rate { get; set; }

        public RatesData() { }

        public RatesData(string curveName, double term, double rate)
        {
            CurveName = curveName;
            Term = term;
            Rate = rate;
        }
    }
}
