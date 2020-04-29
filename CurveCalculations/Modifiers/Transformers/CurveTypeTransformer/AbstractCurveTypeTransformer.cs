using CurveCalculations.Modifiers.Transformers;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer.CurveTypeTransformer
{
    public abstract class AbstractCurveTypeTransformer : AbstractTransformer
    {
        protected abstract CurveType _targetCurveType { get; }

        public override void Transform(Curve curve)
        {
            TransformCurve(curve);
            curve.CurveType = _targetCurveType;
        }

        protected override void TransformFromForwardContinuous(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromZeroContinuous(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromParContinuous(Curve curve) => TransformToDiscountThenDesired(curve);

        protected override void TransformFromForwardKTimesPerYear(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromZeroKTimesPerYear(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromParKTimesPerYear(Curve curve) => TransformToDiscountThenDesired(curve);

        protected override void TransformFromForwardSimple(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromZeroSimple(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromParSimple(Curve curve) => TransformToDiscountThenDesired(curve);


        private void TransformToDiscountThenDesired(Curve curve)
        {
            var discountCurveTransformer = new CurveTypeTransformerFactory().GetTransformer(CurveType.Discount);
            discountCurveTransformer.Transform(curve);
            Transform(curve);
        }
    }
}