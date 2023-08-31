using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Jabra_SDK_Demo.Helpers
{
  /// <summary>
  ///   Contains helper methods for UI, so far just one for showing a waitcursor
  /// </summary>
  public static class UiServices
  {
    public static DispatcherTimer SpinTimer { get; set; }


    public static void SetBusyState(int duration, bool isBlockUi, bool isConnectDisconnectUi, bool isResetDevice, bool isShowUiSpinner)
    {
      SpecialHandlers.ShowSpinner = isBlockUi;
      SpecialHandlers.ShowUiConnectSpinner = isConnectDisconnectUi;
      SpecialHandlers.ShowUiResetSpinner = isResetDevice;
      SpecialHandlers.ShowUiSpinner = isShowUiSpinner;
      SpinTimer = new DispatcherTimer(TimeSpan.FromSeconds(duration), DispatcherPriority.ApplicationIdle, dispatcherTimer_Tick, Application.Current.Dispatcher);
    }

    /// <summary>
    /// Handles the Tick event of the dispatcherTimer control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private static void dispatcherTimer_Tick(object sender, EventArgs e)
    {
      SpecialHandlers.ShowUiSpinner = SpecialHandlers.ShowSpinner = SpecialHandlers.ShowUiConnectSpinner = SpecialHandlers.ShowUiResetSpinner = false;
      SpinTimer?.Stop();
    }
  }
}
