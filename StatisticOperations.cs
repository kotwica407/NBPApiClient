using System;
using System.Collections.Generic;
using MathNet.Numerics.Statistics;
using System.Linq;

namespace NBPApiClient
{
    internal static class StatisticOperations
    {
        internal static double CorrelationCoefficient(List<Decimal> l1, List<Decimal> l2)
        {
            return Correlation
                .Pearson(l1.Select(x => (double)x), l2.Select(x => (double)x));
        }
    }
}