using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;
using JabraSDK;
using System.Drawing;
using System.Drawing.Imaging;
using Jabra_SDK_Demo.Properties;

namespace Jabra_SDK_Demo.Helpers
{
  public static class CommonMethods
  {
    public const string DeviceRemoved = "Device is removed.";
    public const string FunctionalityNotSupported = "Functionality not supported in current version.";

    public static string DemoAppId { get; set; }
    public static string AuthorizationTokenError { get; set; }
    public static string Password { get; set; }
    public static IDevice SelectedDevice { get; set; }
    public static string ApplicationName { get; set; }

    public static List<string> GetBatteryStatus(IDevice device)
    {
      List<string> batteryStatusList = new List<string>();
      string batteryDetails = string.Empty;
      try
      {
        if (device.IsBatteryStatusSupported)
        {
          BatteryStatus batteryStatus = device.GetBatteryStatus();
          if (batteryStatus != null)
          {
            batteryDetails = batteryStatus.LevelInPercent.ToString() + " (" + batteryStatus.Component.ToString() + ")";
            if (batteryStatus.Charging)
              batteryDetails += " charging";
            if (batteryStatus.BatteryLow)
              batteryDetails += " low battery";

            batteryStatusList.Add(batteryDetails);
            if (batteryStatus.ExtraUnits != null && batteryStatus.ExtraUnits.Count > 0)
            {
              string extraUnits = string.Empty;
              foreach (var bsUnit in batteryStatus.ExtraUnits)
              {
                extraUnits += bsUnit.LevelInPercent.ToString() + " (" + bsUnit.Component.ToString() + ") ";
              }
              batteryStatusList.Add(extraUnits);
            }
          }
        }
      }
      catch
      {
      }
      return batteryStatusList;
    }

    public static ImageSource SetImageSource(string path)
    {
      string imagePath = path;
      ImageSource imageSource = null;
      bool relativePath = false;
      try
      {
        imageSource = string.IsNullOrEmpty(imagePath) ? Resources.Default_380x380.ToWpfBitmap() : LoadBitmapImage(imagePath, relativePath);
      }
      catch (Exception )
      {
        //added to handle devices that is not supported
        try
        {
          imageSource = Resources.Default_380x380.ToWpfBitmap();
        }
        catch (Exception exx)
        {
          MessageBoxService.ShowMessage("Exception occurred during file download\n" + exx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
      return imageSource;
    }

    public static ImageSource SetThumbnailImageSource(string path)
    {
      string thumbnailImagePath = path;
      ImageSource imageSource = null;
      bool relativePath = false;
      try
      {
        imageSource = string.IsNullOrEmpty(path) ? Resources.Default_80x80.ToWpfBitmap() : LoadBitmapImage(path, relativePath);
      }
      catch (Exception )
      {
        //added to handle devices that is not supported
        try
        {
          imageSource = Resources.Default_80x80.ToWpfBitmap();
        }
        catch (Exception exx)
        {
          MessageBoxService.ShowMessage("Exception occurred during file download\n" + exx.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
      return imageSource;
    }

    public static string ConvertToHex(int value, string type)
    {
      string message;
      switch (type.ToLower(CultureInfo.InvariantCulture))
      {
        case "x2":
          message = string.Format("0x{0:X2}", value);
          break;
        case "x4":
          message = string.Format("0x{0:X4}", value);
          break;
        default:
          message = string.Format("0x{0:X}", value);
          break;
      }
      return message;
    }

    //todo check
    //public static void SetDemoAppId(string demoAppId)   
    //{
    //	NativeMethods.Jabra_SetAppID(demoAppId);
    //}

    public static void SetEnvironmentVariables()
    {
      Dictionary<string, string> applicationSettings = new Dictionary<string, string>();
      // Set values for environmental variables
      applicationSettings.Add("LIBJABRA_TRACE_LEVEL", "info");// Provides level of logging (Possible values: "debug", "fatal", "error", "warning", "info", "trace")
                                                              //applicationSettings.Add("LIBJABRA_ALLOW_ONLINE", "true");// Allows online access for fetching serial number from cloud server(Possible values: "true", "1", "yes")
                                                              //applicationSettings.Add("LIBJABRA_RESOURCE_PATH", "C:\\SDKApp");// User configurable path for downloading resources, creating log file and db. If path is not provided, default location will be "Appdata".

      foreach (KeyValuePair<string, string> environmentVariable in applicationSettings)
      {
        try
        {
          // Get the read permission to fetch the environment variable.
          EnvironmentPermission permissions = new EnvironmentPermission(EnvironmentPermissionAccess.Read, environmentVariable.Key);
          permissions.Demand();
          string variableValue = Environment.GetEnvironmentVariable(environmentVariable.Key, EnvironmentVariableTarget.User);

          // Add the environment variable if not set
          if (string.IsNullOrEmpty(variableValue))
          {
            // Get the write permission to set the environment variable.
            permissions = new EnvironmentPermission(EnvironmentPermissionAccess.Write, environmentVariable.Key);
            permissions.Demand();
            Environment.SetEnvironmentVariable(environmentVariable.Key, environmentVariable.Value, EnvironmentVariableTarget.User);
          }
        }
        catch (SecurityException ex)
        {
          MessageBoxService.ShowMessage($"Error while setting environment variable {environmentVariable.Key} : Message {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
          MessageBoxService.ShowMessage($"Error while setting environment variable : Message {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
      }
    }

    private static BitmapImage LoadBitmapImage(string filepath, bool isRelativePath)
    {
      //Load BitmapImage from file to memory stream and immediately close the file after loading.				
      byte[] buffer;
      if (isRelativePath)
      {
        using (var stream = Application.GetResourceStream(new Uri(filepath)).Stream)
        {
          buffer = new byte[stream.Length];
          stream.Read(buffer, 0, buffer.Length);
        }
      }
      else
      {
        buffer = File.ReadAllBytes(filepath);
      }

      using (var byteStream = new MemoryStream(buffer))
      {
        var bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.StreamSource = byteStream;
        bitmapImage.EndInit();
        bitmapImage.Freeze();
        return bitmapImage;
      }
    }

    public static BitmapSource ToWpfBitmap(this Bitmap bitmap)
    {
      using (MemoryStream stream = new MemoryStream())
      {
        bitmap.Save(stream, ImageFormat.Bmp);

        stream.Position = 0;
        BitmapImage result = new BitmapImage();
        result.BeginInit();
        // According to MSDN, "The default OnDemand cache option retains access to the stream until the image is needed."
        // Force the bitmap to load right now so we can dispose the stream.
        result.CacheOption = BitmapCacheOption.OnLoad;
        result.StreamSource = stream;
        result.EndInit();
        result.Freeze();
        return result;
      }
    }

  }
}
