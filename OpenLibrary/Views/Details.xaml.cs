using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using OpenLibrary.ViewModels;
using System;

// Namespace for the Details page
namespace OpenLibrary.Views
{
	/// <summary>
	/// Details page class declaration.
	/// </summary>
	public sealed partial class Details : Page
	{
		// ViewModel instance for the Details page
		private DetailsViewModel _viewModel;

		// Constructor for the Details page
		public Details()
		{
			InitializeComponent();
			_viewModel = new DetailsViewModel();
		}

		/// <summary>
		/// Invoked when the Page is loaded and becomes the current source of a parent Frame.
		/// </summary>
		/// <param name="e">Event data that describes how this Page was reached. The Parameter
		/// property provides the group to be displayed.</param>
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// Check if the navigation parameter is a string representing the book URI
			if (e.Parameter is string bookUri)
			{
				// Load book details asynchronously and set DataContext
				await _viewModel.LoadBookAsync(bookUri, authorPanel);
				this.DataContext = _viewModel.Book;
			}
		}

		/// <summary>
		/// Event handler for clicking the image button.
		/// </summary>
		private void Image_Button_Click(object sender, RoutedEventArgs e)
		{
			// Update the cover image URL to use a larger version
			_viewModel.Book.CoverImageUrl = _viewModel.Book.CoverImageUrl.Replace("-M", "-L");
			// Set the source of the image control to display the larger image
			image.Source = new BitmapImage(new Uri(_viewModel.Book.CoverImageUrl));
			// Open the image popup
			imagePopup.IsOpen = true;
		}

		/// <summary>
		/// Event handler for clicking the close button in the image popup.
		/// </summary>
		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			// Close the image popup
			imagePopup.IsOpen = false;
		}

		/// <summary>
		/// Event handler for clicking the hyperlink button (back navigation).
		/// </summary>
		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			// Navigate back to the MainPage
			Frame.Navigate(typeof(MainPage));
		}
	}
}
