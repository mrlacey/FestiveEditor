using System;
using System.IO;
using System.Net.Cache;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.VisualStudio.Text.Editor;

namespace FestiveEditor
{
    internal sealed class BottomRightHollyAdornment
    {
        private const double ImageHeight = 111;
        private const double ImageWidth = 150;

        private readonly IWpfTextView view;

        private readonly Image image;

        private readonly IAdornmentLayer adornmentLayer;

        public BottomRightHollyAdornment(IWpfTextView view)
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
                            "holly_bottom_right.png"))),
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

            // Place the image in the bottom right of the Viewport
            Canvas.SetLeft(this.image, this.view.ViewportRight - ImageWidth);
            Canvas.SetTop(this.image, this.view.ViewportBottom - ImageHeight);

            // Add the image to the adornment layer and make it relative to the viewport
            this.adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, this.image, null);
        }
    }
    internal sealed class BuildIndicatorAdornment
    {
        private const string ImageSource = "https://ci.appveyor.com/api/projects/status/6pyiq51qil3dgc5q";
        private const double ImageHeight = 18;
        private const double ImageWidth = 100;

        private readonly IWpfTextView view;

        private readonly Image image;

        private readonly IAdornmentLayer adornmentLayer;

        private DispatcherTimer refreshTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);

        public BuildIndicatorAdornment(IWpfTextView view)
        {
            if (view == null)
            {
                throw new ArgumentNullException(nameof(view));
            }

            this.view = view;

            this.image = new Image
            {
                Source = new BitmapImage(
                    new Uri(ImageSource)),
                Height = ImageHeight,
                Width = ImageWidth,
            };


            refreshTimer.Interval = TimeSpan.FromMinutes(1);
            refreshTimer.Tick += (e, s) => {

                var _image = new BitmapImage();
                _image.BeginInit();
                _image.CacheOption = BitmapCacheOption.None;
                _image.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                _image.CacheOption = BitmapCacheOption.OnLoad;
                _image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                _image.UriSource = new Uri(ImageSource, UriKind.RelativeOrAbsolute);
                _image.EndInit();
                this.image.Source = _image;

            };
            refreshTimer.Start();

            this.adornmentLayer = view.GetAdornmentLayer("FestiveAdornments");

            this.view.LayoutChanged += this.OnSizeChanged;
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            // Remove the image (if already displayed) so it can be re-added at the new location
            this.adornmentLayer.RemoveAdornment(this.image);

            // Place the image in the bottom right of the Viewport
            Canvas.SetLeft(this.image, this.view.ViewportRight - ImageWidth);
            Canvas.SetTop(this.image, this.view.ViewportBottom - ImageHeight);

            // Add the image to the adornment layer and make it relative to the viewport
            this.adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, null, null, this.image, null);
        }
    }
}
