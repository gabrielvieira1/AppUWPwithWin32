using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWin32
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var result = UnsafeWin32Code.GetDeviceIdentifier();
      Console.WriteLine("Result " + result);
    }
  }
}
