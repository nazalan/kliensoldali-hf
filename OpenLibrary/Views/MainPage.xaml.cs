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
using OpenLibrary.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OpenLibrary.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		private MainPageViewModel _viewModel;
		
		public MainPage()
		{
			this.InitializeComponent();
			_viewModel = new MainPageViewModel();
			_viewModel.LoadQuery();
		}
		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			string key = button.Tag.ToString();
			Frame.Navigate(typeof(Details), key);
		}
		private async void SearchButton_Click(object sender, RoutedEventArgs e)
		{
			resultsListView.ItemsSource = await _viewModel.Search(searchTextBox.Text, searchAuthor.Text, searchLanguage.Text);
		}
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			resultsListView.ItemsSource = await _viewModel.Load() ;
			base.OnNavigatedTo(e);
		}
	}
}
