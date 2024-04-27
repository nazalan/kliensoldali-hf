using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;

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

		private async Task<List<Book>> SearchBooksAsync(string searchQuery)
		{
			List<Book> results = new List<Book>();

			// Az OpenLibrary API végpontja
			//string apiUrl = $"https://openlibrary.org/search.json?q={searchQuery}";
			string apiUrl = $"https://openlibrary.org/search.json?q=crime+and+punishment&fields=key,title,author_name,editions,editions.key,editions.title,editions.ebook_access,editions.language,first_publish_year,subject,cover_i";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					// Elküldjük a GET kérést az OpenLibrary API-hoz
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					// Ellenõrizzük a választ
					if (response.IsSuccessStatusCode)
					{
						// Olvassuk ki a választ JSON formátumban
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						// Feldolgozzuk a választ és hozzáadjuk a találatokat a listához
						foreach (var doc in jsonResponse["docs"])
						{
							string key = doc["key"]?.ToString();
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

	public class Book
	{
		public string Key { get; set; }
		public string Title { get; set; }
		public List<string> AuthorNames { get; set; }
		public string AuthorNamesAsString => string.Join(", ", AuthorNames);
		public int? FirstPublishYear { get; set; }
		public List<string> Subject { get; set; }
		public List<Edition> Editions { get; set; }
		public string CoverImageUrl { get; set; }
	}

	public class Edition
	{
		public string Key { get; set; }
		public string Title { get; set; }
		public int Number { get; set; }
		public string Language { get; set; }
		public string EbookAccess { get; set; }

	}
}
