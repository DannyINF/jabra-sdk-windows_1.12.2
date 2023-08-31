using System;
using System.Collections.Generic;
using Jabra_SDK_Demo.Helpers;
using System.Linq;
using Jabra_SDK_Demo.ViewModel;
using System.Globalization;

namespace Jabra_SDK_Demo.Model
{
  public class RadioButtonControl : CommonControls
  {
    public string GroupName { get; set; }


    private int _currentValue;
    public int CurrentValue
    {
      get { return _currentValue; }
      set
      {
        _currentValue = value;
        OnPropertyChanged("CurrentValue");
      }
    }

    private bool _controlCreated;
    public bool ControlCreated
    {
      get { return _controlCreated; }
      set
      {
        _controlCreated = value;
        OnPropertyChanged("ControlCreated");
      }
    }


    private Dictionary<int, string> _settingValues;
    public Dictionary<int, string> SettingValues
    {
      get { return _settingValues; }
      set
      {
        _settingValues = value;
        OnPropertyChanged("SettingValues");
      }
    }

    private string _radioOn = string.Empty;
    public string RadioOn
    {
      get { return this._radioOn; }
      set
      {
        this._radioOn = value;
        this.OnPropertyChanged("RadioOn");
      }
    }

    private bool _radioOnIsCheck;
    public bool RadioOnIsCheck
    {
      get { return this._radioOnIsCheck; }
      set
      {
        this._radioOnIsCheck = value;
        AddChangedSetings();
        this.OnPropertyChanged("RadioOnIsCheck");
        this.OnPropertyChanged("TextValue");
      }
    }

    private bool _radioOffIsCheck;
    public bool RadioOffIsCheck
    {
      get { return this._radioOffIsCheck; }
      set
      {
        this._radioOffIsCheck = value;
        AddChangedSetings();
        this.OnPropertyChanged("RadioOffIsCheck");
        this.OnPropertyChanged("TextValue");
      }
    }

    private void AddChangedSetings()
    {
      if (SpecialHandlers.SettingsLoaded)
      {
        var data = (from set in SpecialHandlers.UnWrittenSettings
                    where set.Guid == GuidValue
                    select set).FirstOrDefault();
        if (data == null)
        {
          UnWrittenSettings settings = new UnWrittenSettings();
          settings.Guid = GuidValue;
          SpecialHandlers.UnWrittenSettings.Add(settings);
                    if (CommonMethods.SelectedDevice != null)
                    {
                        SpecialHandlers.UnWrittenSettingsDeviceId = CommonMethods.SelectedDevice.DeviceId;
                    }
                    else
                    {
                        SpecialHandlers.UnWrittenSettingsDeviceId = -1;
                    }
        }
        string selected = this.RadioOnIsCheck ? this.RadioOn : this.RadioOff;
        var key = (from val in SettingValues
                   where val.Value.ToLower() == selected.ToLower()
                   select val).SingleOrDefault();
        if (CommonMethods.SelectedDevice != null)
        {
            DependencyFeatures.UpdateDependentControls(CommonMethods.SelectedDevice.DeviceId, GuidValue, key.Key, MainWindowViewModel.ViewModelDeviceSettings.ExpanderControlList);
        }
        MainWindowViewModel.UpdateApplySettingsControl(false);
      }
    }

    private string _radioOff = string.Empty;
    public string RadioOff
    {
      get { return this._radioOff; }
      set
      {
        this._radioOff = value;
        this.OnPropertyChanged("RadioOff");
      }
    }

    public string TextValue
    {
      get
      {
        string selected = this.RadioOnIsCheck ? this.RadioOn : this.RadioOff;

        var key = (from val in SettingValues
                   where val.Value.ToLower() == selected.ToLower()
                   select val).SingleOrDefault();
        return string.Format("You have selected {0}", key.Key);
      }
    }

  }

  public struct SettingValues
  {
    public int key;
    public string value;
    public bool dependentFlag;
  }
}
