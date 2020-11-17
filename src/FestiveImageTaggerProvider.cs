using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace FestiveEditor
{
    [Export(typeof(ITaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(FestiveImageTag))]
    internal sealed class FestiveImageTaggerProvider : ITaggerProvider
    {
        public ITagger<T> CreateTagger<T>(ITextBuffer buffer)
            where T : ITag
        {
            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            return buffer.Properties.GetOrCreateSingletonProperty(() => new FestiveImageTagger(buffer)) as ITagger<T>;
        }
    }
}
