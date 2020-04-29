using CurveCalculations.Modifiers.Interpolation;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer.CurveTypeTransformer
{
    //NOTE: Transformation to par rates not implemented because this transformation is not regular and requires a solver. Can be implemented later if needed
    public class ParCurveTypeTransformer : AbstractCurveTypeTransformer
    {
        protected override CurveType _targetCurveType => CurveType.Par;

        protected override void TransformFromDiscountContinuous(Curve curve)
        {
            throw new NotImplementedException("Transformation from discount curve to continuously compounding par not implemented.");
        }

        protected override void TransformFromDiscountKTimesPerYear(Curve curve)
        {
            throw new NotImplementedException("Transformation from discount curve to ktimesperyear compounding par not implemented.");
        }

        protected override void TransformFromDiscountSimple(Curve curve)
        {
            throw new NotImplementedException("Transformation from discount curve to simple compounding par not implemented.");
        }
    }
}
