using CurveCalculations.Modifiers.Transformer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Modifiers.Transformers
{
    public abstract class AbstractTransformer : ITransformer
    {
        public abstract void Transform(Curve curve);
        protected void TransformCurve(Curve curve)
        {
            switch (curve.CompoundingType)
            {
                case CompoundingType.Continuous:
                    switch (curve.CurveType)
                    {

                        case CurveType.Forward:
                            TransformFromForwardContinuous(curve);
                            break;
                        case CurveType.Zero:
                            TransformFromZeroContinuous(curve);
                            break;
                        case CurveType.Par:
                            TransformFromParContinuous(curve);
                            break;
                        case CurveType.Discount:
                            TransformFromDiscountContinuous(curve);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case CompoundingType.KTimesPerYear:
                    switch (curve.CurveType)
                    {

                        case CurveType.Forward:
                            TransformFromForwardKTimesPerYear(curve);
                            break;
                        case CurveType.Zero:
                            TransformFromZeroKTimesPerYear(curve);
                            break;
                        case CurveType.Par:
                            TransformFromParKTimesPerYear(curve);
                            break;
                        case CurveType.Discount:
                            TransformFromDiscountKTimesPerYear(curve);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                case CompoundingType.Simple:
                    switch (curve.CurveType)
                    {

                        case CurveType.Forward:
                            TransformFromForwardSimple(curve);
                            break;
                        case CurveType.Zero:
                            TransformFromZeroSimple(curve);
                            break;
                        case CurveType.Par:
                            TransformFromParSimple(curve);
                            break;
                        case CurveType.Discount:
                            TransformFromDiscountSimple(curve);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        protected abstract void TransformFromForwardContinuous(Curve curve);
        protected abstract void TransformFromZeroContinuous(Curve curve);
        protected abstract void TransformFromParContinuous(Curve curve);
        protected abstract void TransformFromDiscountContinuous(Curve curve);

        protected abstract void TransformFromForwardKTimesPerYear(Curve curve);
        protected abstract void TransformFromZeroKTimesPerYear(Curve curve);
        protected abstract void TransformFromParKTimesPerYear(Curve curve);
        protected abstract void TransformFromDiscountKTimesPerYear(Curve curve);

        protected abstract void TransformFromForwardSimple(Curve curve);
        protected abstract void TransformFromZeroSimple(Curve curve);
        protected abstract void TransformFromParSimple(Curve curve);
        protected abstract void TransformFromDiscountSimple(Curve curve);

    }
}
