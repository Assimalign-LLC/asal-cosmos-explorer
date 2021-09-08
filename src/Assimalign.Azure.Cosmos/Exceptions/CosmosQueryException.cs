using System;
using System.Collections.Generic;
using System.Text;

namespace Assimalign.Azure.Cosmos.Exceptions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CosmosQueryException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public CosmosQueryException(string message) 
            : base(message) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CosmosQueryException(string message, Exception innerException) 
            : base(message, innerException) {  }


        /// <summary>
        /// 
        /// </summary>
        public virtual string Title { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<CosmosError> GetErrors()
        {
            var errors = new List<CosmosError>();
            var exceptions = FlattenExceptions(this);

            foreach(var exception in exceptions)
            {
                var isInternal = CosmosErrorCode.IsCodeInternal(exception.HResult);

                errors.Add(new CosmosError()
                {
                    Title = isInternal ? $"Cosmos Query Error: {Title}" : "Internal System Error",
                    Code = isInternal ? exception.HResult : CosmosErrorCode.Internal,
                    Message = exception.Message,
                    Source = exception.Source
                });
            }

            return errors;
        }


        /// <summary>
        /// Will pull out all inner exceptions into a collection.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        private IEnumerable<Exception> FlattenExceptions(Exception exception)
        {
            var exceptions = new List<Exception>();
            if (exception.InnerException is not null)
            {
                exceptions.AddRange(FlattenExceptions(exception.InnerException));
            }
            else
            {
                exceptions.Add(exception);
            }
            return exceptions;
        }
    }
}
