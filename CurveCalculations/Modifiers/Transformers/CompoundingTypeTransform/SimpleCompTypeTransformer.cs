using CurveCalculations.Modifiers.Transformer.CompoundingFrequencyTransform;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer.CompoundingTypeTransform
{
    public class SimpleCompTypeTransformer : AbstractCompTypeTransformer
    {
        protected override CompoundingType _targetCompoundingType => CompoundingType.Simple;
    }
}
