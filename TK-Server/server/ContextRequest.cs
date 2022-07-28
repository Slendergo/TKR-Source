using Anna.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace server
{
    internal partial class Program
    {
        public sealed class ContextRequest
        {
            public delegate KeyValuePair<string, RequestContext> AsyncContextRequest(
                RequestContext rContext,
                int maxContentLength);

            public KeyValuePair<string, RequestContext> HandleContext(
                RequestContext rContext,
                int maxContentLength)
            {
                var taskResult = new KeyValuePair<string, RequestContext>(null, null);

                try
                {
                    var r = rContext.Request;
                    var bufferSize = maxContentLength;
                    var headers = r.Headers
                        .ToDictionary(entry => entry.Key, entry => entry.Value);

                    if (headers.ContainsKey("Content-Length"))
                        bufferSize = Math.Min(maxContentLength, int.Parse(headers["Content-Length"].First()));

                    var buffer = new byte[bufferSize];
                    var bytesRead = r.InputStream.ReadAsync(buffer, 0, bufferSize).Result;
                    var result = r.ContentEncoding.GetString(buffer, 0, bytesRead);

                    taskResult = new KeyValuePair<string, RequestContext>(result, rContext);
                }
                catch (Exception e) { OnError(e, rContext); }

                return taskResult;
            }

            public void HandleContextCallback(IAsyncResult ar)
            {
                if (!ar.IsCompleted) return;

                var result = (AsyncResult)ar;
                var acr = (AsyncContextRequest)result.AsyncDelegate;
                var response = acr.EndInvoke(ar);
                var body = response.Key;
                var rContext = response.Value;

                if (body == null || rContext == null) return;

                try
                {
                    var request = rContext.Request.Url.LocalPath;

                    if (!RequestHandlers.Post.ContainsKey(request)) return;

                    var query = HttpUtility.ParseQueryString(body);

                    RequestHandlers.Post[rContext.Request.Url.LocalPath].HandleRequest(rContext, query);
                }
                catch (Exception e) { OnError(e, rContext); }
            }
        }
    }
}
