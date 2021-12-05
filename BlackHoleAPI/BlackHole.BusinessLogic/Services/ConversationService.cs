using BlackHole.Domain.Interfaces;

namespace BlackHole.Business.Services
{
    public class ConversationService : BaseService
    {
        public ConversationService(IUnitOfWork unitOfWork) : base(unitOfWork) { }
    }
}
