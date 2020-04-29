using CurveCalculations.Modifiers.Transformer.CurveTypeTransformer;
using CurveCalculations.Modifiers.Transformers;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer.CompoundingFrequencyTransform
{
    public abstract class AbstractCompTypeTransformer : AbstractTransformer
    {
        protected abstract CompoundingType _targetCompoundingType { get; }
        private MaturityType? TargetCompoundingFrequency { get; }

        public AbstractCompTypeTransformer(MaturityType? compoundingFrequency = null)
        {
            TargetCompoundingFrequency = compoundingFrequency;
        }


        public override void Transform(Curve curve)
        {
            if (curve.CompoundingType == _targetCompoundingType && curve.CompoundingFrequency == TargetCompoundingFrequency)
            {
                return;
            }
            TransformCurve(curve);
            curve.CompoundingType = _targetCompoundingType;
        }

        protected override void TransformFromForwardContinuous(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromZeroContinuous(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromParContinuous(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromDiscountContinuous(Curve curve) => TransformToDiscountThenDesired(curve);

        protected override void TransformFromForwardKTimesPerYear(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromZeroKTimesPerYear(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromParKTimesPerYear(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromDiscountKTimesPerYear(Curve curve) => TransformToDiscountThenDesired(curve);

        protected override void TransformFromForwardSimple(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromZeroSimple(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromParSimple(Curve curve) => TransformToDiscountThenDesired(curve);
        protected override void TransformFromDiscountSimple(Curve curve) => TransformToDiscountThenDesired(curve);


        private void TransformToDiscountThenDesired(Curve curve)
        {
            var transformerFactory = new CurveTypeTransformerFactory();
            var originalCurveTransformer = transformerFactory.GetTransformer(curve.CurveType);
            var discountCurveTransformer = transformerFactory.GetTransformer(CurveType.Discount);
            discountCurveTransformer.Transform(curve);

            //Note: Discount factors are not dependent on CompoundingType/Frequency so this can be performed safely
            curve.CompoundingType = _targetCompoundingType;
            curve.CompoundingFrequency = TargetCompoundingFrequency;

            originalCurveTransformer.Transform(curve);
        }
    }
}
