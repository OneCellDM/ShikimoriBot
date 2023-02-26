using System;

namespace BotShared.ShikimoriSharp.Exceptions
{
    public class FailedRequestException : Exception
    {
        public FailedRequestException(string additionalContent) : base($"Request is failed: {additionalContent}")
        {
        }
    }
}