using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OpenLibrary.Models;
using OpenLibrary.Services;

namespace OpenLibrary.ViewModels
{
	public class DetailsViewModel : ObservableObject
	{
		private Book _book;
		public Book Book
		{
			get => _book;
			set => SetProperty(ref _book, value);
		}

		public async Task LoadBookAsync(string uri, StackPanel panel)
		{
			try
			{
				LibraryService service = new LibraryService();
				Book = await service.SearchWorkAsyncByKey(uri);
				LoadAuthorLinks(panel , Book.Authors);
			}
			catch (Exception ex)
			{
				throw new Exception("Hiba történt: " + ex.Message + "   " + uri);
			}
		}

		private void LoadAuthorLinks(StackPanel panel, List<Author> authors)
		{
			foreach (var author in authors)
			{
				if (!string.IsNullOrEmpty(author.Link))
				{
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
