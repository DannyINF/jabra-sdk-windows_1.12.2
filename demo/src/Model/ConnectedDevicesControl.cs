using System;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Jabra_SDK_Demo.Model
{
	public class ConnectedDevicesControl : INotifyPropertyChanged
	{
		public ConnectedDevicesControl(int deviceId, string deviceName, string version, ImageSource imageSource)
		{
			DeviceId = deviceId;
			DeviceName = deviceName;
			VersionNumber = version;
			ImageSource = imageSource;
		}
		public ConnectedDevicesControl() { }

		private string _deviceName = string.Empty;
		public string DeviceName
		{
			get { return _deviceName; }
			set
			{
				_deviceName = value;
				OnPropertyChanged("DeviceName");
			}
		}

		private string _versionNumber = string.Empty;
		public string VersionNumber
		{
			get { return _versionNumber; }
			set
			{
				_versionNumber = value;
				OnPropertyChanged("VersionNumber");
			}
		}

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

		private ImageSource _imageSource;
		public ImageSource ImageSource
		{
			get { return _imageSource; }
			set
			{
				_imageSource = value;
				OnPropertyChanged("ImageSource");
			}
		}

		private Brush _backgroundColor;

		public Brush BackgroundColor
		{
			get { return _backgroundColor; }
			set
			{
				_backgroundColor = value;
				OnPropertyChanged("BackgroundColor");
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
