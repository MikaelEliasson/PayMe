using PayMe.Core.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;


namespace PayMe2.Infrastructure
{
    public static class ControllerExtensions
    {
        public static AuditInfo GetAudit(this Controller source)
        {
            return new AuditInfo
            {
                TimeUtc = DateTime.UtcNow,
                UserAgent = source.Request.UserAgent,
                UserId = Guid.Parse(source.User.Identity.GetUserId()),
                Ip = source.Request.UserHostAddress
            };
        }
    }
}