namespace OurTrace.Services.Tests
{
    using AutoMapper;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using OurTrace.Data;
    using OurTrace.Data.Identity.Models;
    using OurTrace.Data.Models;
    using OurTrace.Services.Tests.StaticResources;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class GroupServiceTests
    {
        private readonly IMapper automapper;
        private readonly GroupService groupService;
        private readonly OurTraceDbContext dbContext;

        public GroupServiceTests()
        {
            var options = new DbContextOptionsBuilder<OurTraceDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            this.dbContext = new OurTraceDbContext(options);

            this.automapper = MapperInitializer.CreateMapper();

            this.groupService = new GroupService(dbContext, automapper);
        }
        [Fact]
        public async Task CreateNewGroupAsync_ShouldCreate_Succesfully()
        {
            var groupOwner = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };

            await this.dbContext.Users.AddAsync(groupOwner);
            await this.dbContext.SaveChangesAsync();

            var result = await this.groupService.CreateNewGroupAsync("Grupa", "Gosho");
            Assert.True(result);

            var actual = await this.dbContext.Groups.SingleOrDefaultAsync(x => x.Creator.UserName == groupOwner.UserName);
            Assert.NotNull(actual);
        }

        [Fact]
        public async Task DiscoverGroupsAsync_ShouldReturn_NotJoinedGroup()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group1 = new Group()
            {
                Name = "GroupWhereUserIsMember"
            };
            group1.Members.Add(new UserGroup()
            {
                ConfirmedMember = true,
                Group = group1,
                User = user
            });

            var group2 = new Group()
            {
                Name = "GroupWhereUserIsNotMember"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group1);
            await this.dbContext.Groups.AddAsync(group2);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.DiscoverGroupsAsync("Gosho");
            Assert.True(actual.Count() == 1);

            var actualGroupName = actual.ToList().First().Name;
            Assert.Equal(group2.Name, actualGroupName);
        }

        [Fact]
        public async Task GetUserGroupsAsync_ShouldReturn_JoinedGroup()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group1 = new Group()
            {
                Name = "GroupWhereUserIsMember"
            };
            group1.Members.Add(new UserGroup()
            {
                ConfirmedMember = true,
                Group = group1,
                User = user
            });

            var group2 = new Group()
            {
                Name = "GroupWhereUserIsNotMember"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group1);
            await this.dbContext.Groups.AddAsync(group2);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.GetUserGroupsAsync("Gosho");
            Assert.True(actual.Count() == 1);

            var actualGroupName = actual.ToList().First().Name;
            Assert.Equal(group1.Name, actualGroupName);
        }
        [Fact]
        public async Task PrepareGroupForViewAsync_ShouldReturn_ValidView()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "GroupWhereUserIsMember"
            };
            group.Members.Add(new UserGroup()
            {
                ConfirmedMember = true,
                Group = group,
                User = user
            });
            group.Admins.Add(new GroupAdmin()
            {
                AdminType = GroupAdminType.Admin,
                Group = group,
                User = user
            });
            group.Creator = user;

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.PrepareGroupForViewAsync(group.Name, user.UserName);
            Assert.True(actual.IsUserConfirmed);
            Assert.True(actual.IsAdministrator);
            Assert.True(actual.IsOwner);
            Assert.True(actual.Members.Count == 1);
            Assert.Equal(group.Name, actual.Name);
        }

        [Fact]
        public async Task IsUserMemberOfGroupAsync_ShouldReturn_True()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "GroupWhereUserIsMember"
            };
            group.Members.Add(new UserGroup()
            {
                ConfirmedMember = true,
                Group = group,
                User = user
            });

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.IsUserMemberOfGroupAsync(group.Name, user.UserName);
            Assert.True(actual);
        }
        [Fact]
        public async Task IsUserHaveRoleAsync_ShouldReturn_ValidRoles()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "GroupWhereUserIsMember"
            };
            group.Members.Add(new UserGroup()
            {
                ConfirmedMember = true,
                Group = group,
                User = user
            });
            group.Admins.Add(new GroupAdmin()
            {
                AdminType = GroupAdminType.Admin,
                User = user
            });

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual1 = await this.groupService.IsUserHaveRoleAsync(group.Name, user.UserName, "Admin");
            Assert.True(actual1);

            group.Admins.Clear();
            group.Admins.Add(new GroupAdmin()
            {
                AdminType = GroupAdminType.Moderator,
                User = user
            });
            await this.dbContext.SaveChangesAsync();

            var actual2 = await this.groupService.IsUserHaveRoleAsync(group.Name, user.UserName, "Moderator");
            Assert.True(actual2);
        }
        [Fact]
        public async Task JoinGroupAsync_ShouldAdd_UserToGroup()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "GroupWhereUserIsMember"
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.JoinGroupAsync(group.Name, user.UserName);
            Assert.True(actual);

            var actualUser = (await this.dbContext.UserGroups.SingleOrDefaultAsync(x => x.User == user)).User;
            var actualGroup = (await this.dbContext.UserGroups.SingleOrDefaultAsync(x => x.User == user)).Group;
            Assert.Equal(user.UserName, actualUser.UserName);
            Assert.Equal(group.Name, actualGroup.Name);
        }
        [Fact]
        public async Task AcceptMemberAsync_ShouldAccept_PendingMember()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "GroupWhereUserIsMember"
            };
            group.Members.Add(new UserGroup()
            {
                ConfirmedMember = false,
                Group = group,
                User = user
            });

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.AcceptMemberAsync(group.Name, user.UserName);
            Assert.True(actual);

            var actualUserGroup = (await this.dbContext.UserGroups.SingleOrDefaultAsync(x => x.User == user));
            Assert.True(actualUserGroup.ConfirmedMember);
        }
        [Fact]
        public async Task KickMemberAsync_ShouldRemove_ConfirmedMember()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "GroupWhereUserIsMember"
            };
            group.Members.Add(new UserGroup()
            {
                ConfirmedMember = true,
                Group = group,
                User = user
            });

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.KickMemberAsync(group.Name, user.UserName);
            Assert.True(actual);

            var actualUserGroup = (await this.dbContext.UserGroups.SingleOrDefaultAsync(x => x.User == user));
            Assert.Null(actualUserGroup);
        }
        [Fact]
        public async Task GetGroupMembersAsync_ShouldReturn_AllGroupMembers()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "GroupWithMultipleUsers"
            };
            group.Members.Add(new UserGroup()
            {
                ConfirmedMember = true,
                Group = group,
                User = user
            });
            group.Members.Add(new UserGroup()
            {
                ConfirmedMember = true,
                Group = group,
                User = user2
            });
            group.Admins.Add(new GroupAdmin()
            {
                AdminType = GroupAdminType.Admin,
                User = user,
                Group = group
            });

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.GetGroupMembersAsync(group.Name);
            Assert.True(actual.Count() == 2);

            var actualUserGroup1 = actual.First();
            var actualUserGroup2 = actual.Last();
            Assert.Equal(user.UserName, actualUserGroup1.Username);
            Assert.Equal(user2.UserName, actualUserGroup2.Username);
            Assert.Equal(GroupAdminType.Admin, actualUserGroup1.Elevation);
        }
        [Fact]
        public async Task GetGroupOwnerAsync_ShouldReturn_CorrectOwner()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "Group",
                Creator = user
            };

            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.GetGroupOwnerAsync(group.Name);
            Assert.Equal(user.UserName,actual);
        }
        [Fact]
        public async Task IsUserHaveAnyAdministratorRightsAsync_ShouldReturn_CorrectValues()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000"
            };
            var user3 = new OurTraceUser()
            {
                UserName = "Mitko",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "MITKO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "Group"
            };
            group.Admins.Add(new GroupAdmin()
            {
                AdminType = GroupAdminType.Moderator,
                User = user,
                Group = group
            });
            group.Admins.Add(new GroupAdmin()
            {
                AdminType = GroupAdminType.Admin,
                User = user2,
                Group = group
            });


            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual1 = await this.groupService.IsUserHaveAnyAdministratorRightsAsync(group.Name,user.UserName);
            var actual2 = await this.groupService.IsUserHaveAnyAdministratorRightsAsync(group.Name,user2.UserName);
            var actual3 = await this.groupService.IsUserHaveAnyAdministratorRightsAsync(group.Name,user3.UserName);
            Assert.True(actual1);
            Assert.True(actual2);
            Assert.False(actual3);
        }
        [Fact]
        public async Task GetUserRoleAsElevationAsync_ShouldReturn_CorrectValues()
        {
            var user = new OurTraceUser()
            {
                UserName = "Gosho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "GOSHO",
                PasswordHash = "00000"
            };
            var user2 = new OurTraceUser()
            {
                UserName = "Pesho",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "PESHO",
                PasswordHash = "00000"
            };
            var user3 = new OurTraceUser()
            {
                UserName = "Mitko",
                BirthDate = DateTime.Now,
                Country = "Bulgaria",
                Email = "abv@abv.bg",
                NormalizedEmail = "ABV@ABV.BG",
                NormalizedUserName = "MITKO",
                PasswordHash = "00000"
            };
            var group = new Group()
            {
                Name = "Group"
            };
            group.Admins.Add(new GroupAdmin()
            {
                AdminType = GroupAdminType.Moderator,
                User = user,
                Group = group
            });
            group.Admins.Add(new GroupAdmin()
            {
                AdminType = GroupAdminType.Admin,
                User = user2,
                Group = group
            });


            await this.dbContext.Users.AddAsync(user);
            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual1 = await this.groupService.GetUserRoleAsElevationAsync(group.Name, user.UserName);
            var actual2 = await this.groupService.GetUserRoleAsElevationAsync(group.Name, user2.UserName);
            var actual3 = await this.groupService.GetUserRoleAsElevationAsync(group.Name, user3.UserName);
            Assert.Equal((int)GroupAdminType.Moderator,actual1);
            Assert.Equal((int)GroupAdminType.Admin, actual2);
            Assert.Equal(0,actual3);
        }
        [Fact]
        public async Task GroupExistAsync_ShouldReturn_True()
        {
            var group = new Group()
            {
                Name = "Group"
            };

            await this.dbContext.Groups.AddAsync(group);
            await this.dbContext.SaveChangesAsync();

            var actual = await this.groupService.GroupExistAsync(group.Name);
            Assert.True(actual);
        }
    }
}
