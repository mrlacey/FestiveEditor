using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace FestiveEditor
{
    internal sealed class FestiveImageAdornmentTagger
        : IntraTextAdornmentTagger<FestiveImageTag, FestiveImageAdornment>
    {
        private readonly ITagAggregator<FestiveImageTag> tagger;

        private FestiveImageAdornmentTagger(IWpfTextView view, ITagAggregator<FestiveImageTag> tagger)
            : base(view)
        {
            this.tagger = tagger;
        }

        public void Dispose()
        {
            this.tagger.Dispose();

            this.view.Properties.RemoveProperty(typeof(FestiveImageAdornmentTagger));
        }

        internal static ITagger<IntraTextAdornmentTag> GetTagger(IWpfTextView view, Lazy<ITagAggregator<FestiveImageTag>> tagger)
        {
            return view.Properties.GetOrCreateSingletonProperty(
                () => new FestiveImageAdornmentTagger(view, tagger.Value));
        }

        // To produce adornments that don't obscure the text, the adornment tags
        // should have zero length spans. Overriding this method allows control
        // over the tag spans.
        protected override IEnumerable<Tuple<SnapshotSpan, PositionAffinity?, FestiveImageTag>> GetAdornmentData(NormalizedSnapshotSpanCollection spans)
        {
            if (spans.Count == 0)
            {
                yield break;
            }

            ITextSnapshot snapshot = spans[0].Snapshot;

            var fiTags = this.tagger.GetTags(spans);

            foreach (IMappingTagSpan<FestiveImageTag> tagSpan in fiTags)
            {
                NormalizedSnapshotSpanCollection linkTagSpans = tagSpan.Span.GetSpans(snapshot);

                // Ignore tags that are split by projection.
                // This is theoretically possible but highly unlikely.
                if (linkTagSpans.Count != 1)
                {
                    continue;
                }

                // If the length of the term is an odd number put the image at the start
                //  other wise put it at the end of the span
                // Move 1 from the start or end as the RegEx matches an extra (non-"word")
                //  character either side of the matching characters
                SnapshotSpan adornmentSpan = tagSpan.Tag.Term.Length % 2 == 1
                    ? new SnapshotSpan(linkTagSpans[0].Start + 1, 0)
                    : new SnapshotSpan(linkTagSpans[0].End - 1, 0);

                yield return Tuple.Create(adornmentSpan, (PositionAffinity?)PositionAffinity.Successor, tagSpan.Tag);
            }
        }

        protected override FestiveImageAdornment CreateAdornment(FestiveImageTag dataTag, SnapshotSpan span)
        {
            return new FestiveImageAdornment(dataTag);
        }

        protected override bool UpdateAdornment(FestiveImageAdornment adornment, FestiveImageTag dataTag)
        {
            adornment.Update(dataTag);
            return true;
        }
    }
}
