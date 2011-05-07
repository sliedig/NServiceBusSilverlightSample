using System;
using System.Windows;
using System.Windows.Controls;
using System.ServiceModel;
using SilverlightClient.OrderPollingService;
using SilverlightClient.CancelOrderService;

namespace SilverlightClient
{
	public partial class MainPage : UserControl
	{
		private readonly OrderPollingServiceClient _pollingClient;

		public MainPage()
		{
			InitializeComponent();

			// Setup up client channel for the Polling Service
			var address = new EndpointAddress("http://localhost:7101/OrderPollingService.svc");
			var binding = new PollingDuplexHttpBinding();
			_pollingClient = new OrderPollingServiceClient(binding, address);
			_pollingClient.CancelOrderResponseReceived += OnCancelOrderResponseReceived;
			_pollingClient.ReceiveOrderCancellationsReceived += OnReceiveOrderCancellationsReceived;
			_pollingClient.GetOrderCancellationsAsync();

		}

		/// <summary>
		/// Handles the response for the registration to the Polling Service
		/// </summary>
		private void OnReceiveOrderCancellationsReceived(object sender, ReceiveOrderCancellationsReceivedEventArgs e)
		{
			tbConnected.Text = e.Error == null ? e.sessionId : "Not connected.";
		}

		/// <summary>
		/// Handles the response for the Order cancellation.
		/// </summary>
		private void OnCancelOrderResponseReceived(object sender, CancelOrderResponseReceivedEventArgs e)
		{
			if (e.Error == null)
			{
				if (e.pollingResponse.ConfirmationId != Guid.Empty)
				{
					lblOrderId.Text = e.pollingResponse.OrderId.ToString();
					lblConfirmationId.Text = e.pollingResponse.ConfirmationId.ToString();
					lblOrderStatus.Text = e.pollingResponse.Status.ToString();
				}
			}
			else
			{
				// Log error
			}
		}

		/// <summary>
		/// Executes Cancel Order Command
		/// </summary>
		private void OnCanelOrderCommand(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(txtOrderId.Text))
				return;

			var request = new CancelOrderRequest { OrderId = int.Parse(txtOrderId.Text) };

			var client = new WcfServiceOf_CancelOrderRequest_ErrorCodesClient("BasicHttpBinding_IWcfServiceOf_CancelOrderRequest_ErrorCodes");
			client.ProcessCompleted += OnProcessCompleted;
			client.ProcessAsync(request);

			txtOrderId.Text = string.Empty;
		}

		/// <summary>
		/// Handles response from  Cancel Order Command.
		/// </summary>
		void OnProcessCompleted(object sender, ProcessCompletedEventArgs e)
		{
			tbResult.Text = e.Error == null ? Enum.GetName(typeof(ErrorCodes), e.Result) : e.Error.Message;
		}

		internal void Disconnect()
		{
			_pollingClient.CloseAsync();
		}

	}
}
