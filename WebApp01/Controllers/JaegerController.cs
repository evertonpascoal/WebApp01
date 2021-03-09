using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OpenTracing;

namespace SampleA.Controllers
{
    [Route("api/Jaeger")]
    [ApiController]
    public class JaegerController : ControllerBase
    {
        private readonly ITracer _tracer;
        private readonly IHttpClientFactory _httpClientFactory;

        public JaegerController(
            ITracer tracer,
            IHttpClientFactory httpClientFactory
            )
        {
            _tracer = tracer;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Metodo criado para testar o OpenTracing
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            using var client = _httpClientFactory.CreateClient("WebJaegerTest");

            using (_tracer.BuildSpan("waitingForValues").StartActive(finishSpanOnDispose: true))
            {
                var retornoDoTestJaeger = await client.GetAsync("/api/testjaeger");

                return Ok(retornoDoTestJaeger.Content);
            }
        }
    }
}