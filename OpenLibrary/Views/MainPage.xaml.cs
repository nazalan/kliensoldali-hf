using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// Namespace for the MainPage
namespace OpenLibrary.Views
{
	public sealed partial class MainPage : Page
	{
		// Constructor for the MainPage
		public MainPage()
		{
			this.InitializeComponent();
		}

		// Event handler for clicking the hyperlink button to view book details
		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			// Get the button that raised the event
			Button button = (Button)sender;
			// Get the unique key of the book from the button's Tag property
			string key = button.Tag.ToString();
			// Navigate to the Details page passing the key as parameter
			Frame.Navigate(typeof(Details), key);
		}

		// Event handler for the Page's Loaded event
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			// Execute the LoadCommand of the ViewModel when the page is loaded
			ViewModel.LoadCommand.Execute(null);
		}
	}
}
