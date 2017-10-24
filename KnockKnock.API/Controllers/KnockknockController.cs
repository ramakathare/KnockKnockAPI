using KnockKnock.Services;
using Microsoft.ApplicationInsights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace KnockKnock.API.Controllers
{
    [RoutePrefix("api")]
    public class KnockknockController : ApiController
    {
        protected Guid token = new Guid("e0206d90-de1e-45aa-ba93-296b6f61b159");

        protected TelemetryClient telemetry = new TelemetryClient();

        /// <summary>
        /// Returns the nth number in the fibonacci sequence.
        /// </summary>
        /// <param name="n">The index (n) of the fibonacci sequence</param>
        /// <returns>The number at n position in the Fibonacci sequence.</returns>
        [Route("Fibonacci")]
        public long GetFibonacciNumber(long n)
        {
            var properties = new Dictionary<string, string> { { "Argument 'n'", n.ToString() } };
            telemetry.TrackEvent("FibonacciNumber", properties);

            var result = new FibonacciNumberService().Calculate(n);
            return result;
        }

        /// <summary>
        /// Your token.
        /// </summary>
        /// <returns>The Readify token.</returns>
        [Route("Token")]
        public Guid GetToken()
        {
            var properties = new Dictionary<string, string> { { "Token", this.token.ToString() } };
            telemetry.TrackEvent("WhatIsYourToken", properties);

            return this.token;
        }

        /// <summary>
        /// Reverses the letters of each word in a sentence.
        /// </summary>
        /// <param name="sentence">Sentence</param>
        /// <returns>Reversed sentence.</returns>
        [Route("ReverseWords")]
        public string GetReverseWords(string sentence)
        {
            var properties = new Dictionary<string, string> { { "Argument 's'", string.Format("'{0}'", sentence == null ? "null" : sentence) } };
            telemetry.TrackEvent("ReverseWords", properties);

            var result = new StringReverseService().ReverseWords(sentence);
            return result;
        }

        /// <summary>
        /// Returns the type of triange given the lengths of its sides.
        /// </summary>
        /// <param name="a">Length of side 'a'.</param>
        /// <param name="b">Length of side 'b'.</param>
        /// <param name="c">Length of side 'c'.</param>
        /// <returns>The <see cref="TriangleType"/> type.</returns>
        [Route("TriangleType")]
        public string GetWhatShapeIsThis(int a, int b, int c)
        {
            var properties = new Dictionary<string, string> { { "Argument 'a'", a.ToString() }, { "Argument 'b'", b.ToString() }, { "Argument 'c'", c.ToString() } };
            telemetry.TrackEvent("WhatShapeIsThis", properties);

            TriangleType result = TriangleType.Error;
            result = new ShapeService().ClassifyTriangleType(a, b, c);
            return result.ToString();
        }
    }
}
