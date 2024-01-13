using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Collections;
using Windows.Storage;

namespace MyAppService
{
  public sealed class GetDeviceID : IBackgroundTask
  {
    private BackgroundTaskDeferral backgroundTaskDeferral;
    private AppServiceConnection appServiceconnection;

    public void Run(IBackgroundTaskInstance taskInstance)
    {
      // Get a deferral so that the service isn't terminated.
      this.backgroundTaskDeferral = taskInstance.GetDeferral();

      // Associate a cancellation handler with the background task.
      taskInstance.Canceled += OnTaskCanceled;

      // Retrieve the app service connection and set up a listener for incoming app service requests.
      var details = taskInstance.TriggerDetails as AppServiceTriggerDetails;
      appServiceconnection = details.AppServiceConnection;
      appServiceconnection.RequestReceived += OnRequestReceived;
    }

    private async void OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
    {

      // Get a deferral because we use an awaitable API below to respond to the message
      // and we don't want this call to get canceled while we are waiting.
      var messageDeferral = args.GetDeferral();

      ValueSet message = args.Request.Message;
      ValueSet returnData = new ValueSet();

      string command = message["Command"] as string;
      int? inventoryIndex = message["ID"] as int?;

      // Return the data to the caller.
      await args.Request.SendResponseAsync(returnData);

      // Complete the deferral so that the platform knows that we're done responding to the app service call.
      // Note for error handling: this must be called even if SendResponseAsync() throws an exception.
      messageDeferral.Complete();
    }


    private void OnTaskCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
    {
      if (this.backgroundTaskDeferral != null)
      {
        // Complete the service deferral.
        this.backgroundTaskDeferral.Complete();
      }
    }
  }
}
