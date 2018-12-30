using System;
using System.Collections.Generic;
using System.Text;
using tuszcom.models.DAO;

namespace tuszcom.models.Interfaces.Repository
{
    public interface IRepositoryChat
    {
        bool AddNewAttachmentMessage(ChatMessageFiles file);
        bool AppendToChat(string connectionId, string username, string userid);
        bool RemoveFromActiveChat(ChatUserDetails item);
        bool SendMessageToUser(string fromUser, string toUser, string message, string connectionId);
        bool SendMessageWithFileToUser(string fromUser, string toUser, string message, string ConnectionId, List<int> idMessageFile);
        bool UpdateReadingMessageByCustomer(int id);
        int GetAllUnreadMessages(string username);
        string GetLoginByUserId(string id);
        ChatUserDetails GetUserFromChat(string userid);
        ChatUserDetails GetUserFromChatByConnectionId(string connectionId);
        AspNetUsers GetUserByEmail(string email);
        AspNetUsers GetUserById(string userId);
        AspNetUsers GetUserByUsername(string username);
        List<AspNetUsers> GetAllUsers(string username);
        ChatMessageFiles GetMessageFileById(int id);
        List<ChatUserDetails> GetAllUserFromActiveChat();
        List<ChatMessageFiles> ListAttachments(List<int> ids);
        List<ChatMessages> GetActiveMessageBetweenTwoUsers(string fromUser, string toUser);
        List<ViewMessages> GetAllMessagesForUsername(string username);
        List<ViewMessages> GetAllMessagesFromUserToUser(string fromUser, string toUser, int NumberOfRecentMessages, int startElement);
    }
}
