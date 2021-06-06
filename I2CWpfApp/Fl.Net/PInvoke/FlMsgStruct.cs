using System;
using System.Runtime.InteropServices;

namespace Fl.Net.PInvoke
{
    // https://stackoverflow.com/questions/7970128/passing-a-c-sharp-callback-function-through-interop-pinvoke
    public delegate void fl_msg_cb_on_parsed_t(IntPtr parser_handle, IntPtr context);
    public delegate void fl_msg_dbg_cb_on_parse_started_t(IntPtr parser_handle);
    public delegate void fl_msg_dbg_cb_on_parse_ended_t(IntPtr parser_handle);

    #region Common Message
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public unsafe struct FlHwVerStruct
    {
        //public fixed byte version[(int)FlConstant.FL_VER_STR_MAX_LEN];
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)FlConstant.FL_VER_STR_MAX_LEN)]
        public string version;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public unsafe struct FlFwVerStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = (int)FlConstant.FL_VER_STR_MAX_LEN)]
        public string version;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlGpiPortStruct
    {
        public byte port_num;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlGpiPortValueStruct
    {
        public byte port_num;
        public byte port_value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlBtnStatusStruct
    {
        public byte button_num;
        public byte button_value;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlSensorStruct
    {
        public byte sensor_num;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlTempSensorReadStruct
    {
        public byte sensor_num;
        public double temperature;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlHumSensorReadStruct
    {
        public byte sensor_num;
        public double humidity;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlTempHumSensorReadStruct
    {
        public byte sensor_num;
        public double temperature;
        public double humidity;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct FlBootModeStruct
    {
        public byte boot_mode;
    }
    #endregion
}
