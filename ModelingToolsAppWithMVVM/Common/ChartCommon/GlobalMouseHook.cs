﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ModelingToolsAppWithMVVM.Common.ChartCommon
{
    public delegate void delGlobalMouseMove(GlobalMouseArgs e);
    public delegate void delGlobalMouseUp(GlobalMouseArgs e);


    /// <summary>
    /// 捕捉全局鼠标动作
    /// </summary>
    public class GlobalMouseHook
    {
        public static event delGlobalMouseMove evtGlobalMouseMove;
        public static event delGlobalMouseUp evtGlobalMouseUp;
        
        //设置鼠标位置
        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);

        private LowLevelMouseProc _proc;
        private IntPtr _hookID = IntPtr.Zero;
        private MSLLHOOKSTRUCT hookStruct;
        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        public GlobalMouseHook()
        {
            pageLoad();
        }


        ~GlobalMouseHook()
        {
            Stop();
        }


        public void Stop()
        {
            bool retMouse = true;
            if ((int)_hookID != 0)
            {
                UnhookWindowsHookEx(_hookID);
                _hookID = IntPtr.Zero;
            }

            //如果卸下钩子失败
            if (!(retMouse)) throw new Exception("UnhookWindowsHookEx failed.");
        }

        private void pageLoad()
        {
            _hookID = SetHook(_proc);
        }

        private IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            {
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    _proc = HookCallback;
                    return SetWindowsHookEx(WH_MOUSE_LL, _proc, GetModuleHandle(curModule.ModuleName), 0);
                }
            }
        }


        int VK_CONTROL = 0x11;


        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE == (MouseMessages)wParam)
            {
                //hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                ////释放 
                //Marshal.FreeCoTaskMem(lParam); //labMouse.Content = hookStruct.pt.x + ", " + hookStruct.pt.y;

                //GlobalMouseMoveArgs e = new GlobalMouseMoveArgs();

                //int a = GetKeyState(VK_CONTROL);
                //int b = 0x8000;
                //if ((a & b) != 0)
                //{
                //    e.CtrlPressed = true;
                //}

                //e.Position = new Point(hookStruct.pt.x, hookStruct.pt.y);
                //e.ShiftPressed = false;
                //e.LeftBtnPressed = false;
                //e.RightBtnPressed = false;

                //if (null != evtGlobalMouseMove)
                //{
                //    evtGlobalMouseMove(e);
                //}
            }

            if (nCode >= 0 && MouseMessages.WM_LBUTTONUP == (MouseMessages)wParam)
            {
                GlobalMouseArgs e = new GlobalMouseArgs();
                e.Position = new Point(hookStruct.pt.x, hookStruct.pt.y);
                e.Button = MouseButton.Left;
                if (null != evtGlobalMouseUp)
                {
                    evtGlobalMouseUp(e);
                }
            }

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        private const int WH_MOUSE_LL = 14;
        /// <summary>
        /// 鼠标左键状态
        /// </summary>
        private bool LeftBtnPressed = false;
        /// <summary>
        /// 鼠标右键状态
        /// </summary>
        private bool RightBtnPressed = false;

        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205,
            MK_CONTROL = 0x0008
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT { public POINT pt; public uint mouseData; public uint flags; public uint time; public IntPtr dwExtraInfo; }

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName); private void Window_Closed(object sender, EventArgs e) { UnhookWindowsHookEx(_hookID); }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        private static extern int GetKeyState(int keyCode);
    }
}
