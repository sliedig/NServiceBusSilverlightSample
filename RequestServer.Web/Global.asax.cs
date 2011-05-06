using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using NServiceBus;

namespace ResponseServer.Web
{
	public class Global : System.Web.HttpApplication
	{

		public static IBus Bus { get; private set; }

		protected void Application_Start(object sender, EventArgs e)
		{

			Bus = Configure.WithWeb()
							.Log4Net()
							.DefaultBuilder()
							.XmlSerializer()
							.MsmqTransport()
							.UnicastBus()
								.LoadMessageHandlers()
							.CreateBus()
							.Start();
		}

}
}