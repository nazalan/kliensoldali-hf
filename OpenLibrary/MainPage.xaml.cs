using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using OpenLibrary.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Net.Http;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{

		string query = "";
		public MainPage()
		{
			this.InitializeComponent();
			LoadQuery();
		}
		private void LoadQuery()
		{
			if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Query"))
			{
				query = ApplicationData.Current.LocalSettings.Values["Query"].ToString();
			}
			else
			{
				query = "crime+and+punishment";
			}
		}


		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			string key = button.Tag.ToString();
			Frame.Navigate(typeof(Details), key);
		}
		private void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			if (!string.IsNullOrWhiteSpace(searchTextBox.Text))
			{
				query = searchTextBox.Text;
				ApplicationData.Current.LocalSettings.Values["Query"] = query;
				Search();
			}
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Search();
			base.OnNavigatedTo(e);
		}

		private async void Search()
		{
			List<Book> searchResults = await SearchBooksAsync(query);
			resultsListView.ItemsSource = searchResults;
		}

		private async Task<List<Book>> SearchBooksAsync(string searchQuery)
		{
			List<Book> results = new List<Book>();

			string apiUrl = $"https://openlibrary.org/search.json?limit=20&q={searchQuery}&fields=key,title,cover_i,author_name,first_publish_year.json";
			//string apiUrl = $"https://openlibrary.org/search.json?q=";

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
							string key = doc["key"].ToString().Trim().Replace(" ", "");
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
							string coverImageUrl = (coverId != null) ? $"https://covers.openlibrary.org/b/id/{coverId}-S.jpg" : null;

							results.Add(new Book
							{
								Key = key,
								Title = title,
								AuthorNames = authorNames,
								FirstPublishYear = firstPublishYear,
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
