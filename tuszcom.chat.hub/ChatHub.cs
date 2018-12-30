using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tuszcom.chat.Models;
using tuszcom.manager;
using tuszcom.models;
using tuszcom.models.DAO;
using tuszcom.models.Interfaces.Services;
using tuszcom.services;
using tuszcom.services.Helpers;

namespace tuszcom.hub
{
    [Authorize]//(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)
    public class ChatHub : Hub
    {
        private readonly IServiceChat chatService;
        private readonly IServiceUser userService;
        private readonly UserManager<ApplicationUser> managerUser;
        private readonly RoleManager<ApplicationRole> managerRole;

        public ChatHub(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            chatService = new ChatService();
            userService = new UserService();
            managerUser = userManager;
            managerRole = roleManager;
        }

        public override async Task OnConnectedAsync()
        {
            var email = Context.UserIdentifier;
            var user = chatService.GetUserByEmail(Context.UserIdentifier);
            string username = user.UserName;
            var item = chatService.GetUserFromChat(username);

            if (item != null)
            {
                //Disconnect
                chatService.RemoveFromActiveChat(item);
            }
            var userinchat = chatService.GetUserFromChat(username);
            if (userinchat == null)
            {
                //Connect
                chatService.AppendToChat(Context.ConnectionId, username, user.Id);
            }
            var NumberOfUnreadMessagesForUser = chatService.GetAllUnreadMessages(username);
            await Clients.User(Context.UserIdentifier).SendAsync("UnreadCountMessages", NumberOfUnreadMessagesForUser.ToString());
        }

        public Task GetMessages(string toUser, int startElement, string target)
        {
            try
            {

                var email = Context.UserIdentifier;
                var user = chatService.GetUserByEmail(Context.UserIdentifier);
                string username = user.UserName;
                var messages = chatService.GetActiveMessageBetweenTwoUsers(user.Id, toUser);

                foreach (var message in messages)
                {
                    //zaktualizowanie nieprzeczytanych wiadomosci jako przeczytane
                    chatService.UpdateReadingMessageByCustomer(message.IdChatMessage);
                }

                string html = string.Empty;

                int displayMessagesCount =  Convert.ToInt32(SettingsHelper.Get("Messenger.NumberOfRecentMessages"));
                var allmessagesfromusertouser = chatService.GetAllMessagesFromUserToUser(user.Id, toUser, displayMessagesCount, startElement);

                List<ViewMessages> SortingList = new List<ViewMessages>();
                SortingList = allmessagesfromusertouser.OrderBy(x => x.Date).ToList();

                foreach (var message in SortingList)
                {
                    if (message.senderUsername == user.UserName)
                        html += $"<div class='direct-chat-msg ' data-loader='customLoaderName'><div class='direct-chat-info clearfix'><span class='direct-chat-name left'>{message.senderUsername }</span><span class='direct-chat-timestamp right'>{message.Date.ToShortDateString()} {message.Date.ToShortTimeString()}</span></div><div class='direct-chat-text' style='word-wrap: break-word;background-color:#f39c12;border-color:#f39c12;border-left-color:f39c12 !important;'>{message.Message }</div></div>";
                    else
                        html += $"<div class='direct-chat-msg ' data-loader='customLoaderName'><div class='direct-chat-info clearfix'><span class='direct-chat-name right'>{message.senderUsername }</span><span class='direct-chat-timestamp left'>{message.Date.ToShortDateString()} {message.Date.ToShortTimeString()}</span></div><div class='direct-chat-text' style='word-wrap: break-word;'>{message.Message }</div></div>";
                }

                List<object> objects = new List<object>();
                objects.Add(html);
                objects.Add(toUser);
                objects.Add(displayMessagesCount);
                objects.Add(target);
                if (target == "OlderMessages")
                    return Clients.User(Context.UserIdentifier).SendAsync("GetOlderMessages", objects);
                else
                    return Clients.User(Context.UserIdentifier).SendAsync("GetMessages", objects);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return null;
            }

        }
       
        public void ReadMessages(string toUser)
        {
            var email = Context.UserIdentifier;
            var user = chatService.GetUserByEmail(Context.UserIdentifier);
            string username = user.UserName;

            //Pobranie nieprzeczytanych wiadomości
            var messages = chatService.GetActiveMessageBetweenTwoUsers(user.Id, toUser);

            foreach (var message in messages)
            {
                //bazodanowe odczytanie wiadomosci
                chatService.UpdateReadingMessageByCustomer(message.IdChatMessage);
            }

        }
        public async Task UnreadCountMessages()
        {
            //Context.User.Identity.Name   var user = chatManager.GetUserByEmail(Context.UserIdentifier);
            var NumberOfUnreadMessagesForUser = chatService.GetAllUnreadMessages(Context.User.Identity.Name);
            await Clients.User(Context.UserIdentifier).SendAsync("UnreadCountMessages", NumberOfUnreadMessagesForUser.ToString());
        }
        public override async Task OnDisconnectedAsync(Exception ex)
        {
            var item = chatService.GetUserFromChatByConnectionId(Context.ConnectionId);
            //Disconnect
            if (item != null)
                chatService.RemoveFromActiveChat(item);

            await Clients.All.SendAsync("Test", $"{Context.UserIdentifier} left");

        }

        public void Send(string message, string toUser)
        {
            var email = Context.UserIdentifier;
            var fromUser = chatService.GetUserByEmail(Context.UserIdentifier);
            string username = fromUser.UserName;

            var toUserGet = chatService.GetUserById(toUser);

            chatService.SendMessageToUser(fromUser.Id, toUser, message, Context.ConnectionId);
            string htmlMessageLeft = $"<div class='direct-chat-msg' data-loader='customLoaderName'><div class='direct-chat-info clearfix'><span class='direct-chat-name left'>{username}</span><span class='direct-chat-timestamp right'>{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}</span></div><div class='direct-chat-text' style='word-wrap: break-word;background-color:#f39c12;border-color:#f39c12;border-left-color:f39c12 !important; ' >{message}</div></div>";
            string htmlMessageRight = $"<div class='direct-chat-msg ' data-loader='customLoaderName'><div class='direct-chat-info clearfix'><span class='direct-chat-name right'>{username}</span><span class='direct-chat-timestamp left'>{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}</span></div><div class='direct-chat-text'  style='word-wrap: break-word;' >{message}</div></div>";


            List<object> objects = new List<object>();
            objects.Add(htmlMessageRight);
            objects.Add(fromUser.Id);

            Clients.User(Context.UserIdentifier).SendAsync("Send", htmlMessageLeft);
            Clients.User(toUserGet.Email).SendAsync("Get", objects);
        }

        public void SendMessageWithFile(List<int> ListAttachmentIds, string toUser)
        {

            string message = string.Empty;
            var fromUser = chatService.GetUserByEmail(Context.UserIdentifier);
            string username = fromUser.UserName;
            var toUserGet = chatService.GetUserById(toUser);
            List<ChatMessageFiles> files = chatService.ListAttachments(ListAttachmentIds);

            if (files.Count == 1)
                message = $"Użytkownik {fromUser.UserName} przesłał plik. <div class='timeline-footer'><a href='/Chat/DownloadFile/{files[0].IdChatMessageFile}' class='btn btn-primary btn-xs' download={files[0].Name} >Pobierz {files[0].Name} </a></div>";
            else
            {
                message = $"Użytkownik {fromUser.UserName} przesłał pliki";
                foreach (ChatMessageFiles file in files)
                {
                    message += $"<div class='timeline-footer'><a href='/Chat/DownloadFile/{file.IdChatMessageFile}' class='btn btn-primary btn-xs' download={file.Name} >Pobierz {file.Name} </a></div>";
                }
            }


            string htmlMessageLeft = $"<div class='direct-chat-msg'><div class='direct-chat-info clearfix'><span class='direct-chat-name left'>{username}</span><span class='direct-chat-timestamp right'>{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}</span></div><div class='direct-chat-text' style='word-wrap: break-word;'>{message}</div></div>";
            string htmlMessageRight = $"<div class='direct-chat-msg'><div class='direct-chat-info clearfix'><span class='direct-chat-name right'>{username}</span><span class='direct-chat-timestamp left'>{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}</span></div><div class='direct-chat-text' style='word-wrap: break-word;'>{message}</div></div>";

            chatService.SendMessageWithFileToUser(fromUser.Id, toUser, message, Context.ConnectionId, ListAttachmentIds);

            List<object> objects = new List<object>();

            objects.Add(htmlMessageRight);
            objects.Add(fromUser.Email);

            Clients.User(Context.UserIdentifier).SendAsync("Send", htmlMessageLeft);
            Clients.User(toUserGet.Email).SendAsync("Get", objects);
        }

        public Task UnreadMessages(string toUser)
        {
            var toUserGet = chatService.GetUserById(toUser);
            var fromUser = chatService.GetUserByEmail(Context.UserIdentifier);

            string username = fromUser.UserName;
            var messages = chatService.GetActiveMessageBetweenTwoUsers(fromUser.Id, toUserGet.Id); 
            List<object> objects = new List<object>();
            objects.Add($"<small class='label pull-right bg-blue'>{messages.Count}</small>");
            objects.Add(toUser);
            return Clients.User(Context.UserIdentifier).SendAsync("UnreadMessages", objects);

        }
        public async Task GetConversationsUsers()
        {
            var fromUser = chatService.GetUserByEmail(Context.UserIdentifier);
            string username = fromUser.UserName;
            var messagesForUser = chatService.GetAllMessagesForUsername(username);
            var incomingMessagessUsers = messagesForUser.Where(x => x.CustomerUserId == username).Select(x => new { UserName = x.senderUsername, FirstName = x.senderFirstname, SurName = x.senderSurname, UserId = x.SenderUserId }).Distinct().ToList();
            var outgoinMessagesUsers = messagesForUser.Where(x => x.SenderUserId == username).Select(x => new { UserName = x.customerUsername, FirstName = x.customerirstname, SurName = x.customerSurname, UserId = x.CustomerUserId }).Distinct().ToList();

            var conversationUsers = incomingMessagessUsers.Union(outgoinMessagesUsers).ToList();

            string html = string.Empty;

            foreach (var user in conversationUsers)
            {
                var messages = chatService.GetActiveMessageBetweenTwoUsers(fromUser.Id, user.UserId);
                if (messages.Count > 0)
                    html += $"<div class='media conversation'><div class='media-body'><a class='connect' userId={user.UserId}>  <div class='media-heading' style='width:90%;float:left;' >({user.UserName}) { user.FirstName}  {user.SurName}</div> <div id='unread_{user.UserId}' style='width:10%;' class='label right bg-blue'>{messages.Count}</div>  </a> <a/></div></div>";
                else
                    html += $"<div class='media conversation'><div class='media-body'><a class='connect' userId={user.UserId}>   <div class='media-heading' style='width:90%;float:left;'>({user.UserName}) { user.FirstName}  {user.SurName}</div> <div id='unread_{user.UserId}'  style='width:10%;'></div><a/></div></div>";

            }
            await Clients.User(Context.UserIdentifier).SendAsync("GetConversationsUsers", html);
        }

    }
}

