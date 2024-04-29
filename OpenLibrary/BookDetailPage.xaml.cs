using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System.Threading.Tasks;
using System.Net.Http;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;
using Newtonsoft.Json.Linq;
using OpenLibrary.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookDetailPage : Page
    {
		private Book book1;
		public BookDetailPage(string bookUri)
        {
            this.InitializeComponent();
			LoadBookAsync(bookUri);

		}

		private async void LoadBookAsync(string uri)
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

						//Title
						string title = jsonResponse["title"]?.ToString();
						//Cover
						string coverId = jsonResponse["covers"]?[0].ToString();
						string coverImageUrl = (coverId != null) ? $"https://covers.openlibrary.org/b/id/{coverId}-L.jpg" : null;
						//Authors
						JArray authorsArray = (JArray)jsonResponse["authors"];
						List<string> authorKeys = new List<string>();
						foreach (JObject authorObject in authorsArray)
						{
							string key = (string)authorObject["author"]["key"];
							authorKeys.Add(key);
						}
						List<string> authorNames = new List<string>();
						foreach (string key in authorKeys)
						{
							Author author = new Author();
							await author.SearchAuthorsAsyncByKey(key);
							authorNames.Add(author.Name);
						}


						//new Book
						results = (new Book
						{
							Title = title,
							CoverImageUrl = coverImageUrl,
							AuthorNames= authorNames

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
