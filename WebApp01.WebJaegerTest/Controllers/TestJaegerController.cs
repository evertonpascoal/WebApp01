using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenTracing;
using OpenTracing.Propagation;
using OpenTracing.Tag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp01.WebJaegerTest.Controllers
{
    [Route("api/testjaeger")]
    [ApiController]
    public class TestJaegerController : ControllerBase
    {
        private readonly ITracer _tracer;

        public TestJaegerController(ITracer tracer)
        {
            _tracer = tracer;

        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var headers = Request.Headers.ToDictionary(k => k.Key, v => v.Value.First());
            using (var scope = StartServerSpan(_tracer, headers, "producingValues"))
            {
                Thread.Sleep(2000);
                return new[] { "Hello", "OpenTracing!" };
            }
        }

        public static IScope StartServerSpan(ITracer tracer, IDictionary<string, string> headers, string operationName)
        {
            ISpanBuilder spanBuilder;
            try
            {
                var parentSpanCtx = tracer.Extract(BuiltinFormats.HttpHeaders, new TextMapExtractAdapter(headers));

                spanBuilder = tracer.BuildSpan(operationName);
                if (parentSpanCtx != null)
                {
                    spanBuilder = spanBuilder.AsChildOf(parentSpanCtx);
                }
            }
            catch (Exception)
            {
                spanBuilder = tracer.BuildSpan(operationName);
            }

            // TODO could add more tags like http.url
            return spanBuilder.WithTag(Tags.SpanKind, Tags.SpanKindServer).StartActive(true);
        }
    }

}
