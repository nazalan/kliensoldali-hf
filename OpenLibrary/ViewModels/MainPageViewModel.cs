using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenLibrary.Models;
using OpenLibrary.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace OpenLibrary.ViewModels
{
	/// <summary>
	/// ViewModel class responsible for managing the MainPage.
	/// </summary>
	public class MainPageViewModel : ObservableObject
	{
		// Commands for loading and searching books
		public ICommand LoadCommand { get; }
		public ICommand SearchCommand { get; }

		// Property for storing the list of books
		private List<Book> books;
		/// <summary>
		/// Gets or sets the list of books.
		/// </summary>
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
	/// <summary>
	/// Class to hold search parameters.
	/// </summary>
	public class SearchParameters
	{
		/// <summary>
		/// Gets or sets the text to search for.
		/// </summary>
		public string SearchText { get; set; }

		/// <summary>
		/// Gets or sets the author to search for.
		/// </summary>
		public string SearchAuthor { get; set; }

		/// <summary>
		/// Gets or sets the language to search for.
		/// </summary>
		public string SearchLanguage { get; set; }
	}
}
