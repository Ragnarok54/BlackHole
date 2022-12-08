using BlackHole.Domain.Interfaces;
using System;

namespace BlackHole.Business.Services
{
    /// <summary>
    /// Base service implementation
    /// </summary>
    public abstract class BaseService
    {
        /// <summary>
        /// UnitOfWork field for database connection
        /// </summary>
        private protected readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Base Service constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        protected BaseService(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        
        /// <summary>
        /// Propagate changes to database
        /// </summary>
        private protected void Save()
        {
            UnitOfWork.SaveChanges();
        }
    }
}
