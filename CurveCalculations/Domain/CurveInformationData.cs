using CurveCalculations.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Domain
{
    public class CurveInformationData : CsvableBase
    {
        public string CurveName { get; set; }
        public string CurveType { get; set; }
        public string CompoundingType { get; set; }
        public double CompoundingFrequency { get; set; }

        public CurveInformationData() { }

        public CurveInformationData(string curveName, string compoundingType, double compoundingFrequency)
        {
            CurveName = curveName;
            CompoundingType = compoundingType;
            CompoundingFrequency = compoundingFrequency;
        }
    }
}
