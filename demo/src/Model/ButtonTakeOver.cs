using System.ComponentModel;

namespace Jabra_SDK_Demo.Model
{
	public class ButtonTakeOver
	{

		private bool _buttonConfigurationSupported;
		public bool ButtonConfigurationSupported
		{
			get { return _buttonConfigurationSupported; }
			set
			{
				_buttonConfigurationSupported = value;
				OnPropertyChanged("ButtonConfigurationSupported");
			}
		}

		#region INotifyPropertyChanged Members

		public event PropertyChangedEventHandler PropertyChanged;

		// Create the OnPropertyChanged method to raise the event
		protected void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(name));
			}
		}

		#endregion
	}
}
