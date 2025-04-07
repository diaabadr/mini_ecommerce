using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Data.Common;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

namespace Persistence
{
    public class PrometheusDbInterceptor : DbCommandInterceptor
    {
        public override async ValueTask<InterceptionResult<DbDataReader>> ReaderExecutingAsync(
            DbCommand command,
            CommandEventData eventData,
            InterceptionResult<DbDataReader> result,
            CancellationToken cancellationToken = default)
        {
            var stopwatch = Stopwatch.StartNew();
            var res = await base.ReaderExecutingAsync(command, eventData, result, cancellationToken);
            stopwatch.Stop();

            var queryType = command.CommandText.TrimStart().Split(' ')[0].ToUpper();
            var querySource = eventData.CommandSource.ToString() ?? "unknown";
            var sql = command.CommandText.Trim();
            var hash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes(sql)));


            var stackTrace = new StackTrace(true);
            var frames = stackTrace.GetFrames();

            var callingFrame = frames?.FirstOrDefault(f =>
            f.GetFileName() != null &&
            !f.GetFileName().Contains("Microsoft.EntityFrameworkCore") &&
            !f.GetFileName().Contains("System.") &&
            !f.GetFileName().Contains("Prometheus")
        );

            var file = Path.GetFileName(callingFrame?.GetFileName() ?? "unknown");
            var line = callingFrame?.GetFileLineNumber().ToString() ?? "0";

            var location = $"{file}:{line}";

            CustomMetrics.DbQueryDuration
                .Labels(queryType, querySource, location)
                .Observe(stopwatch.Elapsed.TotalSeconds);

            return res;
        }
    }
}