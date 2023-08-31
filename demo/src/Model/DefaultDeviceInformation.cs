using System.ComponentModel;

namespace Jabra_SDK_Demo.Model
{
	public class DefaultDeviceInformation : INotifyPropertyChanged
	{
		private int _deviceId = -1;
		public int DeviceId
		{
			get { return _deviceId; }
			set
			{
				_deviceId = value;
				OnPropertyChanged("DeviceId");
			}
		}

		private string _deviceName;
		public string DeviceName
		{
			get { return _deviceName; }
			set
			{
				_deviceName = value;
				OnPropertyChanged("DeviceName");
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
