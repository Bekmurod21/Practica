﻿using Microsoft.AspNetCore.Http;

namespace Practic.Service.Helpers
{
    public class HttpContextHelper
    {
        public static IHttpContextAccessor Accessor { get; set; }
        public static HttpContext HttpContext => Accessor?.HttpContext;
        public static IHeaderDictionary ResponseHeaders => HttpContext?.Response?.Headers;
        public static long? UserId => long.TryParse(HttpContext?.User?.FindFirst("id")?.Value, out _tempUserId) ? _tempUserId : null;

        private static long _tempUserId;
    }
}
