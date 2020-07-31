using System;
using System.Collections.Generic;
using MathNet.Numerics.Statistics;
using System.Linq;

namespace NBPApiClient
{
    internal static class StatisticOperations
    {
        internal static double GetCorrelationCoefficient(IEnumerable<decimal> l1, IEnumerable<decimal> l2)
        {
            return Correlation
               .Pearson(l1.Select(x => (double) x), l2.Select(x => (double) x));
        }

        internal static double[] GetAutoCorrelation(double[] series)
        {
            return Correlation.Auto(series);
        }

        internal static double[] GetNormalizedSeries(double[] series)
        {
            double min = series.Min();
            double max = series.Max();
            return series.Select(x => (x - min) / (max - min)).ToArray();
        }
    }
}