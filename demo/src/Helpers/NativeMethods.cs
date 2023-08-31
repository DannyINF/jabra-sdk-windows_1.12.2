using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Jabra_SDK_Demo.Helpers
{
	static class NativeMethods
	{
		public enum DataType
		{
			SettingByte = 0,
			SettingString
		}

		public enum ControlType
		{
			CntrlRadio = 0,
			CntrlToggle,
			CntrlComboBox,
			CntrlDrpDown,
			CntrlLabel,
			CntrlTextBox,
			CntrlButton,
			CntrlUnknown
		}

		public enum JabraReturnCode
		{
			ReturnOk,
			DeviceUnknown,
			DeviceInvalid,
			NotSupported,
			ReturnParameterFail,
			NoInformation,
			NetworkRequestFail,
			DeviceWriteFail,
			NoFactoryResetSupported,
			DeviceLock,
			DeviceNotLock,
			SystemError,
			Device_BadState
		}

		public enum JabraHidInput
		{
			Undefined,
			OffHook,
			Mute,
			Flash,
			Redial,
			Key0,
			Key1,
			Key2,
			Key3,
			Key4,
			Key5,
			Key6,
			Key7,
			Key8,
			Key9,
			KeyStar,
			KeyPound,
			KeyClear,
			Online,
			SpeedDial,
			VoiceMail,
			LineBusy,
			RejectCall,
			OutOfRange,
			PseudoOffHook,
			Button1,
			Button2,
			Button3,
			VolumeUp,
			VolumeDown,
			FireAlarm,
			JackConnection,
			QDConnection,
			HeadsetConnection,
		}

		public enum JabraErrorStatus
		{
			NoError = 0,
			SslError,
			CertError,
			NetworkError,
			DownloadError,
			ParseError,
			OtherError
		}

		public enum DeviceListtype
		{
			SearchResult,
			PairedDevices
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ValidationRule
		{
			public int MinLength;
			public int MaxLength;
			public string RegexExp;
			public string ErrorMessage;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DependencySetting
		{
			public string Guid;

			[MarshalAs(UnmanagedType.I1)]
			public bool EnableFlag;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ListKeyValue
		{
			public ushort Key;
			public IntPtr Value;
			public int DependentCount;
			public IntPtr DependencySetting;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct SettingInfo
		{
			public string Guid;

			public string SettingName;

			public string HelpText;

			public IntPtr CurrentValue;

			public int ListSize;

			public IntPtr ListKeyValues;

			[MarshalAs(UnmanagedType.I1)]
			public bool IsValidationSupport;

			public IntPtr ValidationRules;

			[MarshalAs(UnmanagedType.I1)]
			public bool IsDeviceRestart;

			[MarshalAs(UnmanagedType.I1)]
			public bool IsWirelessConnect;

			public ControlType CntrlType;

			public DataType SettingDataType;

			public string GroupName;

			public string GroupHelpText;

			[MarshalAs(UnmanagedType.I1)]
			public bool IsDepedentsetting;

			public IntPtr DependentDefaultValue;

			[MarshalAs(UnmanagedType.I1)]
			public bool IsPCsetting;

			[MarshalAs(UnmanagedType.I1)]
			public bool IsChildDeviceSetting;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct DeviceSettings
		{
			public uint SettingCount;

			public IntPtr SettingInfos;
			public JabraErrorStatus errorStatus;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct JabraDeviceInfo
		{
			public ushort DeviceId;
			public ushort ProductId;
			public IntPtr DeviceName;
			public IntPtr UsbDevicePath;
			public IntPtr ParentInstanceId;
			public JabraErrorStatus ErrorStatus;

			[MarshalAs(UnmanagedType.I1)]
			public bool IsDongleDevice;
			public IntPtr DongleName;
			public IntPtr Variant;
			public IntPtr SerialNumber;
		}

		// Remote MMI structure for button take over
		[StructLayout(LayoutKind.Sequential)]
		public struct ButtonTakeOverEventType
		{
			public ushort Key;
			public string Value;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ButtonTakeOverEventInfo
		{
			public ushort ButtonTypeKey;
			public string ButtonTypeValue;
			public int ButtonEventTypeSize;
			public IntPtr buttonTakeOverEventType;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct ButtonTakeOverEvent
		{
			public int ButtonEventCount;
			public IntPtr buttonTakeOverEventInfo;
		}


		public struct ButtonTakeOverEventDetails
		{
			public int ButtonTypeKey;
			public string ButtonTypeValue;
			public int ButtonEventTypeSize;
			public ButtonTakeOverEventType[] buttonTakeOverEventType;
		}
		// Paired devices list structure
		[StructLayout(LayoutKind.Sequential)]
		public struct Jabra_PairedDevice
		{
			public string DeviceName;
			public string DeviceBTAddr;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct Jabra_PairingList
		{
			public ushort PairedDeviceCount;
			public DeviceListtype listType;
			public IntPtr PairedDeviceInfo;
		}

		public struct JabraDeviceInformation
		{
			public int DeviceId;
			public int ProductId;
			public string DeviceName;
			public string UsbDevicePath;
			public string ParentInstanceId;
			public JabraErrorStatus ErrorStatus;
			public bool IsDongleDevice;
			public string DongleName;
			public string Variant;
			public string SerialNumber;
		}
		public struct ListKeyValueNew
		{
			public int Key;
			public string Value;
			public int DependentCount;
			public DependencySetting[] DependencySettings;
		}

		public struct SettingDetails
		{
			public ListKeyValueNew[] ListKeyValues;
			public SettingInfo SettingInfo;
			public string CurrentValue;
			public string DependentDefaultValue;
			public ValidationRule ValidationRule;
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void FirstScanForDevicesDoneFunc();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void DeviceAttachedFunc(JabraDeviceInfo deviceInfo);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void DeviceRemovedFunc(ushort deviceId);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ButtonInDataRawHidFunc(ushort deviceId, ushort usagePage, ushort usage, [MarshalAs(UnmanagedType.I1)] bool buttonInData);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ButtonInDataTranslatedFunc(ushort deviceId, JabraHidInput translatedInData, [MarshalAs(UnmanagedType.I1)] bool buttonInData);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void BusylightFunc(ushort deviceId, [MarshalAs(UnmanagedType.I1)]bool busyLight);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void ButtonGNPEventFunc(ushort deviceId, IntPtr buttonTakeOverEvent);

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void PairingList(ushort deviceId, IntPtr ParingList);

		//----- Methods ------------------------

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_Initialize(
		[MarshalAs(UnmanagedType.FunctionPtr)] FirstScanForDevicesDoneFunc firstscanDoneCallbackPointer,
		[MarshalAs(UnmanagedType.FunctionPtr)] DeviceAttachedFunc deviceAttachedCallbackPointer,
		[MarshalAs(UnmanagedType.FunctionPtr)] DeviceRemovedFunc deviceDetachedCallbackPointer,
		[MarshalAs(UnmanagedType.FunctionPtr)] ButtonInDataRawHidFunc buttonInDataRawHidCallbackpointer,
		[MarshalAs(UnmanagedType.FunctionPtr)] ButtonInDataTranslatedFunc buttonInDataTranslatedCallbackpointer,
		uint instance);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsFirstScanForDevicesDone();

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsDeviceAttached(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern JabraReturnCode Jabra_GetBatteryStatus(ushort deviceId, ref int levelInPercent, ref bool charging, ref bool batteryLow);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern JabraReturnCode Jabra_GetFirmwareVersion(ushort deviceId, StringBuilder firmwareVersion, int count);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern JabraReturnCode Jabra_GetSerialNumber(ushort deviceId, StringBuilder serialNumber, int count);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr Jabra_GetSettings(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetSettings(ushort deviceId, IntPtr setting);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr Jabra_GetDeviceImagePath(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr Jabra_GetDeviceImageThumbnailPath(ushort deviceId);
		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsRingerSupported(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetRinger(ushort deviceId, [MarshalAs(UnmanagedType.I1)] bool ringer);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsMuteSupported(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetMute(ushort deviceId, [MarshalAs(UnmanagedType.I1)] bool mute);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsHoldSupported(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetHold(ushort deviceId, [MarshalAs(UnmanagedType.I1)] bool hold);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsOnlineSupported(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetOnline(ushort deviceId, [MarshalAs(UnmanagedType.I1)] bool audiolink);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsOffHookSupported(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetOffHook(ushort deviceId, [MarshalAs(UnmanagedType.I1)] bool offHook);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_Uninitialize();

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern JabraReturnCode Jabra_GetVersion(StringBuilder firmwareVersion, int count);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_FreeDeviceSettings(IntPtr setting);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_FreeString(IntPtr stringPtr);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern void Jabra_SetAppID(string appID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_FreeDeviceInfo(JabraDeviceInfo deviceInfo);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsFactoryResetSupported(ushort deviceId, [MarshalAs(UnmanagedType.I1)]bool bluetoothUI);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_FactoryReset(ushort deviceId, [MarshalAs(UnmanagedType.I1)]bool bluetoothUI);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_ConnectToJabraApplication(string guid, string softphoneName);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_DisconnectFromJabraApplication();

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern void Jabra_SetSoftphoneReady([MarshalAs(UnmanagedType.I1)]bool isReady);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsSoftphoneInFocus();

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetBTPairing(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_StopBTPairing(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetAutoPairing(ushort deviceId, bool value);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_GetAutoPairing(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_ClearPairingList(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_ConnectBTDevice(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_DisconnectBTDevice(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr Jabra_GetConnectedBTDeviceName(ushort deviceId);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_GetLock(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_ReleaseLock(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsLocked(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsBusylightSupported(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_GetBusylightStatus(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SetBusylightStatus(ushort deviceID, [MarshalAs(UnmanagedType.I1)]bool value);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_RegisterBusylightEvent([MarshalAs(UnmanagedType.FunctionPtr)] BusylightFunc busyLightCallbackPointer);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr Jabra_GetErrorString(JabraErrorStatus errStatus);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_GetButtonFocus(ushort deviceID, IntPtr buttonTakeOverEvent);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_ReleaseButtonFocus(ushort deviceID, IntPtr buttonTakeOverEvent);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern IntPtr Jabra_GetSupportedButtonEvents(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_FreeButtonEvents(IntPtr eventsSupported);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_RegisterForGNPButtonEvent([MarshalAs(UnmanagedType.FunctionPtr)] ButtonGNPEventFunc buttonGNPEventCallbackPointer);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsRemoteMMISupported(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Jabra_IsPairingListSupported(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr Jabra_GetPairingList(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_FreePairingList(IntPtr deviceList);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_ConnectPairedDevice(ushort deviceID, IntPtr pairedDevice);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_DisConnectPairedDevice(ushort deviceID, IntPtr pairedDevice);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_ClearPairedDevice(ushort deviceID, IntPtr pairedDevice);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_SearchNewDevices(ushort deviceID);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern JabraReturnCode Jabra_ConnectNewDevice(ushort deviceID, IntPtr pairedDevice);

		[DllImport("libjabra.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
		public static extern void Jabra_RegisterPairingListCallback([MarshalAs(UnmanagedType.FunctionPtr)] PairingList pairingListCallbackPointer);
	}
}
