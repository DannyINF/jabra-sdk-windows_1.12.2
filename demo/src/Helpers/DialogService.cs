using Jabra_SDK_Demo.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace Jabra_SDK_Demo.Helpers
{
  public static class DialogService
  {
    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll", SetLastError = true)]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
    static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
    private const int WM_CLOSE = 0x10;

    //public static DialogResult OpenFirmwareUpdateDialog()
    //{
    //  FirmwareUpdates win = new FirmwareUpdates();
    //  win.ShowDialog();
    //  return DialogResult.Undefined;
    //}

    public static void CloseDialogs()
    {
      try
      {
        Microsoft.VisualBasic.Interaction.AppActivate(System.Diagnostics.Process.GetCurrentProcess().Id);
        System.Windows.Forms.SendKeys.SendWait(" ");
      }
      catch { }

      try
      {
        IntPtr h = FindWindow("#32770", "Open");
        SendMessage(h, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
      }
      catch { }

      try
      {
        IntPtr h1 = FindWindow("#32770", "Save As");
        SendMessage(h1, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
      }
      catch { }
      try
      {
        IntPtr h1 = FindWindowByCaption(IntPtr.Zero, "Load Save Setting");
        SendMessage(h1, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
      }
      catch { }
      try
      {
        IntPtr h1 = FindWindowByCaption(IntPtr.Zero, "Information");
        SendMessage(h1, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
      }
      catch { }
    }
  }
}
