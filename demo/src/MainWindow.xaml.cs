using System;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;
using Jabra_SDK_Demo.ViewModel;
using JabraSDK;
using Jabra_SDK_Demo.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using JabraANC;

namespace Jabra_SDK_Demo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void BringToForeground()
        {
            if (this.WindowState == WindowState.Minimized || this.Visibility == Visibility.Hidden)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }

            this.Activate();
            this.Topmost = true;
            this.Topmost = false;
            this.Focus();
        }

        private void BtnANC_Click(object sender, RoutedEventArgs e)
        {
            USSTD(0);
            MessageBox.Show("OFF");
        }

        private void BtnSurround_Click(object sender, RoutedEventArgs e)
        {
            USSTD(1);
            MessageBox.Show("ANC");
        }

        private void BtnSurroundMusic_Click(object sender, RoutedEventArgs e)
        {
            USSTD(2);
            DisplayAllSettingGuidsAndDescriptions();
            MessageBox.Show("Surround");
        }

        private void BtnOff_Click(object sender, RoutedEventArgs e)
        {
            //DisplayAllSettingGuidsAndDescriptions();
            USSTD(3);
            MessageBox.Show("Surround Music");
        }

        public static void DisplayAllSettingGuidsAndDescriptions()
{
    // Retrieve the device.
    IDevice targetDevice = MainWindowViewModel.AvailableDevices.FirstOrDefault(); 
            

    if (targetDevice != null)
    {
        try
        {
            // Get all settings from the device
            List<ISetting> allSettings = targetDevice.GetSettings();

            // Create a string builder to accumulate the information
            StringBuilder sb = new StringBuilder();

            foreach (var setting in allSettings)
            {
                Debug.WriteLine($"GUID: {setting.Guid} - Setting Name: {setting.SettingName}");
            }

            // Display the accumulated information
        }
        catch (Exception ex)
        {
            MessageBox.Show("Exception while fetching settings information: " + ex.Message);
        }
    }
    else
    {
        MessageBox.Show("No connected Jabra device found.");
    }
}


public static void USSTD(int desiredState)
{
    
    // Retrieve the device.
    IDevice targetDevice = MainWindowViewModel.AvailableDevices.FirstOrDefault();
    ManipulateJabra.UpdateSpecificSettingToDevice("9BAF14E5-6F98-4D17-BAC8-D657C15E83D0", desiredState, targetDevice);  
            /*
    if (targetDevice != null)
    {
        try
        {
            // Get all settings from the device
            List<ISetting> allSettings = targetDevice.GetSettings();

            // Locate the setting with the desired GUID
            ISetting specificSetting = allSettings.FirstOrDefault(s => s.Guid == "9BAF14E5-6F98-4D17-BAC8-D657C15E83D0");

            if (specificSetting == null)
            {
                MessageBox.Show("The specified GUID does not exist for this device.");
                return;
            }

            // Update the CurrentValue of the located setting
            specificSetting.CurrentValue = desiredState.ToString();

            // Apply the updated setting to the device
            DeviceStatus status = targetDevice.SetSettings(new List<ISetting>() { specificSetting });

            if (status != DeviceStatus.ReturnOk)
            {
                // Handle any errors or issues
                MessageBox.Show($"Error updating settings. Device status: {status}");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Exception while updating settings: " + ex.Message);
        }
    }
    else
    {
        MessageBox.Show("No connected Jabra device found.");
    }*/
}







    }

    // Lightweight implementation of the ISetting interface just for updating settings
    public class SimpleSetting : ISetting
    {
        public string Guid { get; set; }
        public IValidations ValidationRules { get; }
        public bool IsValidationSupported { get; }
        public string CurrentValue { get; set; }
        public List<ISettingValues> SettingValues { get; }
        public bool IsDepedentSettingSupported { get; }
        public bool IsPcSetting { get; }
        public bool IsSettingProtected { get; }
        public bool IsWirelessConnected { get; }
        public string GroupHelpText { get; }
        public string GroupName { get; }
        public DataType SettingDataType { get; }
        public ControlType ControlType { get; }
        public string HelpText { get; }
        public string SettingName { get; }
        public bool IsDeviceRestartRequired { get; }
        public bool IsSettingProtectionEnabled { get; }
    }
}
