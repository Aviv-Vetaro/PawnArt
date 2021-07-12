using System;
using System.Threading.Tasks;

namespace PawnArt.Logic
{
    /// <summary>
    /// represents a repository of an type
    /// </summary>
    /// <typeparam name="T">the type</typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// gets an item from the repositor
        /// </summary>
        /// <param name="id">the id of the item</param>
        /// <returns>the item found</returns>
        public Task<T> GetItemByIdAsync(Guid id);
        /// <summary>
        /// deletes an item from the repository
        /// </summary>
        /// <param name="id">the id of the item</param>
        /// <returns></returns>
        public Task DeleteItemByIdAsync(Guid id);
        /// <summary>
        /// adds item to the repository
        /// </summary>
        /// <param name="item">the item</param>
        /// <returns></returns>
        public Task AddItemAsync(T item);
    }
}
