using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace JobPortal2.Hubs
{
    public class AppHub : Hub
    {
        public static void updateUserInfo()
        {
            IHubContext contest = GlobalHost.ConnectionManager.GetHubContext<AppHub>();
            contest.Clients.All.updatePage();
        }
    }
}