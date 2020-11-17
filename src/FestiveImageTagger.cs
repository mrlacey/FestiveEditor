using System.Text.RegularExpressions;
using Microsoft.VisualStudio.Text;

namespace FestiveEditor
{
    internal sealed class FestiveImageTagger : RegexTagger<FestiveImageTag>
    {
        internal FestiveImageTagger(ITextBuffer buffer)
            : base(
                    buffer,
                    new[] { new Regex(@"\W(\w{3,7})\W", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase) })
        {
        }

        protected override FestiveImageTag TryCreateTagForMatch(Match match)
        {
            if (match.Groups.Count == 2)
            {
                var term = match.Groups[1].Value;

                return new FestiveImageTag(term);
            }

            return null;
        }
    }
}
