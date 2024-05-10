using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

// Namespace for the MainPage
namespace OpenLibrary.Views
{
	/// <summary>
	/// MainPage class declaration.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		/// <summary>
		/// Initializes a new instance of the MainPage class.
		/// </summary>
		public MainPage()
		{
			this.InitializeComponent();
		}

		/// <summary>
		/// Event handler for clicking the hyperlink button to view book details.
		/// </summary>
		/// <param name="sender">The button that raised the event.</param>
		/// <param name="e">The event data.</param>
		private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
		{
			// Get the button that raised the event
			Button button = (Button)sender;
			// Get the unique key of the book from the button's Tag property
			string key = button.Tag.ToString();
			// Navigate to the Details page passing the key as parameter
			Frame.Navigate(typeof(Details), key);
		}

		/// <summary>
		/// Event handler for the Page's Loaded event.
		/// </summary>
		/// <param name="sender">The source of the event.</param>
		/// <param name="e">An object that contains the event data.</param>
		private void Page_Loaded(object sender, RoutedEventArgs e)
		{
			// Execute the LoadCommand of the ViewModel when the page is loaded
			ViewModel.LoadCommand.Execute(null);
		}
	}
}
