using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArkData
{
    /// <summary>
    /// Interface for ark file parsers.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IParser<T>
    {
        /// <summary>
        /// Parses the specified file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        T Parse(string filePath);

        /// <summary>
        /// Parses the specified file asynchronous.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        Task<T> ParseAsync(string filePath);
    }
}
