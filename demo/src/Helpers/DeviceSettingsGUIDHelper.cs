using JabraSDK;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Jabra_SDK_Demo.Helpers
{
  /// <summary>
  /// Helper class for setting GUID's
  /// </summary>
  static class DeviceSettingsGUIDHelper
  {
    // Ringtone type settings
    private static string SoftphoneBaseRingtoneGuid = "416A1687-5A48-4581-8025-3726D59422CE";
    private static string DeskPhoneBaseRingtoneGuid = "539FF55C-ED5A-4059-A75B-AF82461226BB";
    private static string USBDeskPhoneBaseRingtoneGuid = "2995336A-F74B-4E2B-A0CC-D12503C83310";
    private static string MobilePhoneBaseRingtoneGuid = "AC817C70-DE4C-42C9-8036-5CEDB91A1C2B";

    // Ringtone volume settings
    private static string SoftphoneBaseRingtoneVolumeGuid = "41B1B690-3CE5-43AD-B002-8021952E87A3";
    private static string DeskPhoneBaseRingtoneVolumeGuid = "363EF7FD-D726-4816-9922-3A163D33FA51";
    private static string USBDeskPhoneBaseRingtoneVolumeGuid = "AE5C9DFE-BA7C-45D4-A07B-811ABE021FB4";
    private static string MobilePhoneBaseRingtonVolumeGuid = "EFED2EC6-0DC5-450A-B0C1-9C920CE8CB07";

    /// <summary>
    /// Make ringtone type and volume settings pair for playing ringtone
    /// </summary>
    public static Dictionary<string, string> PlayRingtoneGuidPair => new Dictionary<string, string>
                                                                    {
                                                                      { SoftphoneBaseRingtoneGuid, SoftphoneBaseRingtoneVolumeGuid },
                                                                      { DeskPhoneBaseRingtoneGuid, DeskPhoneBaseRingtoneVolumeGuid },
                                                                      { USBDeskPhoneBaseRingtoneGuid, USBDeskPhoneBaseRingtoneVolumeGuid },
                                                                      { MobilePhoneBaseRingtoneGuid, MobilePhoneBaseRingtonVolumeGuid }
                                                                    };

    /// <summary>
    /// Provide setting details based on guid for current device
    /// </summary>
    /// <param name="guid">Setting GUID</param>
    /// <returns>Setting information</returns>
    public static ISetting GetSettingInformationByGuid(string guid)
    {
      ISetting settingInfo = null;
      if (!string.IsNullOrEmpty(guid))
      {
        ObservableCollection<SettingsInformation> deviceSettingsInfo = SpecialHandlers.SettingsInformation;
        settingInfo = (from devSettingInfo in deviceSettingsInfo
                       where devSettingInfo.DeviceId == CommonMethods.SelectedDevice.DeviceId
                       from setting in devSettingInfo.DeviceSettings
                       where setting.Guid.ToLower() == guid.ToString().ToLower()
                       select setting).FirstOrDefault();
      }
      return settingInfo;
    }
  }
}
