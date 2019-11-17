using System;
using Foundation;
using UIKit;
using ImageSearch.ViewModel;
using SDWebImage;
using CoreGraphics;

namespace ImageSearch.iOS
{
    public partial class ViewController : UIViewController, IUICollectionViewDataSource
	{
        ImageSearchViewModel viewModel;

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
            //iOS Image Search code
            viewModel = new ImageSearchViewModel();
            CollectionViewImages.WeakDataSource = this;

            //Button Click event to get images
            ButtonSearch.TouchUpInside += async (sender, args) =>
            {
                ButtonSearch.Enabled = false;
                ActivityIsLoading.StartAnimating();

                await viewModel.SearchForImagesAsync(TextFieldQuery.Text);
                CollectionViewImages.ReloadData();

                ButtonSearch.Enabled = true;
                ActivityIsLoading.StopAnimating();
            };


            //IOS Keyboard Done Code
            var toolbar = new UIToolbar(new CGRect(0.0f, 0.0f, TextFieldQuery.Frame.Size.Width, 44.0f));
            toolbar.Items = new[]
            {
                new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
                new UIBarButtonItem(UIBarButtonSystemItem.Done, delegate { TextFieldQuery.ResignFirstResponder(); })
            };
            TextFieldQuery.InputAccessoryView = toolbar;
        }

        public nint GetItemsCount(UICollectionView collectionView, nint section) => 
            viewModel.Images.Count;

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = collectionView.DequeueReusableCell("imagecell", indexPath) as ImageCell;
            var item = viewModel.Images[indexPath.Row];
            cell.Caption.Text = item.Title;
            cell.Image.SetImage(new NSUrl(item.ThumbnailLink));

            return cell;
        }
    }
}

