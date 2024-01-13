using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppUWPwithWin32
{
  /// <summary>
  /// An empty page that can be used on its own or navigated to within a Frame.
  /// </summary>
  public sealed partial class MainPage : Page
  {
    private AppServiceConnection deviceIdService;

    public MainPage()
    {
      this.InitializeComponent();
    }

    private async void btnClick_Click(object sender, RoutedEventArgs e)
    {

      if (this.deviceIdService == null)
      {
        deviceIdService = new AppServiceConnection();

        this.deviceIdService.AppServiceName = "com.microsoft.GetDeviceID";

        var status = await this.deviceIdService.OpenAsync();

        if (status != AppServiceConnectionStatus.Success)
        {
          textBox.Text = "Failed to connect";
          this.deviceIdService = null;
          return;
        }
      }

      // Call the service.
      var message = new ValueSet();
      message.Add("Command", "GetId");
      AppServiceResponse response = await this.deviceIdService.SendMessageAsync(message);
      string result = "";


      if (response.Status == AppServiceResponseStatus.Success)
      {
        // Get the data that the service sent to us.
        if (response.Message["Status"] as string == "OK")
        {
          result = response.Message["Result"] as string;
        }
        else
        {
          result = "Failed Get device Id";
        }
      }

      textBox.Text = result;
    }
  }
}
