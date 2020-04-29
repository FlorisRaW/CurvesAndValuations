using System;
using System.Collections.Generic;
using System.Text;

namespace CurveCalculations.Modifiers.Transformer
{
    interface ITransformer
    {
        public abstract void Transform(Curve curve);
    }
}
