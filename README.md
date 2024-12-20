<h1 align="center">
Unity Borderless Toggle
</h1>
<h4 align="center">
A simple script to remove the title bar and make Unity games borderless on Windows platform.
</h4>
<p align="center">
  <img src="https://github.com/user-attachments/assets/afab3534-c459-4bfa-a89b-88b473e87649" alt="demo" />
</p>

## Setup
* Place `BorderlessToggle.cs` on a GameObject.
* Call `ToggleBorders()`, `SetBorderless()`, or `SetBordered()` on that script.

## Windows 11 rounded corners
This script opts in to rounded corners on Windows 11 by passing the value of `DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND` to the `DwmSetWindowAttribute` function. If not desired, comment out line 94 in the `SetBorderless()` function.
