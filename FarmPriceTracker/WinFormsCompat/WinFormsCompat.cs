using System;
using System.Windows;
using System.Windows.Interop;
using IWin32Window = System.Windows.Forms.IWin32Window;

namespace FarmPriceTracker.WinFormsCompat;

public static class WinFormsCompat {
  public static IWin32Window GetIWin32Window(this Window window) =>
    new Win32Window(new WindowInteropHelper(window).Handle);

  private class Win32Window : IWin32Window {
    public Win32Window(IntPtr handle) {
      Handle = handle;
    }

    public IntPtr Handle { get; }
  }
}
