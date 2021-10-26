using BlackHole.Domain.Interfaces;
using System;

namespace BlackHole.Business.Services
{
    public abstract class BaseService
    {
        private protected readonly IUnitOfWork UnitOfWork;

        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        private protected void Save()
        {
            try
            {
                UnitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                throw ex;
            }
        }
    }
}
