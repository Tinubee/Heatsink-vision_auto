using System;
using System.Runtime.InteropServices;

namespace Euresys
{
    class clSerialException : Exception
    {
        public clSerialException(Int32 status, String error) : base(error) { Status = status; }
        public Int32 Status;
    }

    namespace clseremc
    {
        /// <summary>
        /// Class to expose the clseremc C API in .NET
        /// </summary>
        public sealed class CL
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            internal static extern Boolean SetDllDirectory(string lpPathName);

            /// <summary>
            /// Native functions imported from the clseremc C API.
            /// </summary>
            #region Native Methods
            class NativeMethods
            {
                private NativeMethods() { }

                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clFlushPort(IntPtr serialRef);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clGetErrorText(Int32 errorCode, IntPtr errortext, out UInt32 errorTextSize);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clGetNumSerialPorts(out UInt32 numPorts);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clGetNumBytesAvail(IntPtr serialRef, out UInt32 numBytes);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clGetSerialPortIdentifier(UInt32 serialIndex, IntPtr portID, out UInt32 bufferSize);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clGetManufacturerInfo(IntPtr manufacturer, out UInt32 bufferSize, out UInt32 version);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clGetSupportedBaudRates(IntPtr serialRef, out UInt32 baudRates);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clSerialClose(IntPtr serialRef);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clSerialInit(UInt32 serialIndex, out IntPtr serialRef);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clSerialRead(IntPtr serialRef, IntPtr buffer, out UInt32 numBytes, UInt32 serialTimeout);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clSerialWrite(IntPtr serialRef, String buffer, out UInt32 numBytes, UInt32 serialTimeout);
                [DllImport("clseremc.dll", CallingConvention = CallingConvention.Cdecl)]
                internal static extern Int32 clSetBaudRate(IntPtr serialRef, UInt32 baudRate);
            }
            #endregion

            #region Constructors
            private CL() { }
            #endregion

            #region Baud Rates
            public const UInt32 BAUDRATE_9600 = 1;
            public const UInt32 BAUDRATE_19200 = 2;
            public const UInt32 BAUDRATE_38400 = 4;
            public const UInt32 BAUDRATE_57600 = 8;
            public const UInt32 BAUDRATE_115200 = 16;
            public const UInt32 BAUDRATE_230400 = 32;
            public const UInt32 BAUDRATE_460800 = 64;
            public const UInt32 BAUDRATE_921600 = 128;
            #endregion

            #region Error codes
            public const Int32 ERR_NO_ERR = 0;
            public const Int32 ERR_BUFFER_TOO_SMALL = -10001;
            public const Int32 ERR_MANU_DOES_NOT_EXIST = -10002;
            public const Int32 ERR_PORT_IN_USE = -10003;
            public const Int32 ERR_TIMEOUT = -10004;
            public const Int32 ERR_INVALID_INDEX = -10005;
            public const Int32 ERR_INVALID_REFERENCE = -10006;
            public const Int32 ERR_ERROR_NOT_FOUND = -10007;
            public const Int32 ERR_BAUD_RATE_NOT_SUPPORTED = -10008;
            public const Int32 ERR_OUT_OF_MEMORY = -10009;
            public const Int32 ERR_REGISTRY_KEY_NOT_FOUND = -10010;
            public const Int32 ERR_INVALID_PTR = -10011;
            public const Int32 ERR_UNABLE_TO_LOAD_DLL = -10098;
            public const Int32 ERR_FUNCTION_NOT_FOUND = -10099;
            #endregion

            #region Error handling Methods
            private static void ThrowOnSerialError(Int32 status, String action)
            {
                if (status != 0)
                {
                    String error = action;
                    throw new Euresys.clSerialException(status, error);
                }
            }
            #endregion

            #region Serial 'operation' Methods
            public static void FlushPort(IntPtr serialRef)
            {
                ThrowOnSerialError(NativeMethods.clFlushPort(serialRef),
                    "Cannot flush port");
            }

            public static void SerialClose(IntPtr serialRef)
            {
                ThrowOnSerialError(NativeMethods.clSerialClose(serialRef),
                    "Cannot close port");
            }

            public static void SerialInit(UInt32 serialIndex, out IntPtr serialRef)
            {
                ThrowOnSerialError(NativeMethods.clSerialInit(serialIndex, out serialRef),
                    String.Format("Cannot initialize port (index = {0})", serialIndex));
            }

            public static void SerialRead(IntPtr serialRef, IntPtr buffer, out UInt32 numBytes, UInt32 serialTimeout)
            {
                ThrowOnSerialError(NativeMethods.clSerialRead(serialRef, buffer, out numBytes, serialTimeout),
                    "Cannot read port");
            }

            public static void SerialWrite(IntPtr serialRef, String buffer, out UInt32 numBytes, UInt32 serialTimeout)
            {
                ThrowOnSerialError(NativeMethods.clSerialWrite(serialRef, buffer, out numBytes, serialTimeout),
                    "Cannot write port");
            }

            #endregion

            #region Serial 'setter' Methods
            public static void SetBaudRate(IntPtr serialRef, UInt32 baudRate)
            {
                ThrowOnSerialError(NativeMethods.clSetBaudRate(serialRef, baudRate),
                    "Cannot set baud rate");
            }
            #endregion

            #region Serial 'getter' Methods
            public static void GetErrorText(Int32 errorCode, IntPtr errortext, out UInt32 errorTextSize)
            {
                ThrowOnSerialError(NativeMethods.clGetErrorText(errorCode, errortext, out errorTextSize),
                    "clGetErrorText error");
            }

            public static void GetNumSerialPorts(out UInt32 numPorts)
            {
                ThrowOnSerialError(NativeMethods.clGetNumSerialPorts(out numPorts),
                    "Cannot retrieve number of ports");
            }

            public static void GetSerialPortIdentifier(UInt32 serialIndex, IntPtr portID, out UInt32 bufferSize)
            {
                ThrowOnSerialError(NativeMethods.clGetSerialPortIdentifier(serialIndex, portID, out bufferSize),
                    "Cannot retrieve port identifier");
            }

            public static void GetManufacturerInfo(IntPtr manufacturer, out UInt32 bufferSize, out UInt32 version)
            {
                ThrowOnSerialError(NativeMethods.clGetManufacturerInfo(manufacturer, out bufferSize, out version),
                    "Cannot retrieve manufacturer info");
            }

            public static void GetNumBytesAvail(IntPtr serialRef, out UInt32 numBytes)
            {
                ThrowOnSerialError(NativeMethods.clGetNumBytesAvail(serialRef, out numBytes),
                    "Cannot retrieve number of bytes available");
            }

            public static void GetSupportedBaudRates(IntPtr serialRef, out UInt32 baudRates)
            {
                ThrowOnSerialError(NativeMethods.clGetSupportedBaudRates(serialRef, out baudRates),
                    "Cannot retrieve supported baud rates");
            }

            #endregion
        }
    }
}
