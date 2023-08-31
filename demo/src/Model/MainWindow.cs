using System.ComponentModel;

namespace Jabra_SDK_Demo.Model
{
	public class MainWindow : INotifyPropertyChanged
	{
		public MainWindow()
		{

		}

		//public MainWindow(bool deviceConnected)
		//{
		//	DeviceConnected = deviceConnected;
		//}

		//private bool _deviceConnected;
		//public bool DeviceConnected
		//{
		//	get { return _deviceConnected; }
		//	set
		//	{
		//		_deviceConnected = value;
		//		OnPropertyChanged("DeviceConnected");
		//	}
		//}
    
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
