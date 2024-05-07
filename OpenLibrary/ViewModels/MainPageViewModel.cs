using CommunityToolkit.Mvvm.ComponentModel;
using OpenLibrary.Models;
using OpenLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Windows.Storage;

namespace OpenLibrary.ViewModels
{
	public class MainPageViewModel : ObservableObject
	{
		private string query = "";

		public void LoadQuery()
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

		public async Task<List<Book>> Search(string searchTextBox, string searchAuthor, string searchLanguage)
		{

			if (!string.IsNullOrWhiteSpace(searchTextBox) || !string.IsNullOrWhiteSpace(searchLanguage) || !string.IsNullOrWhiteSpace(searchAuthor))
			{
				query = searchTextBox;
				string language = searchLanguage;
				string langParameter = !string.IsNullOrWhiteSpace(language) ? $"&lang={Uri.EscapeDataString(language.Substring(0, Math.Min(2, language.Length)))}" : "";
				string author = searchAuthor;
				string authorParameter = !string.IsNullOrWhiteSpace(author) ? $"&author={author}" : "";
				query = query + langParameter + authorParameter;
				ApplicationData.Current.LocalSettings.Values["Query"] = query;
			}

			List<Book> searchResults = await Load();
			return searchResults;
		}

		public async Task<List<Book>> Load()
		{
			LibraryService service = new LibraryService();
			List<Book> searchResults = await service.SearchBooksAsync(query);
			return searchResults;
		}
	}
}
