using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PokemonApi.Common.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message, HttpStatusCode statusCode, params ErrorCodeEnum[] errorCodes)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = new List<ErrorCodeEnum>(errorCodes);
        }

        public HttpStatusCode StatusCode { get; }

        public IReadOnlyCollection<ErrorCodeEnum> Errors { get; }

        public IReadOnlyCollection<ErrorCodeEnum> GetAllErrors()
        {
            var errorsList = new List<ErrorCodeEnum>(Errors);

            Exception? current = this;

            while (current != null)
            {
                current = current.InnerException;

                if (current is BusinessException businessException)
                {
                    errorsList.AddRange(businessException.Errors);
                }
            }

            return errorsList.Distinct()
                .OrderBy(c => c.Value)
                .ToArray();
        }
    }
}
