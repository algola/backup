using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace PapiroMVC.Hubs
{
    public class ChatHub : Hub
    {
        public void Send(string message)
        {
            // Call the addNewMessageToPage method to update clients.
            Clients.All.addNewMessageToPage(message);
        }

        internal static void Update(string message)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
            context.Clients.All.addNewMessageToPage(message);
        }

    }


    public class PlanningHub : Hub
    {
        public void Send(string groupName, string message)
        {
            // Call the addNewMessageToPage method to update clients.
       //     Clients.Others.addNewMessageToPage(message);
            Clients.OthersInGroup(groupName).addNewMessageToPage(message);
        }

        internal static void Update(string groupName, string message)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<PlanningHub>();
            context.Clients.Group(groupName).addNewMessageToPage(message);
        }

        public Task JoinRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }

    }

}