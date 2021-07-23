using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PokemonApi.Common.Exceptions
{
    public class ErrorCodes : ErrorCodeEnum
    {
        private ErrorCodes(string name, int value)
            : base(name, value)
        {
        }

        public static ErrorCodeEnum ConnectionStringIsNotDefined => new ErrorCodes("Connection string is not defined", 1);
        public static ErrorCodeEnum DefaultCsvFilePathIsNotDefined => new ErrorCodes("Default csv file Path is not defined", 2);
        public static ErrorCodeEnum DefaultCsvFilePathIsNotExist => new ErrorCodes("Default csv file Path is not exist", 3);
    }
}
