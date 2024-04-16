﻿using Microsoft.AspNetCore.Http;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Redis.Services
{
    /// <summary>
    /// مدیریت redis
    /// </summary>
    public interface IRedisManager
    {
        /// <summary>
        /// دیتابیس ردیس
        /// </summary>
        IRedisDatabase db { get; }


        IHttpContextAccessor ContextAccessor { get; }

    }
}
