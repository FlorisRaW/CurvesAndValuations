using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

namespace CurveCalculations.Methods.Interpolation
{
    public class LinearInterpolation : IInterpolation
    {
        private readonly SortedDictionary<double, double> Nodes;
        public LinearInterpolation(IDictionary<double, double> nodes)
        {
            Nodes = new SortedDictionary<double, double>(nodes);
        }

        public double GetValue(double key)
        {
            if (Nodes.Keys.Min() > key)
            {
                throw new InvalidDataException($"Cannot perform linear interplation, the desired value {key} is smaller than the minimum value of the available nodes!");
            }
            if (Nodes.Keys.Max() < key)
            {
                throw new InvalidDataException($"Cannot perform linear interplation, the desired value {key} is larger than the maximum value of the available nodes!");
            }
            var before = Nodes.LastOrDefault(x => x.Key <= key);
            var after = Nodes.FirstOrDefault(x => x.Key >= key);

            return Linear(key, before.Key, after.Key, before.Value, after.Value);
        }

        private static double Linear(double x, double x0, double x1, double y0, double y1)
        {
            if ((x1 - x0) == 0)
            {
                return (y0 + y1) / 2;
            }
            return y0 + (x - x0) * (y1 - y0) / (x1 - x0);
        }
    }
}
