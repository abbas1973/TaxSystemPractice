using Domain.Entities;
using MediatR;
using Application.DTOs;
using Application.Contracts;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Utilities;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Application.SessionServices;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Application.Features.Base
{
    #region Request
    /// <summary>
    /// نوتیفیکیشن ثبت لاگ
    /// </summary>
    /// <typeparam name="TEntity">نوع موجودیت</typeparam>
    /// <typeparam name="TAction">نوع عملیاتی که انجام شده است</typeparam>
    [Description("نوتیفیکیشن ثبت لاگ")]
    public class LogNotification<TEntity, TAction>: INotification
        where TEntity : IBaseEntity
        where TAction : class
    {
        #region Constructors
        public LogNotification(){ }

        public LogNotification(
            List<LogParameterDTO> parameters,
            string description,
            LogLevel level = LogLevel.Information)
        {
            Parameters = parameters;
            Description = description;
            LogLevel = level;
        }

        public LogNotification(
            List<LogParameterDTO> parameters,
            LogLevel level = LogLevel.Information)
        {
            Parameters = parameters;
            LogLevel = level;
        }

        public LogNotification(
            string description,
            LogLevel level = LogLevel.Information)
        {
            Description = description;
            LogLevel = level;
        }
        #endregion


        #region Properties
        [Display(Name = "پارامترهای اضافی ارسالی")]
        public List<LogParameterDTO> Parameters { get; set; } = new List<LogParameterDTO>();


        [Display(Name = "توضیحات متنی")]
        public string Description { get; set; }


        [Display(Name = "سطح لاگ")]
        public LogLevel LogLevel { get; set; } = LogLevel.Information;
        #endregion
    }
    #endregion



    #region Handler
    public class LogNotificationHandler<TEntity, TAction> : INotificationHandler<LogNotification<TEntity, TAction>>
        where TEntity : IBaseEntity
        where TAction : class
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly IJwtManager _jwt;
        private readonly ILogger<TAction> _logger;

        public LogNotificationHandler(
            IJwtManager jwt,
            ILogger<TAction> logger,
            IHttpContextAccessor accessor)
        {
            _jwt = jwt;
            _logger = logger;
            _accessor = accessor;
        }

        public async Task Handle(LogNotification<TEntity, TAction> request, CancellationToken cancellationToken)
        {
            var logParts = InitialLogParts(request);
            MakeLog(logParts, request.LogLevel);
        }


        #region تعیین بخش های مختلف لاگ به ترتیب
        private List<LogParameterDTO> InitialLogParts(LogNotification<TEntity, TAction> request)
        {
            List<LogParameterDTO> logParts = new List<LogParameterDTO>();
            logParts.Add(new LogParameterDTO("موجودیت", "Entity", typeof(TEntity).Name));
            logParts.Add(new LogParameterDTO("نام موجودیت", "EntityName", typeof(TEntity).GetDescription()));
            logParts.Add(new LogParameterDTO("عملیات", "Action", typeof(TAction).Name));
            logParts.Add(new LogParameterDTO("نام عملیات", "ActionName", typeof(TAction).GetDescription()));

            var token = _jwt.GetHeaderToken();
            if (!string.IsNullOrEmpty(token))
            {
                var userId = _jwt.GetUserId();
                var userName = _jwt.GetUserName();
                logParts.Add(new LogParameterDTO("آیدی کاربر", "UserId", userId));
                logParts.Add(new LogParameterDTO("نام کاربر", "UserName", userName));
            }
            else
            {
                var user = _accessor?.HttpContext?.Session?.GetUser();
                if(user != null)
                {
                    logParts.Add(new LogParameterDTO("آیدی کاربر", "UserId", user.Id));
                    logParts.Add(new LogParameterDTO("نام کاربر", "UserName", user.Name));
                }
            }


            if (request.Parameters != null && request.Parameters.Any())
                logParts.AddRange(request.Parameters);

            if (!string.IsNullOrWhiteSpace(request.Description))
                logParts.Add(new LogParameterDTO("توضیحات", "Description", request.Description));

            return logParts;
        }
        #endregion



        #region ایجاد لاگ
        private void MakeLog(List<LogParameterDTO> logParts, LogLevel level)
        {
            List<string> logTexts = new List<string>();
            List<object> logParams = new List<object>();
            foreach (var part in logParts)
            {
                logTexts.Add(part.LogText);
                logParams.Add(part.Value);
            }

            var logText = string.Join(" | ", logTexts);
            _logger.Log(level, logText, logParams.ToArray());
        } 
        #endregion

    }

    #endregion
}
