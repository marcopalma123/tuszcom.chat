using System;
using System.Collections.Generic;
using System.Text;
using tuszcom.dao.Repository;
using tuszcom.models.DAO;
using tuszcom.models.Interfaces.Services;

namespace tuszcom.manager
{
    public class ChatService : IServiceChat
    {
        private readonly ChatRepository repository;

        public ChatService()
        {
            repository = new ChatRepository();
        }

        public bool AddNewAttachmentMessage(ChatMessageFiles file)
        {
            return repository.AddNewAttachmentMessage(file);
        }

        public bool AppendToChat(string connectionId, string username, string userid)
        {
            return repository.AppendToChat(connectionId, username, userid);
        }

        public List<ChatMessages> GetActiveMessageBetweenTwoUsers(string fromUser, string toUser)
        {
            return repository.GetActiveMessageBetweenTwoUsers(fromUser, toUser);
        }

        public List<ViewMessages> GetAllMessagesForUsername(string username)
        {
            return repository.GetAllMessagesForUsername(username);
        }

        public List<ViewMessages> GetAllMessagesFromUserToUser(string fromUser, string toUser, int NumberOfRecentMessages, int startElement)
        {
            return repository.GetAllMessagesFromUserToUser(fromUser, toUser, NumberOfRecentMessages, startElement);
        }

        public int GetAllUnreadMessages(string username)
        {
            return repository.GetAllUnreadMessages(username);
        }

        public List<ChatUserDetails> GetAllUserFromActiveChat()
        {
            return repository.GetAllUserFromActiveChat();
        }

        public List<AspNetUsers> GetAllUsers(string username)
        {
            return repository.GetAllUsers(username);
        }

        public string GetLoginByUserId(string id)
        {
            return repository.GetLoginByUserId(id);
        }

        public ChatMessageFiles GetMessageFileById(int id)
        {
            return repository.GetMessageFileById(id);
        }

        public AspNetUsers GetUserByEmail(string email)
        {
            return repository.GetUserByEmail(email);
        }

        public AspNetUsers GetUserById(string userId)
        {
            return repository.GetUserById(userId);
        }

        public AspNetUsers GetUserByUsername(string username)
        {
            return repository.GetUserByUsername(username);
        }

        public ChatUserDetails GetUserFromChat(string userid)
        {
            return repository.GetUserFromChat(userid);
        }

        public ChatUserDetails GetUserFromChatByConnectionId(string connectionId)
        {
            return repository.GetUserFromChatByConnectionId(connectionId);
        }

        public List<ChatMessageFiles> ListAttachments(List<int> ids)
        {
            return repository.ListAttachments(ids);
        }

        public bool RemoveFromActiveChat(ChatUserDetails item)
        {
            return repository.RemoveFromActiveChat(item);
        }

        public bool SendMessageToUser(string fromUser, string toUser, string message, string connectionId)
        {
            return repository.SendMessageToUser(fromUser, toUser, message, connectionId);
        }

        public bool SendMessageWithFileToUser(string fromUser, string toUser, string message, string ConnectionId, List<int> idMessageFile)
        {
            return repository.SendMessageWithFileToUser(fromUser, toUser, message, ConnectionId, idMessageFile);
        }

        public bool UpdateReadingMessageByCustomer(int id)
        {
            return repository.UpdateReadingMessageByCustomer(id);
        }
    }
}
