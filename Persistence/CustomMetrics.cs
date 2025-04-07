using Prometheus;

namespace Persistence
{
    public static class CustomMetrics
    {
        public static readonly Histogram DbQueryDuration = Prometheus.Metrics
            .CreateHistogram("db_query_duration_seconds",
                "Time taken to execute EF Core DB queries.",
                new HistogramConfiguration
                {
                    LabelNames = new[] { "query_type", "query_source", "query_location" },
                    Buckets = Histogram.ExponentialBuckets(0.001, 2, 10)
                });
    }

}
