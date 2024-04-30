using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using OpenLibrary.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class Details : Page
	{
		string bookUri = "";
		public Details()
		{
			this.InitializeComponent();
			
		}
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e.Parameter is string)
			{
				bookUri=e.Parameter.ToString();
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
				Book book = await SearchWorkAsyncByKey(uri);
				this.DataContext = book;
			}
			catch (Exception ex)
			{
				throw new Exception("Hiba történt: " + ex.Message);
			}
		}

		private async Task<Book> SearchWorkAsyncByKey(string uri)
		{
			Book results;
			string apiUrl = $"https://openlibrary.org{uri}.json";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					if (response.IsSuccessStatusCode)
					{
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						//Key
						string key = jsonResponse["key"]?.ToString();
						//Title
						string title = jsonResponse["title"]?.ToString();
						//Cover
						string coverId = jsonResponse["covers"]?[0].ToString();
						string coverImageUrl = (coverId != null) ? $"https://covers.openlibrary.org/b/id/{coverId}-S.jpg" : null;
						//Authors
						JArray authorsArray = (JArray)jsonResponse["authors"];
						List<string> authorKeys = new List<string>();
						foreach (JObject authorObject in authorsArray)
						{
							string authorkey = (string)authorObject["author"]["key"];
							authorKeys.Add(authorkey);
						}
						List<string> authorNames = new List<string>();
						foreach (string authorkey in authorKeys)
						{
							Author author = new Author();
							await author.SearchAuthorsAsyncByKey(authorkey);
							authorNames.Add(author.Name);
						}
						//Description
						string description = "";
						if (jsonResponse["description"]!=null)
						{JToken descriptionToken = jsonResponse["description"];
							if (descriptionToken.Type == JTokenType.Object)
							{
								description = descriptionToken["value"]?.ToString();
							}
							else if (descriptionToken.Type == JTokenType.String)
							{
								description = descriptionToken.ToString();
							}
						}
						//PublishDate
						int? firstPublishYear = null;
						if (jsonResponse["first_publish_date"] != null)
						{
							firstPublishYear = (int)jsonResponse["first_publish_date"];
						}
						//Subjects
						List<string> subjects = new List<string>();
						if (jsonResponse["subjects"] != null)
						{
							foreach (var subject in jsonResponse["subjects"])
							{
								subjects.Add(subject?.ToString());
							}
						}
						//new Book
						results = (new Book
						{
							Key = key,
							Title = title,
							CoverImageUrl = coverImageUrl,
							AuthorNames = authorNames,
							Description = description,
							FirstPublishYear = firstPublishYear,
							Subjects = subjects,
						});

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
