using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer.CurveTypeTransformer
{
    public class CurveTypeTransformerFactory
    {
        public AbstractCurveTypeTransformer GetTransformer(CurveType desiredCurveType)
        {
            switch (desiredCurveType)
            {

                case CurveType.Forward:
                    return new ForwardCurveTypeTransformer();
                case CurveType.Zero:
                    return new ZeroCurveTypeTransformer();
                case CurveType.Par:
                    return new ParCurveTypeTransformer();
                case CurveType.Discount:
                    return new DiscountCurveTypeTransformer();
                default:
                    throw new NotImplementedException();
            }

        }
    }
}
