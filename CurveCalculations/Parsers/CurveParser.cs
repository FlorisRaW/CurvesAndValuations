using CurveCalculations.Domain;
using CurveCalculations.Utilities;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace CurveCalculations.Parsers
{
    public static class CurveParser
    {
        public static IDictionary<string, Curve> ParseCurves(string curveInfoFilePath, string ratesFilePath)
        {
            var curveInfo = new CsvReader<CurveInformationData>().Read(curveInfoFilePath, true);
            var ratesData = new CsvReader<RatesData>().Read(ratesFilePath, true);

            var groupedRates = ratesData.GroupBy(x => x.CurveName);

            var parsedCurves = new Dictionary<string, Curve>();
            foreach (var curve in groupedRates)
            {
                var name = curve.Key;
                var ratesRaw = curve.Select(x => x.Rate);
                var termsRaw = curve.Select(x => x.Term);
                SortedList<double, double> termsAndRates = new SortedList<double, double>(termsRaw.Zip(ratesRaw, (t, r) => new { t, r }).ToDictionary(item => item.t, item => item.r));
                
                var relevantCurveInfo = curveInfo.FirstOrDefault(x => x.CurveName == name);

                if (relevantCurveInfo == null)
                {
                    throw new InvalidDataException($"Cannot find curve information for curve {name} in {curveInfoFilePath}!");
                }

                if (!Enum.TryParse(relevantCurveInfo.CurveType, out CurveType curveType))
                {
                    throw new InvalidCastException($"Cannot parse {relevantCurveInfo.CurveType} to the CurveType Enum!");
                }
                if (!Enum.TryParse(relevantCurveInfo.CompoundingType, out CompoundingType compType))
                {
                    throw new InvalidCastException($"Cannot parse {relevantCurveInfo.CompoundingType} to the CompoundingType Enum!");
                }
                if (!Enum.IsDefined(typeof(CompoundingType),relevantCurveInfo.CompoundingFrequency))
                {
                    throw new InvalidCastException($"Cannot parse {relevantCurveInfo.CompoundingFrequency} to the CompoundingType Enum!");
                }
                var compFreq = (MaturityType)relevantCurveInfo.CompoundingFrequency;

                var parsedCurve = new Curve(curveType, compType, compFreq, MaturityType.Custom, termsAndRates.Keys.ToArray(), termsAndRates.Values.ToArray());
                parsedCurves.Add(name, parsedCurve);
            }
            return parsedCurves;
        }
    }
}
