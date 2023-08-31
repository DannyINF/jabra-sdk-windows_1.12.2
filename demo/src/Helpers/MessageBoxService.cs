using System.Windows;

namespace Jabra_SDK_Demo.Helpers
{
	public static class MessageBoxService
	{
		public static void ShowMessage(string message, string applicationName, MessageBoxButton button, MessageBoxImage messageType)
		{
			MessageBox.Show(message, applicationName, button, messageType);
		}

		public static MessageBoxResult AskForConfirmation(string message, string applicationName, MessageBoxButton button, MessageBoxImage messageType)
		{
			return MessageBox.Show(message, applicationName, button, messageType);
		}
	}
}