using System;
using System.IO;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.VisualStudio.Text.Editor;

namespace FestiveEditor
{
    internal sealed class TopRightHollyAdornment
    {
        private const double ImageHeight = 111;
        private const double ImageWidth = 150;

        private readonly IWpfTextView view;

        private readonly Image image;

        private readonly IAdornmentLayer adornmentLayer;

        public TopRightHollyAdornment(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;

            this.image = new Image
            {
                Source = new BitmapImage(
                    new Uri(
                        Path.Combine(
                            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                            "Images",
                            "holly_top_right.png"))),
                Height = ImageHeight,
                Width = ImageWidth
            };

            this.adornmentLayer = view.GetAdornmentLayer("FestiveAdornments");

            this.view.LayoutChanged += this.OnSizeChanged;
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            // Remove the image (if already displayed) so it can be re-added at the new location
            this.adornmentLayer.RemoveAdornment(this.image);

            // Place the image in the top right of the Viewport
            Canvas.SetLeft(this.image, this.view.ViewportRight - ImageWidth);
            Canvas.SetTop(this.image, this.view.ViewportTop);

            // Add the image to the adornment layer and make it relative to the viewport
            this.adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, this.image, null);
        }
    }
}
