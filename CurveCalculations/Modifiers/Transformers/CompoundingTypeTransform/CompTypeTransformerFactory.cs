using CurveCalculations.Modifiers.Transformer.CompoundingFrequencyTransform;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer.CompoundingTypeTransform
{
    public class CompTypeTransformerFactory
    {
        public AbstractCompTypeTransformer GetTransformer(CompoundingType compoundingType, MaturityType? compoundingFrequency = null)
        {
            switch (compoundingType)
            {
                case CompoundingType.Continuous:
                    return new ContinuousCompTypeTransformer();
                case CompoundingType.KTimesPerYear:
                    return new KTimesPerYearCompTypeTransformer(compoundingFrequency.Value);
                case CompoundingType.Simple:
                    return new SimpleCompTypeTransformer();
                default:
                    throw new NotImplementedException($"Cannot get  transformer, unknown CompoundingType: {compoundingType}"!);
            }
        }
    }
}
