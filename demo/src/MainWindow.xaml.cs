using System;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using Jabra_SDK_Demo.ViewModel;
using JabraSDK;
using Jabra_SDK_Demo.Helpers;
using System.Collections.Generic;

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
            UpdateSpecificSettingToDevice(0);
            MessageBox.Show("ANC");
        }

        private void BtnSurround_Click(object sender, RoutedEventArgs e)
        {
            UpdateSpecificSettingToDevice(1);
            MessageBox.Show("Surround");
        }

        private void BtnSurroundMusic_Click(object sender, RoutedEventArgs e)
        {
            UpdateSpecificSettingToDevice(2);
            MessageBox.Show("Surround Music");
        }

        private void BtnOff_Click(object sender, RoutedEventArgs e)
        {
            UpdateSpecificSettingToDevice(3);
            MessageBox.Show("All features turned off");
        }

        public static void UpdateSpecificSettingToDevice(int dropdownState)
        {
            if (dropdownState < 0 || dropdownState > 3)
            {
                MessageBoxService.ShowMessage("Invalid dropdown state provided.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (MainWindowViewModel.AvailableDevices == null || !MainWindowViewModel.AvailableDevices.Any())
            {
                MessageBox.Show("No available devices.");
                return;
            }

            IDevice targetDevice = MainWindowViewModel.AvailableDevices.FirstOrDefault(device => device.DeviceId == device.DeviceId);

            if (targetDevice == null)
            {
                MessageBox.Show("No device found");
                return;
            }

            var deviceSettingsInfo = SpecialHandlers.SettingsInformation.FirstOrDefault(info => info.DeviceId == targetDevice.DeviceId);
            MessageBox.Show("Available Devices count: " + MainWindowViewModel.AvailableDevices.Count);
            MessageBox.Show("Settings Information count: " + SpecialHandlers.SettingsInformation.Count);


            if (deviceSettingsInfo == null)
            {
                MessageBox.Show("No settings information found for the target device.");
                return;
            }

            try
            {
                var targetSettingInformation = deviceSettingsInfo.DeviceSettings.FirstOrDefault(sett => sett.Guid.Equals(new Guid("9baf14e5-6f98-4d17-bac8-d657c15e83d0")));

                if (targetSettingInformation == null)
                {
                    MessageBox.Show("The specific setting information was not found.");
                    return;
                }

                targetSettingInformation.CurrentValue = dropdownState.ToString();
                List<ISetting> set = new List<ISetting> { targetSettingInformation };

                DeviceStatus ret = targetDevice.SetSettings(set);

                if (ret == DeviceStatus.ProtectedSettingWrite)
                {
                    MessageBoxService.ShowMessage("Failed to apply settings to the device. The setting is password protected on the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (ret != DeviceStatus.ReturnOk && ret != DeviceStatus.DeviceRebooted)
                {
                    MessageBoxService.ShowMessage("Failed to apply the setting to the device.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBoxService.ShowMessage("Exception while applying settings to the device\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
