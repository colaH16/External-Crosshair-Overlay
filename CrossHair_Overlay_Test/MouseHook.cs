using System;
using System.Diagnostics;
using System.Windows.Input;
using System.Runtime.InteropServices;

using System.Linq;
using System.Windows;
using System.Threading;
using System.Windows.Media;
using System.Collections.Generic;
using External_Crosshair_Overlay.Config;



namespace External_Crosshair_Overlay

{

    public  class MouseHook

    {

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]

        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);



        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]

        public static extern bool UnhookWindowsHookEx(int idHook);



        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]

        public static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);



        public delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);

        /*
        [StructLayout(LayoutKind.Sequential)]

        public class POINT

        {

            public int x;

            public int y;

        }
        */

        [StructLayout(LayoutKind.Sequential)]

        public class MouseHookStruct

        {

            //public POINT pt;

            public int hwnd;

            public int wHitTestCode;

            public int dwExtraInfo;

        }

        public const int WH_MOUSE_LL = 14;



        public static int hookHandle = 0;

        private static HookProc callbackDelegate;

        public static OverlayCrosshair mcrosshairOverlayWindow;
        public static double transparent = 0.8;
        public double SetTransparency
        {
            set
            {
                transparent = value;
            }
        }
        public  void StartHook(OverlayCrosshair crosshairOverlayWindow, double transparent)

        {

            callbackDelegate = new HookProc(CallBack);

            if (hookHandle != 0)

            {

                return;

            }

            hookHandle = MouseHook.SetWindowsHookEx(WH_MOUSE_LL, callbackDelegate, IntPtr.Zero, 0);
            mcrosshairOverlayWindow = crosshairOverlayWindow;
            SetTransparency = transparent;
    }

        public static void StopHook()

        {

           MouseHook.UnhookWindowsHookEx(hookHandle);

        }

        public static int CallBack(int nCode, IntPtr wParam, IntPtr lParam)

        {
            /*
            mcrosshairOverlayWindow.ToggleOverlayOpacity();
            Int32 wwParam32 = wParam.ToInt32();
            Int64 wwParam64 = wParam.ToInt64();
            
            */
            if ( wParam.ToInt32() == 516 || wParam.ToInt32() == 523)
            {
                mcrosshairOverlayWindow.SetCrosshairTransparency = 0.0;
            }

            if (wParam.ToInt32() == 517 || wParam.ToInt32() == 524)
            {
                mcrosshairOverlayWindow.SetCrosshairTransparency = transparent;
            }
            ㅁ
            return MouseHook.CallNextHookEx(hookHandle, nCode, wParam, lParam);

        }

    }
}