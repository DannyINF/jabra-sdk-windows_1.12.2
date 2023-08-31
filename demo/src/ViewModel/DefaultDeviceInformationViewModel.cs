using System;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using Jabra_SDK_Demo.Model;

namespace Jabra_SDK_Demo.ViewModel
{
	public class DefaultDeviceInformationViewModel : ViewModelBase
	{
		private DefaultDeviceInformation _defaultDeviceInformation;

		public DefaultDeviceInformation DefaultDeviceInformation
		{
			get { return _defaultDeviceInformation; }
			set
			{
				_defaultDeviceInformation = value;
				OnPropertyChanged("DefaultDeviceInformation");
			}
		}
		public DefaultDeviceInformationViewModel()
		{
			_defaultDeviceInformation = new DefaultDeviceInformation();
			_defaultDeviceInformation.DeviceId = -1;
			_defaultDeviceInformation.DeviceName = "No Devices Connected";
		}

		#region INotifyPropertyChanged Members

#pragma warning disable CS0108 // 'DefaultDeviceInformationViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.
		public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // 'DefaultDeviceInformationViewModel.PropertyChanged' hides inherited member 'ObservableObject.PropertyChanged'. Use the new keyword if hiding was intended.

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
