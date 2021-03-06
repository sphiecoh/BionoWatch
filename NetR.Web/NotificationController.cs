﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetR.Web.Infrastructure;
using NetR.Web.Infrastructure.Entities;

namespace NetR.Web
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private readonly BionoContext bionoContext;

        public NotificationController(BionoContext bionoContext)
        {
            this.bionoContext = bionoContext;
        }
        // GET: api/<controller>
        [HttpPost("group/{groupname}")]
        public async Task<IActionResult> CreateGroup(string groupname)
        {
            var grp = new Group { Name = groupname };
            bionoContext.Groups.Add(grp);
            await bionoContext.SaveChangesAsync();
            return Ok(grp);
        }
        [HttpPost("notification/{groupid}/{emailId}")]
        public async Task<IActionResult> AddEmailToGroup(int groupid,int emailId)
        {
            var grp = new NotificationGroup { GroupId = groupid, EmailNotificationId = emailId };
            bionoContext.NotificationGroups.Add(grp);
            await bionoContext.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("groups/{id}/notifications")]
        public async Task<IActionResult> GetGroupNotifications(int id)
        {
            var notificationGroups = await bionoContext.NotificationGroups.Include(x => x.EmailNotification).Where(x => x.GroupId == id).ToListAsync();

            return Ok(notificationGroups);
        }
        [HttpDelete("groups/{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            bionoContext.Database.ExecuteSqlCommand($"delete from Groups where id = {id}");
            return Ok();
        }
        [HttpGet]
        public IActionResult GetGroups()
        {
            return Ok(bionoContext.Groups.ToList());
        }

    }
}
