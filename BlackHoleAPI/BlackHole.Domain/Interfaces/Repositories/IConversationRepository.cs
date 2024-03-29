﻿using BlackHole.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHole.Domain.Interfaces.Repositories
{
    public interface IConversationRepository : IRepository<Conversation>
    {
        Conversation Get(Guid user1, Guid user2);
        IEnumerable<Conversation> GetLatestConversations(Guid userId, int count, int skip);
        IQueryable<Conversation> GetUserConversations(Guid userId);
        IEnumerable<User> GetConversationUsers(Guid conversationId);
        IEnumerable<User> GetContacts(Guid userId, string query);
        string GetConversationName(Conversation conversation, Guid currentUserId);
        string GetConversationPicture(Conversation conversation, Guid currentUserId);
    }
}
