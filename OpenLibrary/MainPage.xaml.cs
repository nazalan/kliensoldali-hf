using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
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
using Windows.Globalization;
using OpenLibrary.Services;
using OpenLibrary.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{

		private string query = "";
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
			if (!string.IsNullOrWhiteSpace(searchTextBox.Text) || !string.IsNullOrWhiteSpace(searchLanguage.Text) || !string.IsNullOrWhiteSpace(searchAuthor.Text))
			{
				query = searchTextBox.Text;
				string language = searchLanguage.Text;
				string langParameter = !string.IsNullOrWhiteSpace(language) ? $"&lang={Uri.EscapeDataString(language.Substring(0, Math.Min(2, language.Length)))}" : "";
				string author = searchAuthor.Text;
				string authorParameter = !string.IsNullOrWhiteSpace(author) ? $"&author={author}" : "";
				query = query + langParameter + authorParameter;
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
			LibraryService service = new LibraryService();
			List<Book> searchResults = await service.SearchBooksAsync(query);
			resultsListView.ItemsSource = searchResults;
		}
	}
}
