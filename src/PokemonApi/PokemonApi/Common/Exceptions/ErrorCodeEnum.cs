using Ardalis.SmartEnum;

namespace PokemonApi.Common.Exceptions
{
    public class ErrorCodeEnum : SmartEnum<ErrorCodeEnum>
    {
        protected ErrorCodeEnum(string name, int value)
            : base(name, value)
        {
        }
    }
}
