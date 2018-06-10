using System.Collections.Generic;
using System.Linq;
using System.Threading;
using BankApi.Logic.Data.Models;

namespace BankApi.Logic.Data.Repositories.InMemory
{
    /// <summary>
    ///     Implements a thread-safe List with basic identity services
    /// </summary>
    /// <typeparam name="TModel">Type of the model to be stored in the list</typeparam>
    public class InMemoryDbSet<TModel> where TModel : DbModel
    {
        private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

        private readonly List<TModel> _models = new List<TModel>();

        /// <summary>
        ///     Adds an item to the collection
        /// </summary>
        /// <param name="item">Item to be added</param>
        /// <returns>The item as it exists in the collection</returns>
        public TModel Add(TModel item)
        {
            _lock.EnterUpgradeableReadLock();
            try
            {
                if (!_models.Contains(item))
                {
                    _lock.EnterWriteLock();

                    try
                    {
                        _models.Add(item);
                    }
                    finally
                    {
                        _lock.ExitWriteLock();
                    }
                }

                item.Id = _models.IndexOf(item) + 1;
                return item;
            }
            finally
            {
                _lock.ExitUpgradeableReadLock();
            }
        }

        /// <summary>
        ///     Queries all items in the collection
        /// </summary>
        /// <returns>An IQueryable containing all items in the collection at the moment the query was initiated</returns>
        public IQueryable<TModel> Query()
        {
            _lock.EnterReadLock();
            try
            {
                return _models.ToList().AsQueryable();
            }
            finally
            {
                _lock.ExitReadLock();
            }
        }
    }
}