using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using OpenLibrary.Models;
using OpenLibrary.Services;
using OpenLibrary.ViewModels;
using OpenLibrary.Views;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary.Views
{
	public sealed partial class Details : Page
	{
		private DetailsViewModel _viewModel;
		public Details()
		{
			InitializeComponent();
			_viewModel = new DetailsViewModel();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter is string bookUri)
			{
				await _viewModel.LoadBookAsync(bookUri, authorPanel);
				this.DataContext = _viewModel.Book;
			}
		}

		private void Image_Button_Click(object sender, RoutedEventArgs e)
		{
			_viewModel.Book.CoverImageUrl = _viewModel.Book.CoverImageUrl.Replace("-M", "-L");
			image.Source = new BitmapImage(new Uri(_viewModel.Book.CoverImageUrl));
			imagePopup.IsOpen = true;
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			imagePopup.IsOpen = false;
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainPage));
		}
	}
}
