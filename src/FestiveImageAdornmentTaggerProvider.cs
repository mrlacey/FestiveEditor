using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;

namespace FestiveEditor
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("text")]
    [ContentType("projection")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TagType(typeof(IntraTextAdornmentTag))]
    internal sealed class FestiveImageAdornmentTaggerProvider : IViewTaggerProvider
    {
#pragma warning disable 649 // "field never assigned to" -- field is set by MEF.
        [Import]
#pragma warning disable SA1401 // Fields should be private
        internal IBufferTagAggregatorFactoryService BufferTagAggregatorFactoryService;
#pragma warning restore SA1401 // Fields should be private
#pragma warning restore 649

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer)
            where T : ITag
        {
            if (textView == null)
            {
                throw new ArgumentNullException(nameof(textView));
            }

            if (buffer == null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }

            if (buffer != textView.TextBuffer)
            {
                return null;
            }

            return FestiveImageAdornmentTagger.GetTagger(
                (IWpfTextView)textView,
                new Lazy<ITagAggregator<FestiveImageTag>>(
                    () => this.BufferTagAggregatorFactoryService.CreateTagAggregator<FestiveImageTag>(textView.TextBuffer)))
                as ITagger<T>;
        }
    }
}
