using BlackHole.Domain.DTO.Message;
using BlackHole.Domain.DTO.User;
using BlackHole.Domain.Entities;
using BlackHole.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHole.Business.Services
{
    /// <summary>
    /// Conversation service
    /// </summary>
    public class ConversationService : BaseService
    {
        /// <summary>
        /// Conversation service
        /// </summary>
        /// <param name="unitOfWork"></param>
        public ConversationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        /// <summary>
        /// Get snapshots
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="count"></param>
        /// <param name="skip"></param>
        /// <returns></returns>
        public IEnumerable<ConversationSnapshot> GetSnapshots(Guid userId, int count, int skip)
        {
            var conversations = UnitOfWork.ConversationRepository.GetLatestConversations(userId, count, skip);
            return conversations.Select(c => new ConversationSnapshot
                                 {
                                     ConversationId = c.ConversationId,
                                     Name = UnitOfWork.ConversationRepository.GetConversationName(c, userId),
                                     LastMessage = new BaseMessageModel
                                     {
                                         Text = c.LastMessage?.Text,
                                         CreatedOn = c.LastMessage?.UpdatedOn ?? c.LastMessage?.CreatedOn,
                                         Seen = c.LastMessage?.SenderUserId == userId || (c.LastMessage?.Seen ?? true),
                                     }
                                 });
        }

        /// <summary>
        /// Check if user belongs to a conversation
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool BelongsToConversation(Guid conversationId, Guid userId)
        {
            return UnitOfWork.ConversationRepository.GetUserConversations(userId).Any(c => c.ConversationId == conversationId);
        }

        /// <summary>
        /// Get conversation users
        /// </summary>
        /// <param name="conversationId"></param>
        /// <returns></returns>
        public IEnumerable<UserModel> GetConversationUsers(Guid conversationId)
        {
            return UnitOfWork.ConversationRepository.GetConversationUsers(conversationId)
                                                    .Select(u => new UserModel
                                                    {
                                                        UserId = u.UserId,
                                                        FirstName = u.FirstName,
                                                        LastName = u.LastName,
                                                        PhoneNumber = u.PhoneNumber,
                                                        //Picture = u.Picture == null ? Constants.DefaultPicture : Convert.ToBase64String(u.Picture),
                                                    });
        }

        /// <summary>
        /// Add a conversation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Conversation AddConversation(string name)
        {
            var conversation = new Conversation
            {
                Name = name,            
            };

            UnitOfWork.ConversationRepository.Add(conversation);

            Save();

            return conversation;
        }

        /// <summary>
        /// Add an user
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserConversation AddUser(Guid conversationId, Guid userId)
        {
            var userConversation = new UserConversation
            {
                ConversationId = conversationId,
                UserId = userId
            };

            UnitOfWork.UserConversationRepository.Add(userConversation);

            Save();

            return userConversation;
        }

        /// <summary>
        /// Remove an user
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="userId"></param>
        public void RemoveUser(Guid conversationId, Guid userId)
        {
            var userConversation = UnitOfWork.UserConversationRepository.Find(uc => uc.ConversationId == conversationId && uc.UserId == userId).First();

            UnitOfWork.UserConversationRepository.Remove(userConversation);

            Save();
        }

        /// <summary>
        /// Get messages
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public IEnumerable<MessageModel> GetMessages(Guid conversationId, int skip, int take)
        {
            return UnitOfWork.MessageRepository.GetMessages(conversationId, skip, take)
                                               .Select(m => new MessageModel
                                               {
                                                   ConversationId = m.ConversationId,
                                                   UserId = m.SenderUserId,
                                                   MessageId = m.MessageId,
                                                   Text = m.Text,
                                                   Seen = m.Seen,
                                                   RepliedMessage = m.RepliedMessage == null ? null : new MessageModel
                                                   {
                                                       MessageId = m.RepliedMessage.MessageId,
                                                       UserId = m.RepliedMessage.SenderUserId,
                                                       Text = m.RepliedMessage.Text,
                                                   },
                                               });
        }

        /// <summary>
        /// Get Conversation Details
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public ConversationModel GetConversationDetails(Guid conversationId, Guid currentUserId)
        {
            var conversation = UnitOfWork.ConversationRepository.Get(conversationId);
            var conversationUsers = GetConversationUsers(conversationId);

            return new ConversationModel
            {
                Name = conversation.Name ?? conversationUsers.First(u => u.UserId != currentUserId).Name,
                Users = conversationUsers.OrderBy(cu => cu.UserId == currentUserId)
            };
        }

        /// <summary>
        /// Get Conversation Picture
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public string GetConversationPicture(Guid conversationId, Guid currentUserId)
        {
            var conversationUsers = UnitOfWork.ConversationRepository.GetConversationUsers(conversationId);

            if (conversationUsers.Count() == 2)
            {
                var user = conversationUsers.FirstOrDefault(u => u.UserId != currentUserId);

                return user.Picture == null ? null : Convert.ToBase64String(user.Picture);
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// Get Contacts
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IEnumerable<UserModel> GetContacts(Guid userId, string query)
        {
            return UnitOfWork.ConversationRepository.GetContacts(userId, query)
                                                    .Select(u => new UserModel
                                                    {
                                                        UserId = u.UserId,
                                                        FirstName = u.FirstName,
                                                        LastName = u.LastName,
                                                        PhoneNumber = u.PhoneNumber,
                                                        //Picture = u.Picture == null ? Constants.DefaultPicture : Convert.ToBase64String(u.Picture),
                                                    });
        }

        /// <summary>
        /// Mark Conversation As Seen
        /// </summary>
        /// <param name="conversationId"></param>
        /// <param name="currentUserId"></param>
        public void MarkConversationAsSeen(Guid conversationId, Guid currentUserId)
        {
            var messages = UnitOfWork.MessageRepository.GetUnseenMessages(conversationId, currentUserId);

            messages.ToList().ForEach(u => u.Seen = true);

            Save();
        }

        /// <summary>
        /// Checks if conversation exists
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="targetUserId"></param>
        /// <returns></returns>
        public Guid? ConversationExists(Guid currentUserId, Guid targetUserId)
        {
            var conversation = UnitOfWork.ConversationRepository.Get(currentUserId, targetUserId);

            return conversation?.ConversationId;
        }
    }
}
