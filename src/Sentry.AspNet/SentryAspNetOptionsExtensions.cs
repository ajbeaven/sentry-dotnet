using System.Web;
using Sentry.AspNet.Internal;
using Sentry.Extensibility;
using Sentry.Infrastructure;

namespace Sentry.AspNet
{
    /// <summary>
    /// SentryOptions extensions.
    /// </summary>
    public static class SentryAspNetOptionsExtensions
    {
        /// <summary>
        /// Adds ASP.NET classic integration.
        /// </summary>
        public static void AddAspNet(this SentryOptions options, RequestSize maxRequestBodySize = RequestSize.None)
        {
            var payloadExtractor = new RequestBodyExtractionDispatcher(
                new IRequestPayloadExtractor[] {new FormRequestPayloadExtractor(), new DefaultRequestPayloadExtractor()},
                options,
                () => maxRequestBodySize
            );

            var eventProcessor = new SystemWebRequestEventProcessor(payloadExtractor, options);

            options.DiagnosticLogger ??= new DebugDiagnosticLogger(options.DiagnosticLevel);
            options.Release ??= SystemWebVersionLocator.Resolve(options, HttpContext.Current);
            options.AddEventProcessor(eventProcessor);
        }
    }
}
