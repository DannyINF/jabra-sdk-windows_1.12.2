using Jabra_SDK_Demo.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jabra_SDK_Demo.Helpers
{
	public class IntegrationService
	{
		public static event EventHandler<PropertyChangedEventArgs> StaticPropertyChanged;
		public static void RaiseStaticPropertyChanged(string propName)
		{
			EventHandler<PropertyChangedEventArgs> handler = StaticPropertyChanged;
			if (handler != null)
				handler(null, new PropertyChangedEventArgs(propName));
		}


		private static bool _readyForTelephonyEnabled;
		public static bool ReadyForTelephonyEnabled
		{
			get { return _readyForTelephonyEnabled; }
			set
			{
				_readyForTelephonyEnabled = value;
				RaiseStaticPropertyChanged("ReadyForTelephonyEnabled");
			}
		}

		private static bool _readyForTelephonyChecked;
		public static bool ReadyForTelephonyChecked
		{
			get { return _readyForTelephonyChecked; }
			set
			{
				_readyForTelephonyChecked = value;
				RaiseStaticPropertyChanged("ReadyForTelephonyChecked");
			}
		}

		private static string _clientGuid = "EFDD02FE-46A4-4DFD-8D1D-943A9B3F2E4A";
		public static string ClientGuid
		{
			get { return _clientGuid.ToUpper(CultureInfo.InvariantCulture); }
			set
			{
				_clientGuid = value;
				RaiseStaticPropertyChanged("ClientGuid");
			}
		}

		private static string _clientName = "My softphone";
		public static string ClientName
		{
			get { return _clientName; }
			set
			{
				_clientName = value;
				RaiseStaticPropertyChanged("ClientName");
			}
		}

		private static bool _connectEnabled = true;
		public static bool ConnectEnabled
		{
			get { return _connectEnabled; }
			set
			{
				_connectEnabled = value;
				RaiseStaticPropertyChanged("ConnectEnabled");
			}
		}

		private static string _isClientInFocus;
		public static string IsClientInFocus
		{
			get { return _isClientInFocus; }
			set
			{
				_isClientInFocus = value;
				RaiseStaticPropertyChanged("IsClientInFocus");
			}
		}

		public static bool IsConnectedToIntegrationService { get; set; }

		public static void ShowIntegrationServices()
		{
			IsClientInFocus = MainWindowViewModel.IntegrationServices.IsInFocus ? "Yes" : "No";
		}
	}
}
