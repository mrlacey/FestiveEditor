using System;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace FestiveEditor
{
    internal sealed class FestiveImageAdornment : System.Windows.Controls.Image
    {
        internal FestiveImageAdornment(FestiveImageTag tag)
        {
            this.FestiveImageTag = tag;
            this.SetSource(this.FestiveImageTag.Term);
            this.Height = 20;
            this.Width = 20;
        }

        public FestiveImageTag FestiveImageTag { get; private set; }

        internal void Update(FestiveImageTag dataTag)
        {
            this.FestiveImageTag = dataTag;
            this.SetSource(this.FestiveImageTag.Term);
        }

        internal void SetSource(string term)
        {
            switch (term.Trim().Length)
            {
                case 3:
                    this.Source = GetImagePath("present.png");
                    break;
                case 4:
                    this.Source = GetImagePath("tree.png");
                    break;
                case 5:
                    this.Source = GetImagePath("snowflake.png");
                    break;
                case 6:
                    this.Source = GetImagePath("snowman.png");
                    break;
                case 7:
                default:
                    this.Source = GetImagePath("santa.png");
                    break;
            }
        }

        private BitmapImage GetImagePath(string imageFileName)
        {
            return new BitmapImage(
                new Uri(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                        "Images",
                        imageFileName)));
        }
    }
}
