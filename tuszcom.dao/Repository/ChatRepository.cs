using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using tuszcom.models;
using tuszcom.models.DAO;
using tuszcom.models.Interfaces.Repository;

namespace tuszcom.dao.Repository
{
    public class ChatRepository : IRepositoryChat
    {
        private readonly ChatDbContext context;

        public ChatRepository()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ChatDbContext>();
            context = new ChatDbContext(optionsBuilder.Options);

        }

        public bool AddNewAttachmentMessage(ChatMessageFiles file)
        {
            try
            {
                context.ChatMessageFiles.Add(file);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public bool AppendToChat(string connectionId, string username, string userid)
        {
            try
            {
                var userDetail = new ChatUserDetails
                {
                    ConnectionId = connectionId,
                    UserId = userid
                };

                context.ChatUserDetails.Add(userDetail);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public List<ChatMessages> GetActiveMessageBetweenTwoUsers(string fromUser, string toUser)
        {
            try
            {
                return context.ChatMessages.Where(x => x.CustomerUserId == fromUser && x.SenderUserId == toUser && x.IsRead == false).ToList();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new List<ChatMessages>();
            }
        }

        public List<ViewMessages> GetAllMessagesForUsername(string username)
        {
            try
            {
                var user = context.AspNetUsers.FirstOrDefault(x => x.UserName.ToUpper() == username.ToUpper());
                return context.ViewMessages.Where(x => x.CustomerUserId == user.Id || x.SenderUserId == user.Id).ToList();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new List<ViewMessages>();
            }
        }

        public List<ViewMessages> GetAllMessagesFromUserToUser(string fromUser, string toUser, int NumberOfRecentMessages, int startElement)
        {
            try
            {
                var messagesFrom = context.ViewMessages.Where(x => x.CustomerUserId == fromUser && x.SenderUserId == toUser);
                var messagesTo = context.ViewMessages.Where(x => x.SenderUserId == fromUser && x.CustomerUserId == toUser);
                var allMessages = messagesFrom.Union(messagesTo).OrderByDescending(x => x.Date);

                return allMessages.Skip(startElement).Take(NumberOfRecentMessages).ToList();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new List<ViewMessages>();
            }
        }

        public int GetAllUnreadMessages(string username)
        {
            try
            {
                var allUnreadMessages = context.ViewMessages.Where(x => x.CustomerUserId == username && x.IsRead == false).ToList();
                return allUnreadMessages.Any() ? allUnreadMessages.Count : 0;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return 0;
            }
        }

        public List<ChatUserDetails> GetAllUserFromActiveChat()
        {
            try
            {
                return context.ChatUserDetails.ToList();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new List<ChatUserDetails>();
            }
        }

        public List<AspNetUsers> GetAllUsers(string username)
        {
            try
            {
                return context.AspNetUsers.Where(x => x.UserName != username).ToList();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new List<AspNetUsers>();
            }
        }

        public string GetLoginByUserId(string id)
        {
            try
            {
                return context.AspNetUsers.Where(x => x.Id == id).Select(x => x.UserName).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return string.Empty;
            }
        }

        public ChatMessageFiles GetMessageFileById(int id)
        {
            try
            {
                return context.ChatMessageFiles.FirstOrDefault(x => x.IdChatMessageFile == id);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new ChatMessageFiles();
            }
        }

        public AspNetUsers GetUserById(string userId)
        {
            try
            {
                return context.AspNetUsers.FirstOrDefault(x => x.Id == userId);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new AspNetUsers();
            }
        }

        public AspNetUsers GetUserByUsername(string username)
        {
            try
            {
                return context.AspNetUsers.FirstOrDefault(x => x.UserName == username);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new AspNetUsers();
            }
        }

        public ChatUserDetails GetUserFromChat(string userid)
        {
            try
            {
                return context.ChatUserDetails.FirstOrDefault(x => x.UserId == userid);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return null;
            }
        }

        public ChatUserDetails GetUserFromChatByConnectionId(string connectionId)
        {
            try
            {
                return context.ChatUserDetails.FirstOrDefault(x => x.ConnectionId == connectionId);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return null;
            }
        }

        public List<ChatMessageFiles> ListAttachments(List<int> ids)
        {
            try
            {

                return context.ChatMessageFiles.Where(x => ids.Contains(x.IdChatMessageFile)).ToList();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new List<ChatMessageFiles>();
            }
        }

        public bool RemoveFromActiveChat(ChatUserDetails item)
        {
            try
            {
                context.ChatUserDetails.Remove(item);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public bool SendMessageToUser(string fromUser, string toUser, string message, string connectionId)
        {
            try
            {
                var _message = new ChatMessages
                {
                    CustomerUserId = toUser,
                    IsRead = false,
                    Message = message,
                    SenderUserId = fromUser,
                    ConnectionId = connectionId,
                    Date = DateTime.Now
                };

                context.ChatMessages.Add(_message);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public bool SendMessageWithFileToUser(string fromUser, string toUser, string message, string ConnectionId, List<int> idMessageFile)
        {
            try
            {
                using (IDbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        var _message = new ChatMessages
                        {
                            CustomerUserId = toUser,
                            IsRead = false,
                            Message = message,
                            SenderUserId = fromUser,
                            ConnectionId = ConnectionId,
                            Date = DateTime.Now,

                        };

                        context.ChatMessages.Add(_message);
                        context.SaveChanges();

                        foreach (var item in idMessageFile)
                        {
                            ChatMessageFiles file = context.ChatMessageFiles.FirstOrDefault(x => x.IdChatMessageFile == item);
                            file.ChatMessageId = _message.IdChatMessage;

                            context.ChatMessageFiles.Update(file);
                            context.SaveChanges();
                        }


                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.Error(ex);
                        transaction.Rollback();
                        return false;
                    }
                }


            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public bool UpdateReadingMessageByCustomer(int id)
        {
            try
            {
                var message = context.ChatMessages.FirstOrDefault(x => x.IdChatMessage == id);
                message.IsRead = true;
                context.ChatMessages.Update(message);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return false;
            }
        }

        public AspNetUsers GetUserByEmail(string email)
        {
            try
            {
                return context.AspNetUsers.FirstOrDefault(x => x.Email.ToUpper() == email.ToUpper());
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex);
                return new AspNetUsers();
            }
        }
    }
}

