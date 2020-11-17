using Microsoft.VisualStudio.Text.Tagging;

namespace FestiveEditor
{
    internal class FestiveImageTag : ITag
    {
        public FestiveImageTag(string term)
        {
            this.Term = term;
        }

        public string Term { get; }
    }
}
