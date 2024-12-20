using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class BorderlessToggle : MonoBehaviour
{
    [SerializeField] private bool borderless = false;
    private Vector2Int borderSize;
    private int windowPosX, windowPosY;

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

    [DllImport("user32.dll")]
    private static extern IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int width, int height, uint uFlags);

    [DllImport("user32.dll")]
    private static extern bool GetWindowRect(IntPtr hWnd, ref Rect rect);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("dwmapi.dll", PreserveSig = false)]
    private static extern void DwmSetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, ref int pvAttribute, uint cbAttribute);

    private enum DWMWINDOWATTRIBUTE : uint
    {
        DWMWA_WINDOW_CORNER_PREFERENCE = 33
    }

    private enum DWM_WINDOW_CORNER_PREFERENCE : uint
    {
        DWMWCP_ROUND = 2
    }

    private const int GWL_STYLE = -16;
    private const int WS_OVERLAPPEDWINDOW = 0x00CF0000;

    private struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;

        public int Width() { return Right - Left; }
        public int Height() { return Bottom - Top; }
    }

    public void Start()
    {
        borderSize.x = GetSystemMetrics(32);
        borderSize.y = GetSystemMetrics(33);

        if (borderless)
            SetBorderless();
    }

    public void ToggleBorders()
    {
        borderless = !borderless;
        if (borderless)
            SetBorderless();
        else
            SetBordered();
    }

    public void SetBorderless()
    {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        IntPtr hwnd = GetActiveWindow();
        Rect currentRect = new Rect();
        GetWindowRect(hwnd, ref currentRect);

        windowPosX = currentRect.Left;
        windowPosY = currentRect.Top;

        IntPtr currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
        IntPtr newStyle = new IntPtr(currentStyle.ToInt32() & ~WS_OVERLAPPEDWINDOW);
        SetWindowLongPtr(hwnd, GWL_STYLE, newStyle);

        SetWindowPos(hwnd, IntPtr.Zero, windowPosX, windowPosY, currentRect.Width(), currentRect.Height(), 0x0020);

        int preference = (int)DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
        DwmSetWindowAttribute(hwnd, DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE, ref preference, (uint)Marshal.SizeOf(typeof(int)));

        borderless = true;
#endif
    }

    public void SetBordered()
    {
#if !UNITY_EDITOR && UNITY_STANDALONE_WIN
        IntPtr hwnd = GetActiveWindow();
        Rect currentRect = new Rect();
        GetWindowRect(hwnd, ref currentRect);

        windowPosX = currentRect.Left;
        windowPosY = currentRect.Top;

        IntPtr currentStyle = GetWindowLongPtr(hwnd, GWL_STYLE);
        IntPtr newStyle = new IntPtr(currentStyle.ToInt32() | WS_OVERLAPPEDWINDOW);
        SetWindowLongPtr(hwnd, GWL_STYLE, newStyle);

        SetWindowPos(hwnd, IntPtr.Zero, windowPosX, windowPosY, currentRect.Width(), currentRect.Height(), 0x0020);

        borderless = false;
#endif
    }
}
