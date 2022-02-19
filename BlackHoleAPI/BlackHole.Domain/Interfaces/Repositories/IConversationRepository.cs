﻿using BlackHole.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackHole.Domain.Interfaces.Repositories
{
    public interface IConversationRepository : IRepository<Conversation>
    {
        IEnumerable<Conversation> GetLatestConversations(Guid userId, int count, int skip);
        IQueryable<Conversation> GetUserConversations(Guid userId);
    }
}