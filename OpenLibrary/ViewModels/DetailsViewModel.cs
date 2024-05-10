using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OpenLibrary.Models;
using OpenLibrary.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenLibrary.ViewModels
{
	/// <summary>
	/// ViewModel class responsible for managing book details.
	/// </summary>
	public class DetailsViewModel : ObservableObject
	{
		// Property for storing the book details
		private Book _book;
		/// <summary>
		/// Gets or sets the Book object representing the book details.
		/// </summary>
		public Book Book
		{
			get => _book;
			set => SetProperty(ref _book, value);
		}

		/// <summary>
		/// Method to asynchronously load book details.
		/// </summary>
		/// <param name="uri">The URI of the book.</param>
		/// <param name="panel">The StackPanel to load author links into.</param>
		/// <returns>A Task representing the asynchronous operation.</returns>
		public async Task LoadBookAsync(string uri, StackPanel panel)
		{
			try
			{
				LibraryService service = new LibraryService();
				// Retrieve book details based on the provided URI
				Book = await service.SearchWorkAsyncByKey(uri);
				// Load author links into the provided StackPanel
				LoadAuthorLinks(panel, Book.Authors);
			}
			catch (Exception ex)
			{
				// Throw an exception if an error occurs during loading
				throw new Exception("An error occurred: " + ex.Message + "   " + uri);
			}
		}

		/// <summary>
		/// Method to load author links into the StackPanel.
		/// </summary>
		/// <param name="panel">The StackPanel to load author links into.</param>
		/// <param name="authors">The list of authors.</param>
		private void LoadAuthorLinks(StackPanel panel, List<Author> authors)
		{
			foreach (var author in authors)
			{
				// Check if the author has a link
				if (!string.IsNullOrEmpty(author.Link))
				{
					// Create a HyperlinkButton for the author with a link
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
					// Create a TextBlock for the author without a link
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
