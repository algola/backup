using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Services;

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

    public class PapiroUser
    {
        public string ConnectedIds { get; set; }
        public string GroupName { get; set; }
        public DateTime TimeConnection { get; set; }
    }

    public static class UserHandler
    {
        public static HashSet<PapiroUser> User = new HashSet<PapiroUser>();
    }

    public class UserHub : Hub
    {

        public void Send(string groupName, string message)
        {

            Clients.All.regroup();
            var us = UserHandler.User.Where(x => x.GroupName == groupName);

            ProfileRepository rep = new ProfileRepository();
            rep.SetDbName(groupName);

            int users = 0;

            try
            {
                users = rep.GetSingle(groupName).Modules.FirstOrDefault(x => x.CodModule == message).Users??0;
            }
            catch (Exception)
            {                

            }

            if (us!=null && us.Count() > users && users !=0)
            {
                //estraggo i più vecchi e ciclo fino ad avere un numero di client
                var usToDisc = UserHandler.User.Where(x => x.GroupName == groupName).OrderBy(x=>x.TimeConnection).ToArray();

                for (int i = 0; i < usToDisc.Count() - 2; i++)
                {
                    Clients.Client(usToDisc[i].ConnectedIds).forceDisconnection(message);  
                }

            }

        }

        internal static void Update(string groupName, string message)
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<UserHub>();
            context.Clients.Group(groupName).forceDisconnection(message);
        }

        public Task JoinRoom(string roomName)
        {
            var u = UserHandler.User.FirstOrDefault(x => x.ConnectedIds == Context.ConnectionId);
            u.GroupName = roomName;
            return Groups.Add(Context.ConnectionId, roomName);
        }

        public Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }

        private void AddUser(string connectionId)
        {
            if (!UserHandler.User.Select(x => x.ConnectedIds).Contains(connectionId))
            {
                UserHandler.User.Add(new PapiroUser { ConnectedIds = connectionId, TimeConnection = DateTime.Now });
            }         
        }

        public override Task OnConnected()
        {
            AddUser(Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected()
        {
            try
            {
                UserHandler.User.Remove(UserHandler.User.FirstOrDefault(x => x.ConnectedIds == Context.ConnectionId));
            }
            catch (Exception)
            {
                Console.Write("ciao");
            }
            return base.OnDisconnected();
        }

        public override Task OnReconnected()
        {
            AddUser(Context.ConnectionId);            
            return base.OnReconnected();
        }


    }


}