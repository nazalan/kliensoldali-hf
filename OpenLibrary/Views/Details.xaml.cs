using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using OpenLibrary.Models;
using OpenLibrary.Services;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary
{
	public sealed partial class Details : Page
	{
		private Book book;
		public Details()
		{
			this.InitializeComponent();
			
		}

		private void Image_Button_Click(object sender, RoutedEventArgs e)
		{
			book.CoverImageUrl = book.CoverImageUrl.Replace("-M", "-L");
			image.Source = new BitmapImage(new Uri(book.CoverImageUrl));
			imagePopup.IsOpen = true;
		}

		private void CloseButton_Click(object sender, RoutedEventArgs e)
		{
			imagePopup.IsOpen = false;
		}


		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e.Parameter is string)
			{
				string bookUri=e.Parameter.ToString();
				await LoadBookAsync(bookUri);

			}
			base.OnNavigatedTo(e);
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MainPage));
		}

		private async Task LoadBookAsync(string uri)
		{
			try
			{
				LibraryService service = new LibraryService();
				book = await service.SearchWorkAsyncByKey(uri);
				this.DataContext = book;
				LoadAuthorLinks(authorPanel, book.Authors);
			}
			catch (Exception ex)
			{
				throw new Exception("Hiba történt: " + ex.Message +"   " +uri);
			}
		}
		private void LoadAuthorLinks(StackPanel panel, List<Author> authors)
		{
			foreach (var author in authors)
			{
				if (!string.IsNullOrEmpty(author.Link))
				{
					HyperlinkButton hyperlinkButton = new HyperlinkButton
					{
						Content = author.Name,
						NavigateUri = new Uri(author.Link),
						Margin = new Thickness(0, 0, 5, 0)
					};
					panel.Children.Add(hyperlinkButton);
				}
				else
				{
					TextBlock textBlock = new TextBlock
					{
						Text = author.Name,
						Margin = new Thickness(0, 0, 5, 0),
						FontSize = 16 
					};
					panel.Children.Add(textBlock);
				}
			}
		}

		
	}
}
