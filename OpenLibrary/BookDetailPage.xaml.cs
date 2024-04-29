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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BookDetailPage : Page
    {
        public BookDetailPage(string bookUri)
        {
            this.InitializeComponent();
			_ = SearchBooksAsync(bookUri);

		}

		private async Task SearchBooksAsync(string uri)
		{
			List<Book> results = new List<Book>();
			string apiUrl = $"https://openlibrary.org{uri}.json";

			using (HttpClient client = new HttpClient())
			{
				try
				{
					HttpResponseMessage response = await client.GetAsync(apiUrl);

					if (response.IsSuccessStatusCode)
					{
						dynamic jsonResponse = await response.Content.ReadAsAsync<dynamic>();

						string title = jsonResponse["title"]?.ToString();


						results.Add(new Book
						{
							Title = title,
							//AuthorNames = authorNames,
							//FirstPublishYear = firstPublishYear,
							//CoverImageUrl = coverImageUrl
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

			results.Add(new Book
			{
				Title = "title",
				AuthorNames = { "authorNames", "authorNames2" },
				FirstPublishYear = 20,
			});
			results.Add(new Book
			{
				Title = "title2",
				AuthorNames = { "authorNames2", "authorNames" },
				FirstPublishYear = 202,
			});

			BookListView.ItemsSource = results;
		}

	}
}
