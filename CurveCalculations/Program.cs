using CurveCalculations.Domain;
using CurveCalculations.Modifiers.Extrapolation;
using CurveCalculations.Modifiers.Interpolation;
using CurveCalculations.Modifiers.Transformer.CurveTypeTransformer;
using CurveCalculations.Utilities;
using System;
using System.Collections.Generic;
using static CurveCalculations.Parsers.CurveParser;

namespace CurveCalculations
{
    class Program
    {
        static void Main()
        {
            var curveInfoFilePath = @"C:\Users\flori\OneDrive\Documents\Learning\C# Curves\Inputs\TestCurveInformation.csv";
            var ratesFilePath = @"C:\Users\flori\OneDrive\Documents\Learning\C# Curves\Inputs\TestRates.csv";
            var outputPathCurves = @"C:\Users\flori\OneDrive\Documents\Learning\C# Curves\Outputs\TransformedCurves.csv";

            var curves = ParseCurves(curveInfoFilePath, ratesFilePath);

            var adjustedCurves = new Dictionary<string, Curve>();
            foreach (var curveWithName in curves)
            {
                var name = curveWithName.Key;
                var curve = curveWithName.Value;
                adjustedCurves.Add($"{name}_1base", curve);

                //interpolated curve example
                var interpolatedCurve = curve.Copy();
                var interpolator = new InterpolationModifier(InterpolationType.Spline, curve.TermsAndRates);
                interpolator.Interpolate(interpolatedCurve, MaturityType.Quarterly);
                adjustedCurves.Add($"{name}_2interpolated", interpolatedCurve);

                //extrapolated curve example
                var extrapolatedCurve = interpolatedCurve.Copy();
                var extrapolator = new ExtrapolationModifier(ExtrapolationType.Constant, interpolatedCurve.TermsAndRates);
                extrapolator.Extrapolate(extrapolatedCurve, 100);
                adjustedCurves.Add($"{name}_3extrapolated", extrapolatedCurve);

                //transformed curve example forward
                var transformedCurveDisc = extrapolatedCurve.Copy();
                var curveTypeTransformerDisc = new CurveTypeTransformerFactory().GetTransformer(CurveType.Discount);
                curveTypeTransformerDisc.Transform(transformedCurveDisc);
                adjustedCurves.Add($"{name}_4transformedDisc", transformedCurveDisc);

                //transformed curve example forward
                var transformedCurveFwd = transformedCurveDisc.Copy();
                var curveTypeTransformerFwd = new CurveTypeTransformerFactory().GetTransformer(CurveType.Forward);
                curveTypeTransformerFwd.Transform(transformedCurveFwd);
                adjustedCurves.Add($"{name}_5transformedFwd", transformedCurveFwd);
            }
            WriteRatesToCsv(adjustedCurves, outputPathCurves);
        }
        private static void WriteRatesToCsv(IDictionary<string,Curve> curves, string outputPath)
        {
            var dataToExport = new List<RatesData>();
            foreach (var curve in curves)
            {
                var name = curve.Key;
                var termStructure = curve.Value.TermsAndRates;
                foreach (var termAndRate in termStructure)
                {
                    dataToExport.Add(new RatesData(name, termAndRate.Key, termAndRate.Value));
                }
            }
            new CsvWriter<RatesData>().Write(dataToExport, outputPath);
        }
    }
}