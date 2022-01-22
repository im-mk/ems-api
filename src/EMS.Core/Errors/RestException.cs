using System;
using System.Net;

namespace EMS.Core.Errors
{
    public class RestException : Exception
    {
        public HttpStatusCode Code { get; }
        public Object Errors { get; }

        public RestException(HttpStatusCode code, Object errors = null)
        {
            this.Errors = errors;
            this.Code = code;
        }
    }
}