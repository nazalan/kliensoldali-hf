using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;


namespace OpenLibrary
{
    public sealed partial class App : Application
    {
        public App()
        {
            this.InitializeComponent();
        }

		protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
		{
			m_window = new MainWindow();

			// Create a Frame to act as the navigation context and navigate to the first page
			Frame rootFrame = new Frame();
			rootFrame.NavigationFailed += OnNavigationFailed;
			// Navigate to the first page, configuring the new page
			// by passing required information as a navigation parameter
			rootFrame.Navigate(typeof(MainPage), args.Arguments);

			// Place the frame in the current Window
			m_window.Content = rootFrame;
			// Ensure the MainWindow is active
			m_window.Activate();
		}

		void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		private Window m_window;

	}
}
