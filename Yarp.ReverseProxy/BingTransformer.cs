using System.IO.Compression;
using System.Text;
using Yarp.ReverseProxy.Forwarder;

namespace Yarp.Gateways
{
    public class BingTransformer : HttpTransformer
    {
        public override async ValueTask TransformRequestAsync(HttpContext httpContext, HttpRequestMessage proxyRequest,
            string destinationPrefix,
            CancellationToken cancellationToken)
        {
            var path = httpContext.Request.Path.Value?.Split('/').Skip(2).Aggregate((a, b) => $"{a}/{b}");

            var uri = RequestUtilities.MakeDestinationAddress(destinationPrefix, $"/{path}",
                httpContext.Request.QueryString);
            proxyRequest.RequestUri = uri;

            proxyRequest.Headers.Host = uri.Host;
            await base.TransformRequestAsync(httpContext, proxyRequest, destinationPrefix, cancellationToken);
        }

        public override async ValueTask<bool> TransformResponseAsync(HttpContext httpContext,
            HttpResponseMessage? proxyResponse,
            CancellationToken cancellationToken)
        {
            await base.TransformResponseAsync(httpContext, proxyResponse, cancellationToken);

            if (httpContext.Request.Method == "GET" &&
                httpContext.Response.Headers["Content-Type"].Any(x => x.StartsWith("text/html")))
            {
                var encoding = proxyResponse.Content.Headers.FirstOrDefault(x => x.Key == "Content-Encoding").Value;
                if (encoding?.FirstOrDefault() == "gzip")
                {
                    var content = proxyResponse?.Content.ReadAsByteArrayAsync(cancellationToken).Result;
                    if (content != null)
                    {
                        var result = Encoding.UTF8.GetString(GZipDecompressByte(content));
                        result = result.Replace("国内版", "Token Bing 搜索 - 国内版");
                        proxyResponse.Content = new StringContent(GZipDecompressString(result));
                    }
                }
                else if (encoding.FirstOrDefault() == "br")
                {
                    var content = proxyResponse?.Content.ReadAsByteArrayAsync(cancellationToken).Result;
                    if (content != null)
                    {
                        var result = Encoding.UTF8.GetString(BrDecompress(content));
                        result = result.Replace("国内版", "Token Bing 搜索 - 国内版");
                        proxyResponse.Content = new ByteArrayContent(BrCompress(result));
                    }
                }
                else
                {
                    var content = proxyResponse?.Content.ReadAsStringAsync(cancellationToken).Result;
                    if (content != null)
                    {
                        content = content.Replace("国内版", "Token Bing 搜索 - 国内版");
                        proxyResponse.Content = new StringContent(content);
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 解压GZip
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] GZipDecompressByte(byte[] bytes)
        {
            using var targetStream = new MemoryStream();
            using var compressStream = new MemoryStream(bytes);
            using var zipStream = new GZipStream(compressStream, CompressionMode.Decompress);
            using (var decompressionStream = new GZipStream(compressStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(targetStream);
            }

            return targetStream.ToArray();
        }

        /// <summary>
        /// 解压GZip
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GZipDecompressString(string str)
        {
            using var compressStream = new MemoryStream(Encoding.UTF8.GetBytes(str));
            using var zipStream = new GZipStream(compressStream, CompressionMode.Decompress);
            using var resultStream = new StreamReader(new MemoryStream(compressStream.ToArray()));
            return resultStream.ReadToEnd();
        }

        /// <summary>
        /// Br压缩
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] BrCompress(string str)
        {
            using var outputStream = new MemoryStream();
            using (var compressionStream = new BrotliStream(outputStream, CompressionMode.Compress))
            {
                compressionStream.Write(Encoding.UTF8.GetBytes(str));
            }

            return outputStream.ToArray();
        }

        /// <summary>
        /// Br解压
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] BrDecompress(byte[] input)
        {
            using (var inputStream = new MemoryStream(input))
            using (var outputStream = new MemoryStream())
            using (var decompressionStream = new BrotliStream(inputStream, CompressionMode.Decompress))
            {
                decompressionStream.CopyTo(outputStream);
                return outputStream.ToArray();
            }
        }
    }
}
