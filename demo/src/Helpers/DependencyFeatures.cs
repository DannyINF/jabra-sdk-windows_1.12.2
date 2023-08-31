using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Jabra_SDK_Demo.ViewModel;
using JabraSDK;
using System.Collections.Generic;
using System.Windows;

namespace Jabra_SDK_Demo.Helpers
{
  public static class DependencyFeatures
    {
        // need to keep track of temporary UI-selected values (for settings that can control UI access to others)
        // outer key is deviceId, inner key is setting Guid, value is the current value.
        internal static Dictionary<int, Dictionary<Guid, int>> currentUiValues = new Dictionary<int, Dictionary<Guid, int>>();
        // lookup controlling setting from controlled setting
        // outer key is deviceId, inner key is Guid of controlled setting, value is Guids of controlling settings.
        internal static Dictionary<int, Dictionary<Guid, HashSet<Guid>>> controllingSettings = new Dictionary<int, Dictionary<Guid, HashSet<Guid>>>();

        // called when a control is changed - update state of controls that depend on this one
        internal static void UpdateDependentControls(int deviceId, Guid guid, int currentValue, ObservableCollection<ExpanderControlViewModel> expanderControlList, bool fromUi=true)
        {
            ObservableCollection<SettingsInformation> settingsInformations = SpecialHandlers.SettingsInformation;
            try
            {
                var thisSetting = (from sett in settingsInformations
                                   where sett.DeviceId == deviceId
                                   from setting in sett.DeviceSettings
                                   where setting.Guid.ToLower(CultureInfo.InvariantCulture) == guid.ToString().ToLower(CultureInfo.InvariantCulture)
                                   select setting).FirstOrDefault();

                if(thisSetting != null && thisSetting.IsDepedentSettingSupported)
                {
                    currentUiValues[deviceId][guid] = currentValue;

                    // iterate over all relevant controlled settings to find out how controlling settings are affecting them - we may have multiple controllers per controlled setting
                    var settingsAffectedByThisOne = (from lst in thisSetting.SettingValues
                                               where lst.Key == currentValue
                                               select lst.DependencySettings).FirstOrDefault();
                    // actually, the only relevant setting to investigate is the ones that depends on this one (from XML, there should be only one, but...)
                    foreach(var affectedSetting in settingsAffectedByThisOne)
                    {
                        var controlledG = Guid.Parse(affectedSetting.Guid);
                        var controllers = controllingSettings[deviceId][controlledG];
                        if(controllers != null)
                        {
                            // someone wants to control controlledG
                            bool enableControlledSetting = true;

                            foreach (Guid controllingG in controllers)
                            {
                                var controllingValue = currentUiValues[deviceId][controllingG];
                                // look up definition for controllingG to see how /it/ wants to affect controlledG with its current value
                                var controllingSetting = (from sett in settingsInformations
                                                          where sett.DeviceId == deviceId
                                                          from devSet in sett.DeviceSettings
                                                          where devSet.Guid.ToLower(CultureInfo.InvariantCulture) == controllingG.ToString().ToLower(CultureInfo.InvariantCulture)
                                                          select devSet).FirstOrDefault();
                                if (controllingSetting != null) {
                                    var controlledValueInfo = (from lst in controllingSetting.SettingValues
                                                               where lst.Key == controllingValue
                                                               select lst.DependencySettings).FirstOrDefault();
                                    if (controlledValueInfo != null)
                                    {
                                        enableControlledSetting &= controlledValueInfo.ElementAt(0).EnableFlag;
                                    }
                                }
                            }

                            // update controlled UI element.
                            ComboBoxControlViewModel uiComboBoxSetting = UserControlDetails.GetComboBoxControlInformations(controlledG, expanderControlList);
                            if (uiComboBoxSetting != null)
                            {                                
                                uiComboBoxSetting.ComboBoxControl.ControlEnabled = uiComboBoxSetting.ComboBoxControl.Protected ? false : enableControlledSetting;
                            }
                            else
                            {
                                RadioButtonControlViewModel uiRadioButtonSetting = UserControlDetails.GetRadioButtonControlInformations(controlledG, expanderControlList);
                                if (uiRadioButtonSetting != null)
                                {                                    
                                    uiRadioButtonSetting.RadioButtonControl.ControlEnabled = uiRadioButtonSetting.RadioButtonControl.Protected ? false : enableControlledSetting;

                                    // Add dependent radio buttons to unwrittensettings 
                                    // TODO: (why treat them specially ??)
                                    if (fromUi && !enableControlledSetting)
                                    {
                                        var pendingValue = (from set in SpecialHandlers.UnWrittenSettings
                                                    where set.Guid == controlledG
                                                    select set).FirstOrDefault();
                                        if (!uiRadioButtonSetting.RadioButtonControl.ControlEnabled)
                                        {
                                            if (pendingValue == null)
                                            {
                                                UnWrittenSettings settings = new UnWrittenSettings();
                                                settings.Guid = controlledG;
                                                SpecialHandlers.UnWrittenSettings.Add(settings);
                                            }
                                        }
                                        else
                                        {
                                            if (pendingValue != null)
                                                SpecialHandlers.UnWrittenSettings.Remove(pendingValue);
                                        }
                                    }
                                }
                                else
                                {
                                    TextControlViewModel uiTextSetting = UserControlDetails.GetTextControlInformations(controlledG, expanderControlList);
                                    if (uiTextSetting != null)
                                    {
                                        uiTextSetting.TextControl.ControlEnabled = uiTextSetting.TextControl.Protected ? false : enableControlledSetting;
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessage($"Exception while Set Dependency" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        internal static void SetupControlDependencies(int deviceId, List<ISetting> settings, ObservableCollection<ExpanderControlViewModel> expanderControlList)
        {
            currentUiValues.Clear();
            controllingSettings.Clear();

            // pass one, build data structures 
            foreach (var controllingSetting in settings)
            {
                if (controllingSetting.IsDepedentSettingSupported)
                {
                    switch (controllingSetting.SettingDataType)
                    {
                        case DataType.Byte:
                            var controlledSettings = (from lst in controllingSetting.SettingValues
                                      where lst.Key == Convert.ToInt32(controllingSetting.CurrentValue)
                                      select lst.DependencySettings).FirstOrDefault();
                            Guid controllingG = Guid.Parse(controllingSetting.Guid);
                            if(!currentUiValues.ContainsKey(deviceId))
                                currentUiValues[deviceId] = new Dictionary<Guid, int>();
                            currentUiValues[deviceId][controllingG] = Convert.ToInt32(controllingSetting.CurrentValue); // initial state

                            if (controlledSettings != null)
                            {
                                foreach (var setting in controlledSettings)
                                {
                                    Guid g = Guid.Parse(setting.Guid);
                                    if (!controllingSettings.ContainsKey(deviceId))
                                        controllingSettings[deviceId] = new Dictionary<Guid, HashSet<Guid>>();
                                    if (!controllingSettings[deviceId].ContainsKey(g))
                                        controllingSettings[deviceId][g] = new HashSet<Guid>();
                                    controllingSettings[deviceId][g].Add(controllingG);
                                }
                            }
                            break;
                    }
                }
            }

            foreach(var controllingSetting in settings)
            {
                Guid g = Guid.Parse(controllingSetting.Guid);
                if (currentUiValues.ContainsKey(deviceId) && currentUiValues[deviceId].ContainsKey(g))
                {
                    UpdateDependentControls(deviceId, g, currentUiValues[deviceId][g], expanderControlList, false);
                }
            }
        }
    }
}
