using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWin32
{
  public static class UnsafeWin32Code
  {
    // Importa a função SetupDiGetDeviceRegistryProperty da biblioteca setupapi.dll
    [DllImport("setupapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern bool SetupDiGetDeviceRegistryProperty(
        IntPtr deviceInfoSet,
        ref SP_DEVINFO_DATA deviceInfoData,
        uint property,
        out uint propertyRegDataType,
        IntPtr propertyBuffer,
        uint propertyBufferSize,
        out uint requiredSize
    );

    // Estrutura SP_DEVINFO_DATA para representar os dados do dispositivo
    [StructLayout(LayoutKind.Sequential)]
    public struct SP_DEVINFO_DATA
    {
      public uint cbSize;
      public Guid ClassGuid;
      public uint DevInst;
      public IntPtr Reserved;
    }

    // Constante para o identificador do dispositivo (SPDRP_HARDWAREID)
    private const uint SPDRP_HARDWAREID = 0x00000001;

    public static string GetDeviceIdentifier()
    {
      // Obtém o identificador do dispositivo
      IntPtr deviceInfoSet = IntPtr.Zero; // NÃO É SEGURO, apenas para fins ilustrativos
      SP_DEVINFO_DATA deviceInfoData = new SP_DEVINFO_DATA();

      deviceInfoData.cbSize = (uint)Marshal.SizeOf(deviceInfoData);

      uint requiredSize;
      IntPtr propertyBuffer = IntPtr.Zero;

      bool result = SetupDiGetDeviceRegistryProperty(
          deviceInfoSet,
          ref deviceInfoData,
          SPDRP_HARDWAREID,
          out uint propertyRegDataType,
          propertyBuffer,
          0,
          out requiredSize
      );

      if (!result && Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
      {
        propertyBuffer = Marshal.AllocHGlobal((int)requiredSize);

        result = SetupDiGetDeviceRegistryProperty(
            deviceInfoSet,
            ref deviceInfoData,
            SPDRP_HARDWAREID,
            out propertyRegDataType,
            propertyBuffer,
            requiredSize,
            out requiredSize
        );
      }

      if (result)
      {
        string deviceIdentifier = Marshal.PtrToStringUni(propertyBuffer);
        Marshal.FreeHGlobal(propertyBuffer);
        return deviceIdentifier;
      }

      return null;
    }

    private const int ERROR_INSUFFICIENT_BUFFER = 0x0000007A;
  }
}
