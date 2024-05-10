using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenLibrary.Models;
using OpenLibrary.Services;
using OpenLibrary.Views;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Xml.Schema;
using Windows.Storage;
using static System.Reflection.Metadata.BlobBuilder;

namespace OpenLibrary.ViewModels
{
	public class MainPageViewModel : ObservableObject
	{
		// Commands for loading and searching books
		public ICommand LoadCommand { get; }
		public ICommand SearchCommand { get; }

		// Property for storing the list of books
		private List<Book> books;
		public List<Book> Books
		{
			get => books;
			set => SetProperty(ref books, value);
		}

		// Query string for book search
		private string query = "";

		// Constructor
		public MainPageViewModel()
		{
			// Initialize commands
			LoadCommand = new AsyncRelayCommand(Load);
			SearchCommand = new AsyncRelayCommand<SearchParameters>(Search);
		}

		// Method to load books
		public async Task Load()
		{
			LoadQuery(); // Load saved query from local settings
			LibraryService service = new LibraryService();
			List<Book> searchResults = await service.SearchBooksAsync(query); // Search books based on query
			Books = searchResults; // Set the list of books
			System.Diagnostics.Debug.WriteLine(Books[0].Title); // Output the title of the first book (for debugging)
		}

		// Method to load the saved query from local settings
		public void LoadQuery()
		{
			if (ApplicationData.Current.LocalSettings.Values.ContainsKey("Query"))
			{
				query = ApplicationData.Current.LocalSettings.Values["Query"].ToString();
			}
			else
			{
				query = "crime+and+punishment"; // Default query if not found in settings
			}
		}

		// Method to perform book search
		public async Task Search(SearchParameters parameters)
		{
			if (!string.IsNullOrWhiteSpace(parameters.SearchText) || !string.IsNullOrWhiteSpace(parameters.SearchLanguage) || !string.IsNullOrWhiteSpace(parameters.SearchAuthor))
			{
				query = parameters.SearchText; // Set the search query from the provided parameters
				string language = parameters.SearchLanguage;
				string langParameter = !string.IsNullOrWhiteSpace(language) ? $"&lang={Uri.EscapeDataString(language.Substring(0, Math.Min(2, language.Length)))}" : "";
				string author = parameters.SearchAuthor;
				string authorParameter = !string.IsNullOrWhiteSpace(author) ? $"&author={author}" : "";
				query = query + langParameter + authorParameter; // Append language and author parameters to the query
				ApplicationData.Current.LocalSettings.Values["Query"] = query; // Save the query in local settings
				await Load(); // Load books based on the updated query
			}
			System.Diagnostics.Debug.WriteLine(parameters.SearchText); // Output the search text (for debugging)
		}
	}

	// Class to hold search parameters
	public class SearchParameters
	{
		public string SearchText { get; set; } // Text to search for
		public string SearchAuthor { get; set; } // Author to search for
		public string SearchLanguage { get; set; } // Language to search for
	}
}
