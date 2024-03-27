using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Yarp.Gateways.Infrastructure.Extensions;

public static class Extensions
{
    public static bool WildCardContains(this IEnumerable<string> data, string code)
    {
        return data.Any(item => Regex.IsMatch(code.ToLower(),
            Regex.Escape(item.ToLower()).Replace(@"\*", ".*").Replace(@"\?", ".")));
    }

    internal static RequestAppInfo? RequestAppInfo(this PathString path)
    {
        Regex reg = new Regex(@"^/app/(?<AppId>[^/]+)(?<ApiUrl>/api/.*)");

        var match = reg.Match(path);

        return match.Success
            ? new RequestAppInfo(match.Groups["AppId"].Value,
                $"{match.Groups["AppId"].Value}{Regex.Replace(match.Groups["ApiUrl"].Value, "/", ".")}")
            : default;
    }
}

internal record RequestAppInfo(string AppId,string AppCode);