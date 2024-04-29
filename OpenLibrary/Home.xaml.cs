using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using OpenLibrary.Models;
//using System.Windows.Navigation;
using Windows.Devices.Enumeration;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Home : Page
	{

		public Home()
		{
			this.InitializeComponent();
		}

		private async void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			string searchQuery = searchTextBox.Text;
			List<Book> searchResults = await SearchBooksAsync(searchQuery);

			resultsListView.ItemsSource = searchResults;
		}

		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			string key = button.Tag.ToString();

			BookDetailPage mynewPage = new BookDetailPage(key);
			this.Content = mynewPage;
		}


		private async Task<List<Book>> SearchBooksAsync(string searchQuery)
		{
			List<Book> results = new List<Book>();

			//string apiUrl = $"https://openlibrary.org/search.json?q={searchQuery}";
			string apiUrl = $"https://openlibrary.org/search.json?q=crime+and+punishment&fields=key,title,author_name,editions,editions.key,editions.title,editions.ebook_access,editions.language,first_publish_year,subject,cover_i";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					if (response.IsSuccessStatusCode)
					{
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						foreach (var doc in jsonResponse["docs"])
						{
							string key = doc["key"].ToString();
							string title = doc["title"]?.ToString();
							List<string> authorNames = new List<string>();
							if (doc["author_name"] != null)
							{
								foreach (var authorName in doc["author_name"])
								{
									authorNames.Add(authorName?.ToString());
								}
							}

							int? firstPublishYear = null;
							if (doc["first_publish_year"] != null)
							{
								firstPublishYear = (int)doc["first_publish_year"];
							}
							string coverId = doc["cover_i"]?.ToString();
							string coverImageUrl = (coverId != null) ? $"https://covers.openlibrary.org/b/id/{coverId}-L.jpg" : null;

							results.Add(new Book
							{
								Key = key,
								Title = title,
								AuthorNames = authorNames,
								FirstPublishYear=firstPublishYear,
								CoverImageUrl = coverImageUrl
							});
						}

					}
					else
					{
						throw new Exception("A kérés nem sikerült. Hibaüzenet: " + response.StatusCode);
					}
				}
				catch (Exception ex)
				{
					throw new Exception("Hiba történt: " + ex.Message);
				}
			}

			return results;
		}
	}
}
