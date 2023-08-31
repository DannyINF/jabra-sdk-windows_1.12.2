using System.ComponentModel;

namespace Jabra_SDK_Demo.Model
{

	public class HeaderControl : INotifyPropertyChanged
	{
		public HeaderControl(string version)
		{
			SdkVersion = version;
		}

		public HeaderControl()
		{
		}

		private string _sdkVersion;
		public string SdkVersion
		{
			get { return _sdkVersion; }
			set
			{
				_sdkVersion = value;
				OnPropertyChanged("SdkVersion");
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
