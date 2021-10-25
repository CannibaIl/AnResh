using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Anresh.Api
{
    public class Preprocessor
    {
        private const string Pattern = @"<!--\s*#include\s+""([^""]+)""\s*-->";
        private readonly Regex re = new Regex(Pattern, RegexOptions.Multiline);
        public string Process(string content)
        {
            var matches = re.Matches(content);
            for (var i = matches.Count - 1; i >= 0; i--)
            {
                var match = matches[i];
                var group = match.Groups[1];
                var filename = group.Value;
                var path = Path.Combine(@"wwwroot\pageComponents", filename);
                if (!File.Exists(path))
                {
                    continue;
                }

                var includeContent = File.ReadAllText(path, Encoding.UTF8);
                var left = content.Substring(0, match.Index);
                var right = content.Substring(match.Index + match.Length);
                content = left + includeContent + right;
            }

            return content;
        }
    }
}
