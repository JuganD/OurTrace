using AutoMapper.Configuration.Annotations;
using OurTrace.Data.Models;
using System;

namespace OurTrace.App.Models.ViewModels.Group
{
    public class GroupMemberViewModel
    {
        public string Username { get; set; }
        public string JoinedOn { get; set; }
        public string FullName { get; set; }
        [Ignore]
        public GroupAdminType Elevation { get; set; }
    }
}