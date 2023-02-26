using System;

namespace BotShared.ShikimoriSharp.Exceptions
{
    public class UnprocessableEntityException : Exception
    {
        public UnprocessableEntityException() : base("Unprocessable entity, the input was wrong")
        {
        }
    }
}