using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OurTrace.App.Models.InputModels.Posts;
using OurTrace.Data;
using OurTrace.Data.Identity.Models;
using OurTrace.Data.Models;
using OurTrace.Services.Abstraction;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OurTrace.Services.Seeding
{
    public class OurTraceDbContextSeeder : ISeeder
    {
        private readonly OurTraceDbContext dbContext;
        private readonly IAdvertService advertService;
        private readonly IGroupService groupService;
        private readonly IMessageService messageService;
        private readonly IPostService postService;
        private readonly IRelationsService relationsService;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        private readonly UserManager<OurTraceUser> userManager;
        private readonly IFileService fileService;

        public OurTraceDbContextSeeder(OurTraceDbContext dbContext,
            IAdvertService advertService,
            IGroupService groupService,
            IMessageService messageService,
            IPostService postService,
            IRelationsService relationsService,
            IUserService userService,
            IRoleService roleService,
            UserManager<OurTraceUser> userManager,
            IFileService fileService)
        {
            this.dbContext = dbContext;
            this.advertService = advertService;
            this.groupService = groupService;
            this.messageService = messageService;
            this.postService = postService;
            this.relationsService = relationsService;
            this.userService = userService;
            this.roleService = roleService;
            this.userManager = userManager;
            this.fileService = fileService;
        }

        public async Task SeedAdmin()
        {
            var adminUser = new OurTraceUser()
            {
                UserName = "Admin",
                FullName = "Admincho Adminchov",
                BirthDate = new DateTime(1992, 2, 10),
                Email = "admincho@abv.bg",
                Sex = UserSex.Male,
                Country = "Bulgaria"
            };
            await this.userManager.CreateAsync(adminUser, "111111");
            await this.roleService.CreateRoleAsync("Admin");
            await this.roleService.AssignRoleAsync("Admin", "Admin");
        }
        public async Task SeedAdminFriendships(int skip, int take)
        {
            var users = await this.dbContext.Users
                .Where(x => x.UserName != "Admin")
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            foreach (var user in users)
            {
                await this.relationsService.AddFriendshipAsync("Admin", user.UserName);
                await this.relationsService.AddFriendshipAsync(user.UserName, "Admin");
            }
        }
        public async Task SeedUsers()
        {
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Barbabas", Email = "boulett0@cmu.edu", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2002-08-04 05:16:50"), FullName = "Barbabas Oulett" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Hobard", Email = "hdisdel1@freewebs.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("1995-05-20 05:32:33"), FullName = "Hobard Disdel" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rupert", Email = "rshellcross2@hc360.com", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1993-08-12 09:07:07"), FullName = "Rupert Shellcross" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Stefa", Email = "skehoe3@yandex.ru", Sex = UserSex.Female, Country = "Sweden", BirthDate = DateTime.Parse("1999-06-03 11:47:37"), FullName = "Stefa Kehoe" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Tomlin", Email = "tlondsdale4@tumblr.com", Sex = UserSex.Male, Country = "Poland", BirthDate = DateTime.Parse("1993-07-31 05:19:38"), FullName = "Tomlin Londsdale" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jerry", Email = "jproswell5@altervista.org", Sex = UserSex.Female, Country = "Thailand", BirthDate = DateTime.Parse("1999-01-21 21:34:12"), FullName = "Jerry Proswell" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ahmad", Email = "abuddington6@twitter.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2004-01-21 17:08:33"), FullName = "Ahmad Buddington" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Orsa", Email = "ofolan7@wiley.com", Sex = UserSex.Female, Country = "Ethiopia", BirthDate = DateTime.Parse("1999-06-13 22:48:18"), FullName = "Orsa Folan" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Anneliese", Email = "agarahan8@answers.com", Sex = UserSex.Female, Country = "Haiti", BirthDate = DateTime.Parse("2003-06-14 00:13:28"), FullName = "Anneliese Garahan" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Bernette", Email = "bfrankish9@google.com.br", Sex = UserSex.Female, Country = "Malta", BirthDate = DateTime.Parse("2005-04-23 01:31:45"), FullName = "Bernette Frankish" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Chrystel", Email = "chailesa@cisco.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2001-06-01 03:55:36"), FullName = "Chrystel Hailes" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Reynard", Email = "rrainsb@plala.or.jp", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("2001-02-07 11:10:51"), FullName = "Reynard Rains" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Carr", Email = "cminettc@soundcloud.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1996-11-02 03:01:41"), FullName = "Carr Minett" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Laurie", Email = "lnelseyd@csmonitor.com", Sex = UserSex.Female, Country = "Thailand", BirthDate = DateTime.Parse("1998-10-12 21:51:43"), FullName = "Laurie Nelsey" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Janaye", Email = "jhamnete@trellian.com", Sex = UserSex.Female, Country = "Poland", BirthDate = DateTime.Parse("1998-08-15 02:18:37"), FullName = "Janaye Hamnet" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Chantal", Email = "cgreenacref@accuweather.com", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("1999-10-10 18:16:47"), FullName = "Chantal Greenacre" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Shell", Email = "snorrisg@angelfire.com", Sex = UserSex.Female, Country = "Vietnam", BirthDate = DateTime.Parse("1996-02-19 15:09:29"), FullName = "Shell Norris" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Urson", Email = "uhollingdaleh@buzzfeed.com", Sex = UserSex.Male, Country = "Madagascar", BirthDate = DateTime.Parse("1995-01-16 17:51:23"), FullName = "Urson Hollingdale" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Devina", Email = "dislepi@nbcnews.com", Sex = UserSex.Female, Country = "Portugal", BirthDate = DateTime.Parse("2005-05-06 11:40:08"), FullName = "Devina Islep" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Geoffry", Email = "gstanneyj@latimes.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("1998-12-08 21:40:05"), FullName = "Geoffry Stanney" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Drake", Email = "dpippink@hhs.gov", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2002-12-14 14:46:08"), FullName = "Drake Pippin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Herc", Email = "hsimoninl@is.gd", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("2005-02-08 05:10:50"), FullName = "Herc Simonin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Tybalt", Email = "tsprittm@cafepress.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1998-02-07 22:41:44"), FullName = "Tybalt Spritt" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Aguie", Email = "achesshyren@hostgator.com", Sex = UserSex.Male, Country = "Morocco", BirthDate = DateTime.Parse("1996-12-28 21:12:25"), FullName = "Aguie Chesshyre" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Vergil", Email = "vcairneyo@who.int", Sex = UserSex.Male, Country = "Germany", BirthDate = DateTime.Parse("2005-08-11 03:40:52"), FullName = "Vergil Cairney" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sheelah", Email = "sstillwellp@sohu.com", Sex = UserSex.Female, Country = "Mongolia", BirthDate = DateTime.Parse("2002-09-14 00:02:09"), FullName = "Sheelah Stillwell" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Nadeen", Email = "nconnueq@barnesandnoble.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2002-02-23 11:08:27"), FullName = "Nadeen Connue" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Dolf", Email = "dhappelr@google.co.jp", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1997-08-27 02:13:12"), FullName = "Dolf Happel" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jaquenette", Email = "jnendicks@1688.com", Sex = UserSex.Female, Country = "Kyrgyzstan", BirthDate = DateTime.Parse("2001-08-25 03:23:57"), FullName = "Jaquenette Nendick" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ollie", Email = "otanzert@google.com.hk", Sex = UserSex.Male, Country = "Bolivia", BirthDate = DateTime.Parse("2001-11-09 14:43:38"), FullName = "Ollie Tanzer" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sabine", Email = "swallageu@cpanel.net", Sex = UserSex.Female, Country = "Sweden", BirthDate = DateTime.Parse("1994-04-07 09:20:58"), FullName = "Sabine Wallage" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sabrina", Email = "sinnmanv@theatlantic.com", Sex = UserSex.Female, Country = "Greece", BirthDate = DateTime.Parse("1998-11-10 11:22:23"), FullName = "Sabrina Innman" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Audy", Email = "agerritzenw@shutterfly.com", Sex = UserSex.Female, Country = "Japan", BirthDate = DateTime.Parse("1993-04-02 07:30:08"), FullName = "Audy Gerritzen" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lorin", Email = "lmennellx@tinypic.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1999-11-08 21:00:11"), FullName = "Lorin Mennell" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Elane", Email = "eprycey@amazon.de", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1999-08-12 17:13:59"), FullName = "Elane Pryce" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Gherardo", Email = "gmcarthurz@cmu.edu", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1996-10-18 12:19:09"), FullName = "Gherardo McArthur" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Winonah", Email = "wpurkis11@wikipedia.org", Sex = UserSex.Female, Country = "Uzbekistan", BirthDate = DateTime.Parse("2001-07-15 13:22:36"), FullName = "Winonah Purkis" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lamond", Email = "llevermore12@hibu.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1994-07-17 23:06:42"), FullName = "Lamond Levermore" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Margareta", Email = "mkenwrick13@theglobeandmail.com", Sex = UserSex.Female, Country = "Tunisia", BirthDate = DateTime.Parse("1996-11-16 09:03:08"), FullName = "Margareta Kenwrick" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ogden", Email = "odimbleby14@macromedia.com", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("2000-08-07 10:37:15"), FullName = "Ogden Dimbleby" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Tadeas", Email = "tkemell15@alexa.com", Sex = UserSex.Male, Country = "Portugal", BirthDate = DateTime.Parse("1994-12-14 07:24:19"), FullName = "Tadeas Kemell" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ermanno", Email = "ecrichmer16@nasa.gov", Sex = UserSex.Male, Country = "Portugal", BirthDate = DateTime.Parse("1997-07-08 00:51:20"), FullName = "Ermanno Crichmer" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Chrysler", Email = "cjochens17@kickstarter.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("2000-03-29 23:47:18"), FullName = "Chrysler Jochens" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Putnam", Email = "pjaspar1a@list-manage.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2002-06-12 21:38:45"), FullName = "Putnam Jaspar" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Brod", Email = "bgarlicke1b@nba.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2004-08-20 23:10:59"), FullName = "Brod Garlicke" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Krysta", Email = "kstanwix1c@japanpost.jp", Sex = UserSex.Female, Country = "Nigeria", BirthDate = DateTime.Parse("1998-09-01 03:37:16"), FullName = "Krysta Stanwix" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Tresa", Email = "tspolton1d@amazonaws.com", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("1995-03-11 10:02:59"), FullName = "Tresa Spolton" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ogdon", Email = "ocorris1e@squarespace.com", Sex = UserSex.Male, Country = "Sweden", BirthDate = DateTime.Parse("1998-04-10 03:52:38"), FullName = "Ogdon Corris" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Aurthur", Email = "aroome1f@taobao.com", Sex = UserSex.Male, Country = "Portugal", BirthDate = DateTime.Parse("1992-10-06 12:25:54"), FullName = "Aurthur Roome" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Donetta", Email = "dbisgrove1g@abc.net.au", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("1998-06-06 01:31:46"), FullName = "Donetta Bisgrove" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Hebert", Email = "hroskrug1h@spiegel.de", Sex = UserSex.Male, Country = "Azerbaijan", BirthDate = DateTime.Parse("1999-12-02 18:07:00"), FullName = "Hebert Roskrug" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Arnaldo", Email = "agarrish1i@yale.edu", Sex = UserSex.Male, Country = "Belarus", BirthDate = DateTime.Parse("2000-06-08 01:22:32"), FullName = "Arnaldo Garrish" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Kaitlin", Email = "kmclugaish1j@slashdot.org", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2002-02-01 05:51:56"), FullName = "Kaitlin McLugaish" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Suzy", Email = "sshirland1k@bloglovin.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("2003-02-25 12:43:25"), FullName = "Suzy Shirland" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Betteann", Email = "bseabrocke1l@w3.org", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2002-09-29 22:44:13"), FullName = "Betteann Seabrocke" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Arel", Email = "abraybrookes1m@gov.uk", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2005-04-26 23:24:09"), FullName = "Arel Braybrookes" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Trumaine", Email = "tbulloch1o@ihg.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1994-07-11 06:15:29"), FullName = "Trumaine Bulloch" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Phil", Email = "pfilisov1p@apple.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("1996-07-28 16:27:43"), FullName = "Phil Filisov" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Carline", Email = "cstickney1r@meetup.com", Sex = UserSex.Female, Country = "Venezuela", BirthDate = DateTime.Parse("2002-05-12 20:07:04"), FullName = "Carline Stickney" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Vincenty", Email = "vhuglin1s@vimeo.com", Sex = UserSex.Male, Country = "Pakistan", BirthDate = DateTime.Parse("1996-11-11 02:44:03"), FullName = "Vincenty Huglin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Eustacia", Email = "ehedden1v@diigo.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2002-09-28 01:20:30"), FullName = "Eustacia Hedden" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Steven", Email = "stebb1w@moonfruit.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1997-09-21 09:52:11"), FullName = "Steven Tebb" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Allegra", Email = "ajosipovitz1x@techcrunch.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1992-12-08 07:11:02"), FullName = "Allegra Josipovitz" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Aubrey", Email = "acastellani1y@t.co", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("2001-03-12 05:46:29"), FullName = "Aubrey Castellani" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Gal", Email = "garnaudin1z@cnn.com", Sex = UserSex.Male, Country = "Brazil", BirthDate = DateTime.Parse("2002-08-28 22:02:57"), FullName = "Gal Arnaudin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Brigitte", Email = "bdoornbos20@admin.ch", Sex = UserSex.Female, Country = "France", BirthDate = DateTime.Parse("1996-11-16 13:07:17"), FullName = "Brigitte Doornbos" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Glennie", Email = "gpaolino21@sitemeter.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2002-03-04 15:33:57"), FullName = "Glennie Paolino" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Edythe", Email = "enelle22@usatoday.com", Sex = UserSex.Female, Country = "Argentina", BirthDate = DateTime.Parse("1998-01-07 20:29:52"), FullName = "Edythe Nelle" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Nevins", Email = "ncranston23@de.vu", Sex = UserSex.Male, Country = "Canada", BirthDate = DateTime.Parse("1995-10-15 11:03:15"), FullName = "Nevins Cranston" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Itch", Email = "iedwards24@cloudflare.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("2005-06-19 09:02:10"), FullName = "Itch Edwards" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ivette", Email = "ipalin25@unesco.org", Sex = UserSex.Female, Country = "Spain", BirthDate = DateTime.Parse("2001-08-29 00:38:55"), FullName = "Ivette Palin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Reuven", Email = "redlyn26@xing.com", Sex = UserSex.Male, Country = "Comoros", BirthDate = DateTime.Parse("2004-02-19 11:07:21"), FullName = "Reuven Edlyn" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Reg", Email = "rbeddon27@youtu.be", Sex = UserSex.Male, Country = "Afghanistan", BirthDate = DateTime.Parse("2005-08-22 17:10:54"), FullName = "Reg Beddon" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Wenona", Email = "wforrestall28@feedburner.com", Sex = UserSex.Female, Country = "Portugal", BirthDate = DateTime.Parse("1998-01-30 11:12:36"), FullName = "Wenona Forrestall" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Timi", Email = "taskey29@ebay.co.uk", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1992-12-04 04:14:35"), FullName = "Timi Askey" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Shem", Email = "sheggman2b@lycos.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("2001-04-20 10:57:31"), FullName = "Shem Heggman" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rosabella", Email = "rdimmock2c@is.gd", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("2000-01-24 22:21:40"), FullName = "Rosabella Dimmock" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lyndell", Email = "lbradley2d@about.me", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1995-11-28 09:21:53"), FullName = "Lyndell Bradley" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Pammi", Email = "pworwood2e@barnesandnoble.com", Sex = UserSex.Female, Country = "Peru", BirthDate = DateTime.Parse("1999-08-20 08:32:52"), FullName = "Pammi Worwood" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Antoine", Email = "agimblet2f@vimeo.com", Sex = UserSex.Male, Country = "Argentina", BirthDate = DateTime.Parse("1997-08-31 15:54:52"), FullName = "Antoine Gimblet" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Iorgos", Email = "iblueman2g@amazonaws.com", Sex = UserSex.Male, Country = "Peru", BirthDate = DateTime.Parse("1995-08-02 11:52:21"), FullName = "Iorgos Blueman" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ardeen", Email = "alabba2h@drupal.org", Sex = UserSex.Female, Country = "Iran", BirthDate = DateTime.Parse("1996-06-22 07:49:09"), FullName = "Ardeen Labba" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Eli", Email = "ejosefsen2j@merriam-webster.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("2005-05-01 09:51:11"), FullName = "Eli Josefsen" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Pepita", Email = "pmillership2k@amazon.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1998-06-10 16:19:36"), FullName = "Pepita Millership" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Barnard", Email = "brozier2l@cnet.com", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1995-09-11 17:35:58"), FullName = "Barnard Rozier" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sterne", Email = "sgimson2m@fastcompany.com", Sex = UserSex.Male, Country = "Colombia", BirthDate = DateTime.Parse("2002-12-25 19:30:00"), FullName = "Sterne Gimson" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Welsh", Email = "wfozard2n@moonfruit.com", Sex = UserSex.Male, Country = "Japan", BirthDate = DateTime.Parse("1995-05-12 09:33:45"), FullName = "Welsh Fozard" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Greggory", Email = "gpapachristophorou2o@comcast.net", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("2000-04-24 18:23:27"), FullName = "Greggory Papachristophorou" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Theodosia", Email = "tamberg2p@sbwire.com", Sex = UserSex.Female, Country = "France", BirthDate = DateTime.Parse("1998-06-25 18:41:14"), FullName = "Theodosia Amberg" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Dre", Email = "dpendleberry2q@opera.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1997-03-03 12:31:54"), FullName = "Dre Pendleberry" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Abie", Email = "aaskaw2r@furl.net", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1993-01-18 23:37:08"), FullName = "Abie Askaw" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Toby", Email = "tferrie2s@liveinternet.ru", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2001-11-28 01:35:30"), FullName = "Toby Ferrie" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Hana", Email = "hcaughey2t@lycos.com", Sex = UserSex.Female, Country = "Netherlands", BirthDate = DateTime.Parse("1997-08-09 14:19:33"), FullName = "Hana Caughey" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Donnie", Email = "dblackwood2u@google.co.uk", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("2002-01-11 20:19:13"), FullName = "Donnie Blackwood" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Monica", Email = "mbatchellor2v@dmoz.org", Sex = UserSex.Female, Country = "Ukraine", BirthDate = DateTime.Parse("1995-03-19 06:56:08"), FullName = "Monica Batchellor" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Chloris", Email = "chuband2w@lulu.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1995-02-28 23:58:37"), FullName = "Chloris Huband" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Clarette", Email = "cmcilmorow2x@lulu.com", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("1993-02-27 18:46:55"), FullName = "Clarette McIlmorow" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Emmett", Email = "ebartlomiej2y@ow.ly", Sex = UserSex.Male, Country = "France", BirthDate = DateTime.Parse("1993-03-09 22:42:06"), FullName = "Emmett Bartlomiej" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Cyrillus", Email = "cknibb2z@over-blog.com", Sex = UserSex.Male, Country = "Greece", BirthDate = DateTime.Parse("2003-04-27 15:08:34"), FullName = "Cyrillus Knibb" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Brigitta", Email = "bmatiebe33@dion.ne.jp", Sex = UserSex.Female, Country = "Poland", BirthDate = DateTime.Parse("2003-10-26 22:30:11"), FullName = "Brigitta Matiebe" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Eddie", Email = "esamsin34@amazon.co.jp", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1997-10-09 14:13:49"), FullName = "Eddie Samsin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Todd", Email = "tgoldup35@naver.com", Sex = UserSex.Male, Country = "Japan", BirthDate = DateTime.Parse("1998-07-12 12:48:56"), FullName = "Todd Goldup" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Staffard", Email = "sledrun36@amazon.de", Sex = UserSex.Male, Country = "Armenia", BirthDate = DateTime.Parse("1992-11-02 02:13:19"), FullName = "Staffard Ledrun" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Brenda", Email = "bitzakovitz37@nifty.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1996-07-29 03:16:42"), FullName = "Brenda Itzakovitz" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Neal", Email = "nbosward38@state.tx.us", Sex = UserSex.Male, Country = "Greece", BirthDate = DateTime.Parse("1996-02-24 11:40:34"), FullName = "Neal Bosward" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rozalin", Email = "rslegg39@usa.gov", Sex = UserSex.Female, Country = "Portugal", BirthDate = DateTime.Parse("1997-06-02 03:42:26"), FullName = "Rozalin Slegg" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Maribeth", Email = "mmerrien3a@tuttocitta.it", Sex = UserSex.Female, Country = "Cyprus", BirthDate = DateTime.Parse("2005-06-10 20:45:33"), FullName = "Maribeth Merrien" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Windy", Email = "wlemmer3b@fema.gov", Sex = UserSex.Female, Country = "Argentina", BirthDate = DateTime.Parse("1993-10-17 23:03:51"), FullName = "Windy Lemmer" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Joanie", Email = "jdog3c@acquirethisname.com", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("2003-07-23 23:39:01"), FullName = "Joanie Dog" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Aurie", Email = "apenwarden3d@walmart.com", Sex = UserSex.Female, Country = "Brazil", BirthDate = DateTime.Parse("2002-06-16 05:12:25"), FullName = "Aurie Penwarden" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Loella", Email = "lpayn3e@a8.net", Sex = UserSex.Female, Country = "Nigeria", BirthDate = DateTime.Parse("1992-10-14 16:47:09"), FullName = "Loella Payn" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Blanche", Email = "bbuer3f@ameblo.jp", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1994-07-27 14:58:32"), FullName = "Blanche Buer" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Hubey", Email = "hbroughton3g@cloudflare.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1998-06-01 05:55:04"), FullName = "Hubey Broughton" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Leonore", Email = "lpalleske3h@wordpress.org", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1994-11-22 22:16:03"), FullName = "Leonore Palleske" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Bertie", Email = "bnorcutt3i@google.pl", Sex = UserSex.Male, Country = "Armenia", BirthDate = DateTime.Parse("1997-06-11 14:57:56"), FullName = "Bertie Norcutt" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Murvyn", Email = "malesi3j@naver.com", Sex = UserSex.Male, Country = "Brazil", BirthDate = DateTime.Parse("1993-07-29 13:51:08"), FullName = "Murvyn Alesi" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Tait", Email = "tmee3k@bbc.co.uk", Sex = UserSex.Male, Country = "Japan", BirthDate = DateTime.Parse("2003-01-10 11:27:59"), FullName = "Tait Mee" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jocelyne", Email = "jmedhurst3l@xinhuanet.com", Sex = UserSex.Female, Country = "Finland", BirthDate = DateTime.Parse("2003-09-12 02:48:19"), FullName = "Jocelyne Medhurst" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lefty", Email = "lpech3m@people.com.cn", Sex = UserSex.Male, Country = "Poland", BirthDate = DateTime.Parse("1996-01-22 14:17:24"), FullName = "Lefty Pech" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Nelle", Email = "nedworthye3n@nih.gov", Sex = UserSex.Female, Country = "Oman", BirthDate = DateTime.Parse("1995-05-25 20:10:44"), FullName = "Nelle Edworthye" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rosanna", Email = "rwhitechurch3o@ucoz.ru", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1993-12-03 22:08:24"), FullName = "Rosanna Whitechurch" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Salem", Email = "sfurnival3p@mail.ru", Sex = UserSex.Male, Country = "Pakistan", BirthDate = DateTime.Parse("1996-11-17 04:25:21"), FullName = "Salem Furnival" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Mavis", Email = "mcourteney3q@usnews.com", Sex = UserSex.Female, Country = "Estonia", BirthDate = DateTime.Parse("2003-06-08 01:57:22"), FullName = "Mavis Courteney" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Roselia", Email = "rshapter3r@whitehouse.gov", Sex = UserSex.Female, Country = "France", BirthDate = DateTime.Parse("2001-02-10 18:30:13"), FullName = "Roselia Shapter" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ferguson", Email = "fdomenichelli3s@rediff.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2002-06-12 19:57:53"), FullName = "Ferguson Domenichelli" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Melany", Email = "mdubber3t@cnet.com", Sex = UserSex.Female, Country = "Brazil", BirthDate = DateTime.Parse("1996-05-27 00:40:19"), FullName = "Melany Dubber" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Adan", Email = "abrinklow3u@studiopress.com", Sex = UserSex.Male, Country = "Portugal", BirthDate = DateTime.Parse("1995-10-31 06:36:05"), FullName = "Adan Brinklow" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Aristotle", Email = "abanbrigge3v@scribd.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("2005-01-21 06:06:23"), FullName = "Aristotle Banbrigge" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Darrell", Email = "dferron3w@instagram.com", Sex = UserSex.Male, Country = "Ukraine", BirthDate = DateTime.Parse("1995-05-23 11:50:34"), FullName = "Darrell Ferron" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ninnette", Email = "nvasyutkin3x@cbsnews.com", Sex = UserSex.Female, Country = "Poland", BirthDate = DateTime.Parse("1993-09-06 08:56:06"), FullName = "Ninnette Vasyutkin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Felisha", Email = "fbirtley3y@house.gov", Sex = UserSex.Female, Country = "Ukraine", BirthDate = DateTime.Parse("2001-01-12 07:39:35"), FullName = "Felisha Birtley" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Mervin", Email = "mperchard3z@zimbio.com", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1994-05-24 11:05:15"), FullName = "Mervin Perchard" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Karlotte", Email = "kmaytom40@howstuffworks.com", Sex = UserSex.Female, Country = "Nigeria", BirthDate = DateTime.Parse("2000-09-11 05:55:27"), FullName = "Karlotte Maytom" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Gayle", Email = "gpreedy41@addthis.com", Sex = UserSex.Male, Country = "Azerbaijan", BirthDate = DateTime.Parse("1995-10-11 15:08:41"), FullName = "Gayle Preedy" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Willie", Email = "wthew42@de.vu", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1997-10-14 10:56:19"), FullName = "Willie Thew" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Paddie", Email = "pscyone43@github.com", Sex = UserSex.Male, Country = "Colombia", BirthDate = DateTime.Parse("1992-12-04 08:34:13"), FullName = "Paddie Scyone" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Burlie", Email = "bkinze44@weibo.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2000-06-12 02:40:16"), FullName = "Burlie Kinze" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Reynolds", Email = "rcoombe45@biblegateway.com", Sex = UserSex.Male, Country = "Ethiopia", BirthDate = DateTime.Parse("1993-07-07 11:55:01"), FullName = "Reynolds Coombe" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Barny", Email = "bdemkowicz47@nbcnews.com", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1998-12-31 22:21:33"), FullName = "Barny Demkowicz" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Valentine", Email = "vkinsley48@ehow.com", Sex = UserSex.Female, Country = "Canada", BirthDate = DateTime.Parse("2005-08-08 01:20:09"), FullName = "Valentine Kinsley" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Carmel", Email = "choys49@ustream.tv", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1994-01-14 03:18:15"), FullName = "Carmel Hoys" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Arnuad", Email = "ablinder4a@freewebs.com", Sex = UserSex.Male, Country = "Guatemala", BirthDate = DateTime.Parse("2001-08-24 17:40:36"), FullName = "Arnuad Blinder" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Joy", Email = "jcobbledick4b@blogspot.com", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("1997-09-21 02:47:09"), FullName = "Joy Cobbledick" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Brok", Email = "bsimeoni4c@weather.com", Sex = UserSex.Male, Country = "Japan", BirthDate = DateTime.Parse("2002-10-26 00:10:35"), FullName = "Brok Simeoni" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jaquenette", Email = "jjeggo4d@mac.com", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("1995-04-21 13:03:35"), FullName = "Jaquenette Jeggo" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jonis", Email = "jphilott4e@bing.com", Sex = UserSex.Female, Country = "Sweden", BirthDate = DateTime.Parse("1995-06-24 08:43:27"), FullName = "Jonis Philott" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Heidie", Email = "hjammet4g@vistaprint.com", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("1995-05-24 17:25:47"), FullName = "Heidie Jammet" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Cordelie", Email = "cmangan4h@pen.io", Sex = UserSex.Female, Country = "Serbia", BirthDate = DateTime.Parse("1992-12-13 22:17:59"), FullName = "Cordelie Mangan" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Nobe", Email = "nkleinplac4i@trellian.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2000-04-09 02:06:29"), FullName = "Nobe Kleinplac" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rory", Email = "rsampey4j@ehow.com", Sex = UserSex.Female, Country = "Nigeria", BirthDate = DateTime.Parse("1993-12-23 18:26:48"), FullName = "Rory Sampey" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Quinton", Email = "qdioniso4k@forbes.com", Sex = UserSex.Male, Country = "Slovenia", BirthDate = DateTime.Parse("2000-02-25 19:37:40"), FullName = "Quinton Dioniso" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ki", Email = "kde4l@paginegialle.it", Sex = UserSex.Female, Country = "Sweden", BirthDate = DateTime.Parse("1995-02-28 15:40:24"), FullName = "Ki De Benedetti" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Fredelia", Email = "fnewcom4n@dion.ne.jp", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("2004-09-01 13:48:49"), FullName = "Fredelia Newcom" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lonnie", Email = "lmenichillo4o@com.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1996-03-18 20:47:27"), FullName = "Lonnie Menichillo" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ray", Email = "rduns4p@tiny.cc", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("2000-11-08 11:39:29"), FullName = "Ray Duns" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Darrelle", Email = "dconrath4q@instagram.com", Sex = UserSex.Female, Country = "Sweden", BirthDate = DateTime.Parse("1992-11-07 13:26:49"), FullName = "Darrelle Conrath" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Morganica", Email = "mdennick4r@de.vu", Sex = UserSex.Female, Country = "Azerbaijan", BirthDate = DateTime.Parse("1993-11-30 18:09:03"), FullName = "Morganica Dennick" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Gilberte", Email = "gwanstall4s@soundcloud.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2000-10-07 06:30:56"), FullName = "Gilberte Wanstall" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Audry", Email = "amacgraith4t@prnewswire.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2001-07-30 10:40:21"), FullName = "Audry MacGraith" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Hazlett", Email = "htapscott4u@stumbleupon.com", Sex = UserSex.Male, Country = "Portugal", BirthDate = DateTime.Parse("1996-11-10 12:05:09"), FullName = "Hazlett Tapscott" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Dev", Email = "dfettis4v@ucsd.edu", Sex = UserSex.Male, Country = "Brazil", BirthDate = DateTime.Parse("2005-07-20 11:32:53"), FullName = "Dev Fettis" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Granthem", Email = "garndtsen4x@patch.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1995-10-21 15:15:42"), FullName = "Granthem Arndtsen" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lyn", Email = "lmanueau4y@blog.com", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("1997-08-24 01:22:00"), FullName = "Lyn Manueau" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Alexina", Email = "abutson4z@reference.com", Sex = UserSex.Female, Country = "Botswana", BirthDate = DateTime.Parse("1995-05-17 10:23:07"), FullName = "Alexina Butson" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Linnet", Email = "lkepp50@wufoo.com", Sex = UserSex.Female, Country = "Egypt", BirthDate = DateTime.Parse("2002-08-14 12:19:51"), FullName = "Linnet Kepp" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Nils", Email = "npohlak51@tinyurl.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2000-09-06 06:30:27"), FullName = "Nils Pohlak" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Luis", Email = "lbiswell52@mac.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1993-03-05 23:04:26"), FullName = "Luis Biswell" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Tansy", Email = "tdomleo54@simplemachines.org", Sex = UserSex.Female, Country = "Peru", BirthDate = DateTime.Parse("2002-02-25 00:20:37"), FullName = "Tansy Domleo" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Olivero", Email = "opiwall55@ameblo.jp", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2002-03-09 22:15:11"), FullName = "Olivero Piwall" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jamima", Email = "jwitnall57@printfriendly.com", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("2000-09-03 05:01:49"), FullName = "Jamima Witnall" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ansley", Email = "acullinane58@tripod.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2000-01-03 23:06:05"), FullName = "Ansley Cullinane" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ingram", Email = "imanwaring5a@seesaa.net", Sex = UserSex.Male, Country = "Mexico", BirthDate = DateTime.Parse("1995-04-15 23:53:04"), FullName = "Ingram Manwaring" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rudie", Email = "rdowse5c@wunderground.com", Sex = UserSex.Male, Country = "Brazil", BirthDate = DateTime.Parse("1996-07-18 23:59:07"), FullName = "Rudie Dowse" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Dannel", Email = "dhampshire5f@photobucket.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1993-04-07 06:10:57"), FullName = "Dannel Hampshire" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ronnie", Email = "rkirkman5g@wikimedia.org", Sex = UserSex.Female, Country = "Bulgaria", BirthDate = DateTime.Parse("2002-11-03 10:21:49"), FullName = "Ronnie Kirkman" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jake", Email = "jtwiddy5h@merriam-webster.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2002-06-15 12:08:23"), FullName = "Jake Twiddy" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Annabelle", Email = "aroskams5i@senate.gov", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1996-08-01 17:07:16"), FullName = "Annabelle Roskams" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Theadora", Email = "tmacshirrie5j@accuweather.com", Sex = UserSex.Female, Country = "Sweden", BirthDate = DateTime.Parse("1992-09-05 04:42:47"), FullName = "Theadora MacShirrie" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Edvard", Email = "eclementet5k@mashable.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1994-09-14 18:02:18"), FullName = "Edvard Clementet" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Berke", Email = "bdrohan5l@e-recht24.de", Sex = UserSex.Male, Country = "Paraguay", BirthDate = DateTime.Parse("1993-08-26 08:15:10"), FullName = "Berke Drohan" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Aubine", Email = "aewbach5m@who.int", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("2004-12-02 06:38:24"), FullName = "Aubine Ewbach" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Valry", Email = "vchern5n@xrea.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1998-07-09 00:16:19"), FullName = "Valry Chern" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Duffie", Email = "dlamdin5o@parallels.com", Sex = UserSex.Male, Country = "Vietnam", BirthDate = DateTime.Parse("1997-10-03 06:42:20"), FullName = "Duffie Lamdin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Wilek", Email = "wcricket5p@sohu.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1995-03-12 12:51:32"), FullName = "Wilek Cricket" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Obadias", Email = "omesser5q@bigcartel.com", Sex = UserSex.Male, Country = "Portugal", BirthDate = DateTime.Parse("1997-10-31 18:04:58"), FullName = "Obadias Messer" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jorgan", Email = "jwhitehurst5r@reddit.com", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("2004-03-15 19:00:21"), FullName = "Jorgan Whitehurst" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Nicki", Email = "nginn5s@youku.com", Sex = UserSex.Female, Country = "Brazil", BirthDate = DateTime.Parse("1995-08-30 12:12:30"), FullName = "Nicki Ginn" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ode", Email = "ogrinston5t@tripadvisor.com", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1992-09-19 10:15:37"), FullName = "Ode Grinston" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jay", Email = "jcastano5u@t-online.de", Sex = UserSex.Male, Country = "Finland", BirthDate = DateTime.Parse("2003-07-05 13:14:52"), FullName = "Jay Castano" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Fair", Email = "fsalvador5v@vinaora.com", Sex = UserSex.Male, Country = "Thailand", BirthDate = DateTime.Parse("1996-11-20 13:18:44"), FullName = "Fair Salvador" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lorenza", Email = "lgash5w@flickr.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("2005-08-05 16:33:56"), FullName = "Lorenza Gash" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Allan", Email = "amordecai5x@cloudflare.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1999-01-17 01:17:50"), FullName = "Allan Mordecai" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Aura", Email = "aoveril5y@opensource.org", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("1999-08-09 08:58:40"), FullName = "Aura Overil" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Inness", Email = "iseden5z@princeton.edu", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1999-12-22 06:45:50"), FullName = "Inness Seden" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Aubine", Email = "aekins60@gnu.org", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2002-01-10 19:25:03"), FullName = "Aubine Ekins" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Giulia", Email = "glipscombe61@umn.edu", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2003-04-07 02:46:31"), FullName = "Giulia Lipscombe" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Neddie", Email = "nhullbrook62@clickbank.net", Sex = UserSex.Male, Country = "Mauritania", BirthDate = DateTime.Parse("1999-03-31 11:44:54"), FullName = "Neddie Hullbrook" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Shawnee", Email = "ssazio63@ifeng.com", Sex = UserSex.Female, Country = "Japan", BirthDate = DateTime.Parse("1997-02-25 18:17:30"), FullName = "Shawnee Sazio" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Farly", Email = "fjakuszewski64@columbia.edu", Sex = UserSex.Male, Country = "France", BirthDate = DateTime.Parse("1993-12-29 21:24:41"), FullName = "Farly Jakuszewski" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Colline", Email = "cdyshart65@cornell.edu", Sex = UserSex.Female, Country = "Colombia", BirthDate = DateTime.Parse("1993-10-05 20:13:36"), FullName = "Colline Dyshart" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Shane", Email = "sspring67@uol.com.br", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2001-12-27 21:23:18"), FullName = "Shane Spring" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Gavin", Email = "gbogges68@stumbleupon.com", Sex = UserSex.Male, Country = "Cuba", BirthDate = DateTime.Parse("2000-01-08 03:05:18"), FullName = "Gavin Bogges" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Francine", Email = "fchang69@bloomberg.com", Sex = UserSex.Female, Country = "Nigeria", BirthDate = DateTime.Parse("1999-08-23 13:39:01"), FullName = "Francine Chang" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Gratiana", Email = "gboxall6a@imageshack.us", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2003-01-21 22:55:56"), FullName = "Gratiana Boxall" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Shalna", Email = "statton6b@soundcloud.com", Sex = UserSex.Female, Country = "Peru", BirthDate = DateTime.Parse("1994-04-17 23:39:12"), FullName = "Shalna Tatton" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Arly", Email = "afuentez6c@cyberchimps.com", Sex = UserSex.Female, Country = "Colombia", BirthDate = DateTime.Parse("1993-03-21 09:22:11"), FullName = "Arly Fuentez" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Cello", Email = "cmacallister6d@discovery.com", Sex = UserSex.Male, Country = "Tanzania", BirthDate = DateTime.Parse("2001-02-13 13:36:04"), FullName = "Cello MacAllister" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Pietrek", Email = "pbrinded6e@cornell.edu", Sex = UserSex.Male, Country = "Finland", BirthDate = DateTime.Parse("2004-12-08 07:26:34"), FullName = "Pietrek Brinded" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Chevy", Email = "cdrews6f@answers.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1995-07-26 17:31:30"), FullName = "Chevy Drews" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Der", Email = "dstaddom6g@time.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1995-08-19 04:12:03"), FullName = "Der Staddom" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Nannie", Email = "ngrisedale6h@shareasale.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1993-02-18 12:53:12"), FullName = "Nannie Grisedale" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Dian", Email = "dnewhouse6i@gmpg.org", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2005-04-04 10:43:10"), FullName = "Dian Newhouse" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sheilah", Email = "ssmooth6j@sciencedirect.com", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("2001-10-21 17:21:35"), FullName = "Sheilah Smooth" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Francoise", Email = "fjirik6k@patch.com", Sex = UserSex.Female, Country = "France", BirthDate = DateTime.Parse("1998-11-25 07:16:43"), FullName = "Francoise Jirik" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Obed", Email = "omather6l@mtv.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1999-06-25 15:08:03"), FullName = "Obed Mather" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ferdinande", Email = "fdeble6m@psu.edu", Sex = UserSex.Female, Country = "Sweden", BirthDate = DateTime.Parse("1999-05-07 06:24:56"), FullName = "Ferdinande Deble" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Wrennie", Email = "wjeste6n@mac.com", Sex = UserSex.Female, Country = "Colombia", BirthDate = DateTime.Parse("2003-09-23 22:03:43"), FullName = "Wrennie Jeste" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Budd", Email = "bveysey6o@imageshack.us", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2004-06-01 15:18:53"), FullName = "Budd Veysey" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Meade", Email = "mjanout6p@prnewswire.com", Sex = UserSex.Male, Country = "Malaysia", BirthDate = DateTime.Parse("2000-04-04 14:55:42"), FullName = "Meade Janout" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Allie", Email = "arudolfer6q@php.net", Sex = UserSex.Male, Country = "Jordan", BirthDate = DateTime.Parse("1999-11-13 17:58:39"), FullName = "Allie Rudolfer" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Agnesse", Email = "arobben6r@chronoengine.com", Sex = UserSex.Female, Country = "Colombia", BirthDate = DateTime.Parse("1997-11-04 00:33:32"), FullName = "Agnesse Robben" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Vidovic", Email = "vgiraudat6s@studiopress.com", Sex = UserSex.Male, Country = "Japan", BirthDate = DateTime.Parse("1993-01-28 12:53:40"), FullName = "Vidovic Giraudat" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Halsey", Email = "hmckimmey6t@google.co.jp", Sex = UserSex.Male, Country = "Colombia", BirthDate = DateTime.Parse("2002-01-31 05:45:24"), FullName = "Halsey McKimmey" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Carena", Email = "cterne6v@gravatar.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1993-07-10 13:40:40"), FullName = "Carena Terne" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Morty", Email = "mballard6w@ustream.tv", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("1997-04-12 15:36:42"), FullName = "Morty Ballard" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Arin", Email = "acockley6x@nytimes.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("1996-12-24 03:07:39"), FullName = "Arin Cockley" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Wood", Email = "wtennet6y@harvard.edu", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("2003-11-01 17:32:44"), FullName = "Wood Tennet" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Jaye", Email = "jmoodycliffe70@tamu.edu", Sex = UserSex.Male, Country = "Argentina", BirthDate = DateTime.Parse("2001-03-29 09:15:55"), FullName = "Jaye Moodycliffe" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Melonie", Email = "mgrimston71@springer.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1993-08-07 20:59:33"), FullName = "Melonie Grimston" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Care", Email = "cbritney72@forbes.com", Sex = UserSex.Male, Country = "Serbia", BirthDate = DateTime.Parse("1996-06-22 04:48:41"), FullName = "Care Britney" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ronna", Email = "rcecere73@google.co.uk", Sex = UserSex.Female, Country = "Thailand", BirthDate = DateTime.Parse("2001-04-13 01:47:45"), FullName = "Ronna Cecere" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Saba", Email = "syates74@chronoengine.com", Sex = UserSex.Female, Country = "Botswana", BirthDate = DateTime.Parse("1996-01-16 06:49:05"), FullName = "Saba Yates" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ame", Email = "adeverell75@wisc.edu", Sex = UserSex.Female, Country = "Brazil", BirthDate = DateTime.Parse("2001-06-02 05:00:46"), FullName = "Ame Deverell" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Flss", Email = "fkohtler77@deviantart.com", Sex = UserSex.Female, Country = "Greece", BirthDate = DateTime.Parse("2003-07-12 07:45:04"), FullName = "Flss Kohtler" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Essa", Email = "ecastanie78@canalblog.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1996-01-25 19:11:48"), FullName = "Essa Castanie" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Dell", Email = "dbugdale79@homestead.com", Sex = UserSex.Female, Country = "France", BirthDate = DateTime.Parse("1999-09-27 08:39:59"), FullName = "Dell Bugdale" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Olag", Email = "omcghee7a@omniture.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("1999-11-05 00:05:36"), FullName = "Olag McGhee" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Angeline", Email = "akeddie7c@wsj.com", Sex = UserSex.Female, Country = "Thailand", BirthDate = DateTime.Parse("1998-08-07 21:30:09"), FullName = "Angeline Keddie" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Marlene", Email = "msprowle7d@alexa.com", Sex = UserSex.Female, Country = "Mongolia", BirthDate = DateTime.Parse("1993-07-17 02:06:50"), FullName = "Marlene Sprowle" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Bonnibelle", Email = "bkarolczyk7e@kickstarter.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2005-06-28 22:52:27"), FullName = "Bonnibelle Karolczyk" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Allsun", Email = "asantarelli7f@pagesperso-orange.fr", Sex = UserSex.Female, Country = "Ethiopia", BirthDate = DateTime.Parse("1993-07-17 17:55:39"), FullName = "Allsun Santarelli" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Konstantine", Email = "kjzak7g@cdc.gov", Sex = UserSex.Male, Country = "Vietnam", BirthDate = DateTime.Parse("2005-01-07 17:59:36"), FullName = "Konstantine Jzak" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Damara", Email = "dtivnan7h@netscape.com", Sex = UserSex.Female, Country = "Sweden", BirthDate = DateTime.Parse("2002-06-04 14:29:46"), FullName = "Damara Tivnan" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ewen", Email = "eprestwich7i@deviantart.com", Sex = UserSex.Male, Country = "Japan", BirthDate = DateTime.Parse("2005-02-06 10:47:21"), FullName = "Ewen Prestwich" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Buckie", Email = "bborgnet7j@dropbox.com", Sex = UserSex.Male, Country = "Brazil", BirthDate = DateTime.Parse("2000-05-21 05:25:58"), FullName = "Buckie Borgnet" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Willie", Email = "wfonso7k@utexas.edu", Sex = UserSex.Female, Country = "Peru", BirthDate = DateTime.Parse("2003-04-18 18:51:58"), FullName = "Willie Fonso" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sandra", Email = "srichie7l@weather.com", Sex = UserSex.Female, Country = "Pakistan", BirthDate = DateTime.Parse("1996-09-01 00:28:26"), FullName = "Sandra Richie" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Branden", Email = "bmcfetridge7o@51.la", Sex = UserSex.Male, Country = "Germany", BirthDate = DateTime.Parse("1995-09-10 08:34:58"), FullName = "Branden McFetridge" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Abeu", Email = "abrett7q@tripod.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2004-02-28 06:59:34"), FullName = "Abeu Brett" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Abe", Email = "agracey7r@biglobe.ne.jp", Sex = UserSex.Male, Country = "Finland", BirthDate = DateTime.Parse("1999-02-08 00:29:16"), FullName = "Abe Gracey" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Pieter", Email = "pguerola7s@deviantart.com", Sex = UserSex.Male, Country = "Ecuador", BirthDate = DateTime.Parse("1998-05-18 02:36:43"), FullName = "Pieter Guerola" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lil", Email = "lemmatt7t@goo.ne.jp", Sex = UserSex.Female, Country = "Poland", BirthDate = DateTime.Parse("1994-10-15 01:12:40"), FullName = "Lil Emmatt" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Juliana", Email = "jprosser7u@washington.edu", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1997-04-05 16:16:14"), FullName = "Juliana Prosser" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Gusta", Email = "gmcgriffin7v@chronoengine.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2002-08-23 02:02:31"), FullName = "Gusta McGriffin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Amata", Email = "agypson7w@bluehost.com", Sex = UserSex.Female, Country = "Afghanistan", BirthDate = DateTime.Parse("1999-01-13 14:30:17"), FullName = "Amata Gypson" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ninnette", Email = "npetheridge7x@gizmodo.com", Sex = UserSex.Female, Country = "Ukraine", BirthDate = DateTime.Parse("1996-12-16 22:28:41"), FullName = "Ninnette Petheridge" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Augustin", Email = "acassie7y@wufoo.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2002-07-09 22:55:11"), FullName = "Augustin Cassie" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rollins", Email = "rfolomin7z@mashable.com", Sex = UserSex.Male, Country = "Argentina", BirthDate = DateTime.Parse("1994-07-08 14:10:54"), FullName = "Rollins Folomin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Leonid", Email = "lboow80@mac.com", Sex = UserSex.Male, Country = "Brazil", BirthDate = DateTime.Parse("1997-10-22 19:57:28"), FullName = "Leonid Boow" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Allis", Email = "amanass81@zimbio.com", Sex = UserSex.Female, Country = "Iran", BirthDate = DateTime.Parse("1998-07-28 11:09:42"), FullName = "Allis Manass" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Kerr", Email = "kglasscoo82@meetup.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("2004-06-16 02:38:37"), FullName = "Kerr Glasscoo" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ulrica", Email = "udawid83@over-blog.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("2002-12-27 11:35:28"), FullName = "Ulrica Dawid" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Hewet", Email = "hheams84@cbslocal.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("2004-02-18 14:15:51"), FullName = "Hewet Heams" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Granny", Email = "gmedhurst85@studiopress.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1996-06-04 04:13:27"), FullName = "Granny Medhurst" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Johnnie", Email = "jdoutch86@weebly.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("2003-12-23 19:49:28"), FullName = "Johnnie Doutch" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Suzy", Email = "spoolman87@hp.com", Sex = UserSex.Female, Country = "Indonesia", BirthDate = DateTime.Parse("1996-03-03 08:10:25"), FullName = "Suzy Poolman" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Dodi", Email = "dpiburn88@livejournal.com", Sex = UserSex.Female, Country = "Pakistan", BirthDate = DateTime.Parse("2003-08-13 20:54:13"), FullName = "Dodi Piburn" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Bride", Email = "bvedyashkin89@tripadvisor.com", Sex = UserSex.Female, Country = "Brazil", BirthDate = DateTime.Parse("1995-02-14 17:44:50"), FullName = "Bride Vedyashkin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Chauncey", Email = "cchina8a@jigsy.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("2002-04-02 22:22:10"), FullName = "Chauncey China" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sande", Email = "swoodbridge8b@engadget.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2000-10-29 12:09:25"), FullName = "Sande Woodbridge" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Darcy", Email = "dscopes8c@bing.com", Sex = UserSex.Female, Country = "Germany", BirthDate = DateTime.Parse("2003-03-22 22:04:07"), FullName = "Darcy Scopes" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Derrik", Email = "dofficer8d@imageshack.us", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("2000-04-25 11:34:01"), FullName = "Derrik Officer" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Carleton", Email = "cbirkin8e@omniture.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1996-07-22 13:30:47"), FullName = "Carleton Birkin" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Korey", Email = "kbirk8f@cdbaby.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1996-06-05 07:42:36"), FullName = "Korey Birk" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Avigdor", Email = "asnalum8g@nih.gov", Sex = UserSex.Male, Country = "Latvia", BirthDate = DateTime.Parse("1995-03-18 07:29:40"), FullName = "Avigdor Snalum" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sam", Email = "strudgian8h@hhs.gov", Sex = UserSex.Male, Country = "Mexico", BirthDate = DateTime.Parse("1997-01-26 13:11:26"), FullName = "Sam Trudgian" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Kare", Email = "kkirman8i@harvard.edu", Sex = UserSex.Female, Country = "Portugal", BirthDate = DateTime.Parse("2002-03-26 21:31:02"), FullName = "Kare Kirman" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Astra", Email = "ahassekl8j@ihg.com", Sex = UserSex.Female, Country = "Mexico", BirthDate = DateTime.Parse("1996-01-30 16:36:39"), FullName = "Astra Hassekl" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rossie", Email = "rstrelitzer8k@comsenz.com", Sex = UserSex.Male, Country = "Colombia", BirthDate = DateTime.Parse("1995-04-01 22:45:46"), FullName = "Rossie Strelitzer" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Wallas", Email = "wcullip8l@cocolog-nifty.com", Sex = UserSex.Male, Country = "Poland", BirthDate = DateTime.Parse("2000-02-14 10:26:16"), FullName = "Wallas Cullip" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Perri", Email = "paddie8m@geocities.com", Sex = UserSex.Female, Country = "Honduras", BirthDate = DateTime.Parse("1997-11-25 01:00:11"), FullName = "Perri Addie" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Terri", Email = "tfenney8n@businesswire.com", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1993-06-12 00:11:16"), FullName = "Terri Fenney" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Curr", Email = "cmckennan8o@google.es", Sex = UserSex.Male, Country = "Zambia", BirthDate = DateTime.Parse("1995-06-30 12:49:46"), FullName = "Curr McKennan" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Minda", Email = "mmansfield8p@booking.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2001-08-09 04:50:35"), FullName = "Minda Mansfield" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Shandie", Email = "scrisall8q@hubpages.com", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("2000-06-06 17:00:13"), FullName = "Shandie Crisall" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Amandy", Email = "amackinnon8r@github.io", Sex = UserSex.Female, Country = "Colombia", BirthDate = DateTime.Parse("2001-12-24 05:03:25"), FullName = "Amandy MacKinnon" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Arman", Email = "aarchbold8s@typepad.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("2003-05-25 11:32:48"), FullName = "Arman Archbold" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lelah", Email = "lwoodthorpe8t@economist.com", Sex = UserSex.Female, Country = "Argentina", BirthDate = DateTime.Parse("2000-09-10 16:50:38"), FullName = "Lelah Woodthorpe" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Cordy", Email = "cbratten8u@blogs.com", Sex = UserSex.Male, Country = "Norway", BirthDate = DateTime.Parse("1998-11-22 14:05:29"), FullName = "Cordy Bratten" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Bald", Email = "btattersall8v@hexun.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1998-04-21 13:04:22"), FullName = "Bald Tattersall" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Leontine", Email = "lreuven8w@elegantthemes.com", Sex = UserSex.Female, Country = "Poland", BirthDate = DateTime.Parse("2004-05-22 22:51:12"), FullName = "Leontine Reuven" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Marten", Email = "mplott8x@psu.edu", Sex = UserSex.Male, Country = "Poland", BirthDate = DateTime.Parse("2000-09-23 15:45:03"), FullName = "Marten Plott" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Abby", Email = "acoushe8y@google.co.uk", Sex = UserSex.Female, Country = "Gabon", BirthDate = DateTime.Parse("1993-05-05 05:44:09"), FullName = "Abby Coushe" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Cass", Email = "cgoulbourn8z@cnet.com", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1998-03-02 23:22:32"), FullName = "Cass Goulbourn" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Petronia", Email = "phabgood90@slideshare.net", Sex = UserSex.Female, Country = "Gambia", BirthDate = DateTime.Parse("1999-02-21 02:34:50"), FullName = "Petronia Habgood" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Shir", Email = "sgilmore91@wufoo.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1998-10-18 07:42:10"), FullName = "Shir Gilmore" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Ciro", Email = "cmcettigen92@reuters.com", Sex = UserSex.Male, Country = "Indonesia", BirthDate = DateTime.Parse("1998-11-28 10:00:23"), FullName = "Ciro McEttigen" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rand", Email = "rpeidro93@wix.com", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1997-06-19 08:48:19"), FullName = "Rand Peidro" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Guillemette", Email = "gattiwill94@myspace.com", Sex = UserSex.Female, Country = "France", BirthDate = DateTime.Parse("2005-04-06 08:18:43"), FullName = "Guillemette Attiwill" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Niles", Email = "nmckee96@ow.ly", Sex = UserSex.Male, Country = "Norway", BirthDate = DateTime.Parse("2004-10-27 03:32:06"), FullName = "Niles McKee" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Lyon", Email = "lphillippo97@webs.com", Sex = UserSex.Male, Country = "Ireland", BirthDate = DateTime.Parse("2002-02-28 10:22:21"), FullName = "Lyon Phillippo" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Alberik", Email = "apardue98@discovery.com", Sex = UserSex.Male, Country = "Japan", BirthDate = DateTime.Parse("1994-09-08 04:40:46"), FullName = "Alberik Pardue" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Meier", Email = "mnaerup99@dion.ne.jp", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1996-01-02 07:54:18"), FullName = "Meier Naerup" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Fredia", Email = "fchicchetto9a@netlog.com", Sex = UserSex.Female, Country = "Canada", BirthDate = DateTime.Parse("1996-09-18 23:34:12"), FullName = "Fredia Chicchetto" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Daphna", Email = "dmckerrow9b@typepad.com", Sex = UserSex.Female, Country = "Philippines", BirthDate = DateTime.Parse("1999-02-27 22:40:28"), FullName = "Daphna McKerrow" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rosanne", Email = "rwyss9c@sphinn.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1997-10-06 20:33:04"), FullName = "Rosanne Wyss" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Essa", Email = "ebowkley9d@technorati.com", Sex = UserSex.Female, Country = "Malta", BirthDate = DateTime.Parse("2004-10-06 12:22:56"), FullName = "Essa Bowkley" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Dagny", Email = "dghidotti9f@salon.com", Sex = UserSex.Male, Country = "Sweden", BirthDate = DateTime.Parse("1996-02-02 12:25:33"), FullName = "Dagny Ghidotti" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Homere", Email = "hgladbeck9g@virginia.edu", Sex = UserSex.Male, Country = "Egypt", BirthDate = DateTime.Parse("2004-10-16 17:50:55"), FullName = "Homere Gladbeck" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Paquito", Email = "pblayd9h@gmpg.org", Sex = UserSex.Male, Country = "China", BirthDate = DateTime.Parse("1994-01-20 21:17:39"), FullName = "Paquito Blayd" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sigismund", Email = "skhoter9i@chronoengine.com", Sex = UserSex.Male, Country = "Brazil", BirthDate = DateTime.Parse("2004-08-08 11:30:36"), FullName = "Sigismund Khoter" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Theo", Email = "tdulinty9j@yahoo.co.jp", Sex = UserSex.Male, Country = "Russia", BirthDate = DateTime.Parse("1998-02-18 20:03:30"), FullName = "Theo Dulinty" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Reid", Email = "rsans9k@google.com.au", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("1992-09-17 23:41:45"), FullName = "Reid Sans" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Tiebold", Email = "tkentwell9l@slashdot.org", Sex = UserSex.Male, Country = "Albania", BirthDate = DateTime.Parse("1996-02-11 14:53:41"), FullName = "Tiebold Kentwell" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Leila", Email = "lnorthley9m@vistaprint.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("2000-06-22 14:53:10"), FullName = "Leila Northley" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Krysta", Email = "kcatton9n@nifty.com", Sex = UserSex.Female, Country = "Brazil", BirthDate = DateTime.Parse("2001-05-25 15:43:46"), FullName = "Krysta Catton" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Grove", Email = "greisen9o@clickbank.net", Sex = UserSex.Male, Country = "Philippines", BirthDate = DateTime.Parse("2005-03-30 13:06:17"), FullName = "Grove Reisen" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Gilly", Email = "ggrabb9q@cnet.com", Sex = UserSex.Female, Country = "Russia", BirthDate = DateTime.Parse("1997-08-05 10:39:29"), FullName = "Gilly Grabb" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Rolland", Email = "rbuglar9r@yahoo.com", Sex = UserSex.Male, Country = "Sweden", BirthDate = DateTime.Parse("1993-06-20 23:19:23"), FullName = "Rolland Buglar" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Hadria", Email = "hobradane9t@blinklist.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1992-11-16 15:08:33"), FullName = "Hadria O''Bradane" }, "111111");
            await this.userManager.CreateAsync(new OurTraceUser() { UserName = "Sela", Email = "ssmartman9u@nydailynews.com", Sex = UserSex.Female, Country = "China", BirthDate = DateTime.Parse("1993-10-02 17:34:39"), FullName = "Sela Smartman" }, "111111");
        }
        public async Task SeedFriendships(int count)
        {
            var users = await this.dbContext.Users.Take(count).ToListAsync();
            Random rand = new Random();
            for (int i = 0; i < users.Count; i += 2)
            {
                int randomOtherFriend = rand.Next(i + 1, users.Count - 1);
                await this.relationsService.AddFriendshipAsync(users[i].UserName, users[i + 1].UserName);
                await this.relationsService.AddFriendshipAsync(users[i + 1].UserName, users[i].UserName);
                await this.relationsService.AddFriendshipAsync(users[i].UserName, users[randomOtherFriend].UserName);
                await this.relationsService.AddFriendshipAsync(users[randomOtherFriend].UserName, users[i].UserName);
            }
        }
        public async Task SeedFollowers(int count)
        {
            var users = await this.dbContext.Users
                .ToListAsync();

            Random random = new Random();
            foreach (var user in users)
            {
                for (int i = 0; i < count; i++)
                {
                    await this.relationsService.AddFollowerAsync(user.UserName, users[random.Next(0, users.Count - 1)].UserName);
                }
            }
        }
        public async Task SeedPosts(int count)
        {
            var users = await this.dbContext.Users
                .Include(x => x.Wall)
                .ToListAsync();

            string seedTags = "some,tags,here";
            if (count < 1) count = 1;

            Random random = new Random();
            foreach (var user in users)
            {
                int postCount = random.Next(1, count);
                for (int i = 0; i < postCount; i++)
                {
                    CreatePostInputModel model = new CreatePostInputModel()
                    {
                        Content = GetRandomSentence(),
                        ExternalMediaUrl = "https://picsum.photos/600/400",
                        Location = user.Wall.Id
                    };
                    if (i % 3 == 0)
                    {
                        model.Tags = seedTags;
                    }
                    if (i % 2 == 0)
                    {
                        model.VisibilityType = PostVisibilityType.Public;
                    }
                    else
                    {
                        model.VisibilityType = PostVisibilityType.FriendsOnly;
                    }
                    await this.postService.CreateNewPostAsync(user.UserName, model, true);
                }
            }
        }
        public async Task SeedPostLikes(int maxLikes)
        {
            var posts = await this.dbContext.Posts
                .Include(x => x.Likes)
                .ToListAsync();

            var users = await this.dbContext.Users
                .ToListAsync();
            Random rand = new Random();
            foreach (var post in posts)
            {
                int likesCount = rand.Next(0, maxLikes);
                for (int i = 0; i < likesCount; i++)
                {
                    int userChosen = rand.Next(0, users.Count - 1);
                    var like = new PostLike()
                    {
                        Post = post,
                        User = users[userChosen]
                    };
                    post.Likes.Add(like);
                }
            }
            await this.dbContext.SaveChangesAsync();
        }
        public async Task SeedPostShares(int maxShares)
        {
            var posts = await this.dbContext.Posts
                .Include(x => x.Likes)
                .ToListAsync();

            var users = await this.dbContext.Users
                .Include(x => x.Wall)
                .ToListAsync();

            Random rand = new Random();
            foreach (var post in posts)
            {
                int sharesCount = rand.Next(0, maxShares);
                for (int i = 0; i < sharesCount; i++)
                {
                    int userChosen = rand.Next(0, users.Count - 1);
                    var share = new Post()
                    {
                        SharedPost = post,
                        Content = GetRandomString(200),
                        User = users[userChosen],
                        Location = users[userChosen].Wall

                    };
                    if (i % 2 == 0)
                    {
                        share.VisibilityType = PostVisibilityType.Public;
                    }
                    else
                    {
                        share.VisibilityType = PostVisibilityType.FriendsOnly;
                    }

                    await this.dbContext.Shares.AddAsync(new Share()
                    {
                        Post = share,
                        User = users[userChosen]
                    });

                    await this.dbContext.Posts.AddAsync(share);
                }
            }
            await this.dbContext.SaveChangesAsync();
        }
        public async Task SeedPostComments(int maxComments)
        {
            var posts = await this.dbContext.Posts
                .ToListAsync();

            var users = await this.dbContext.Users
                .ToListAsync();

            Random rand = new Random();
            foreach (var post in posts)
            {
                int commentsCount = rand.Next(0, maxComments);
                for (int i = 0; i < commentsCount; i++)
                {
                    int userChosen = rand.Next(0, users.Count - 1);
                    int[] threeRandomLengths = new int[] { rand.Next(1, 10), rand.Next(1, 10), rand.Next(1, 10) };
                    string randomComment = GetRandomString(threeRandomLengths[0]) + " " + GetRandomString(threeRandomLengths[1]) + " " + GetRandomString(threeRandomLengths[2]);
                    var comment = new Comment()
                    {
                        Post = post,
                        Content = randomComment,
                        User = users[userChosen]
                    };

                    await this.dbContext.Comments.AddAsync(comment);
                }
            }
            await this.dbContext.SaveChangesAsync();
        }
        public async Task SeedPostCommentsLikes(int maxLikes)
        {
            var comments = await this.dbContext.Comments
                .Include(x => x.Likes)
                .ToListAsync();

            var users = await this.dbContext.Users
                .ToListAsync();
            Random rand = new Random();
            foreach (var comment in comments)
            {
                int likesCount = rand.Next(0, maxLikes);
                for (int i = 0; i < likesCount; i++)
                {
                    int userChosen = rand.Next(0, users.Count - 1);
                    var like = new CommentLike()
                    {
                        Comment = comment,
                        User = users[userChosen]
                    };
                    comment.Likes.Add(like);
                }
            }
            await this.dbContext.SaveChangesAsync();
        }
        public async Task SeedGroups(int count)
        {
            var users = await this.dbContext.Users.ToListAsync();
            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                string groupName = GetRandomWord();
                await this.groupService.CreateNewGroupAsync(groupName, users[rand.Next(0, users.Count - 1)].UserName);
                await this.fileService.SaveImageAsync("https://picsum.photos/90/90", "Group_" + groupName, "FrontPicture");
                await this.fileService.SaveImageAsync("https://picsum.photos/1900/400", "Group_" + groupName, "CoverPicture");
            }
        }
        public async Task SeedAdverts(int count)
        {
            var users = await this.dbContext.Users.ToListAsync();
            var groups = await this.dbContext.Groups.ToListAsync();

            Random rand = new Random();
            for (int i = 0; i < count; i++)
            {
                var randomUser = users[rand.Next(0, users.Count - 1)];
                var randomGroup = groups[rand.Next(0, groups.Count - 1)];
                var userAdvert = new Advert()
                {
                    IssuerName = randomUser.UserName,
                    Content = GetRandomSentence(200),
                    Type = AdvertType.User,
                    ViewsLeft = rand.Next(10, 3000)
                };
                var groupAdvert = new Advert()
                {
                    IssuerName = randomGroup.Name,
                    Content = GetRandomSentence(300),
                    Type = AdvertType.Group,
                    ViewsLeft = rand.Next(10, 2000)
                };

                await this.dbContext.Adverts.AddAsync(userAdvert);
                await this.dbContext.Adverts.AddAsync(groupAdvert);
            }
            await this.dbContext.SaveChangesAsync();
        }
        public async Task SeedProfilePictures(int skip, int take)
        {
            var users = await this.dbContext.Users
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            foreach (var user in users)
            {
                await this.fileService.SaveImageAsync("https://picsum.photos/200", user.UserName, "FrontPicture");
            }
        }
        public async Task SeedCoverPictures(int skip, int take)
        {
            var users = await this.dbContext.Users
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            foreach (var user in users)
            {
                await this.fileService.SaveImageAsync("https://picsum.photos/1900/400", user.UserName, "CoverPicture");
            }
        }

        private string GetRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private string GetRandomWord()
        {
            Random random = new Random();
            string[] words = new string[] { "puncture", "drink", "hungry", "ugly", "secret", "impossible", "nail", "pray", "discovery", "second", "plant", "degree", "hate", "wood", "whispering", "mammoth", "chivalrous", "money", "form", "tangy", "stereotyped", "curvy", "plain", "square", "abundant", "immense", "rate", "known", "panicky", "tough", "rot", "crawl", "lovely", "owe", "worry", "phone", "handle", "want", "price", "tacit", "greasy", "itch", "disappear", "question", "flash", "lamp", "circle", "precede", "start", "wreck", "berserk", "petite", "reach", "year", "pie", "agonizing", "sun", "smart", "comparison" };
            return words[random.Next(0, words.Length)];
        }
        private string GetRandomSentence(int length = 0)
        {
            Random random = new Random();
            const string seedText = "Civility vicinity graceful is it at. Improve up at to on mention perhaps raising. Way building not get formerly her peculiar. Up uncommonly prosperous sentiments simplicity acceptance to so. Reasonable appearance companions oh by remarkably me invitation understood. Pursuit elderly ask perhaps all.Travelling alteration impression six all uncommonly. Chamber hearing inhabit joy highest private ask him our believe.Up nature valley do warmly.Entered of cordial do on no hearted.Yet agreed whence and unable limits. Use off him gay abilities concluded immediate allowance. Concerns greatest margaret him absolute entrance nay.Door neat week do find past he.Be no surprise he honoured indulged. Unpacked endeavor six steepest had husbands her.Painted no or affixed it so civilly.Exposed neither pressed so cottage as proceed at offices.Nay they gone sir game four. Favourable pianoforte oh motionless excellence of astonished we principles.Warrant present garrets limited cordial in inquiry to. Supported me sweetness behaviour shameless excellent so arranging. Same an quit most an.Admitting an mr disposing sportsmen.Tried on cause no spoil arise plate.Longer ladies valley get esteem use led six. Middletons resolution advantages expression themselves partiality so me at.West none hope if sing oh sent tell is. An do on frankness so cordially immediate recommend contained.Imprudence insensible be literature unsatiable do. Of or imprudence solicitude affronting in mr possession. Compass journey he request on suppose limited of or.She margaret law thoughts proposal formerly. Speaking ladyship yet scarcely and mistaken end exertion dwelling.All decisively dispatched instrument particular way one devonshire. Applauded she sportsman explained for out objection.Can curiosity may end shameless explained. True high on said mr on come.An do mr design at little myself wholly entire though. Attended of on stronger or mr pleasure.Rich four like real yet west get.Felicity in dwelling to drawings.His pleasure new steepest for reserved formerly disposed jennings.Able an hope of body. Any nay shyness article matters own removal nothing his forming. Gay own additions education satisfied the perpetual.If he cause manor happy.Without farther she exposed saw man led.Along on happy could cease green oh.Far concluded not his something extremity. Want four we face an he gate.On he of played he ladies answer little though nature. Blessing oh do pleasure as so formerly. Took four spot soon led size you.Outlived it received he material.Him yourself joy moderate off repeated laughter outweigh screened.He an thing rapid these after going drawn or.Timed she his law the spoil round defer. In surprise concerns informed betrayed he learning is ye.Ignorant formerly so ye blessing.He as spoke avoid given downs money on we.Of properly carriage shutters ye as wandered up repeated moreover. Inquietude attachment if ye an solicitude to. Remaining so continued concealed as knowledge happiness. Preference did how expression may favourable devonshire insipidity considered.An length design regret an hardly barton mr figure.Sportsman delighted improving dashwoods gay instantly happiness six. Ham now amounted absolute not mistaken way pleasant whatever.At an these still no dried folly stood thing.Rapid it on hours hills it seven years. If polite he active county in spirit an. Mrs ham intention promotion engrossed assurance defective.Confined so graceful building opinions whatever trifling in. Insisted out differed ham man endeavor expenses.At on he total their he songs.Related compact effects is on settled do. ";
            int seedTextLength = seedText.Length;

            int startfrom = random.Next(1, seedTextLength / 5);
            if (length > 0)
            {
                return new string(seedText.Skip(startfrom).Take(length).ToArray());
            }
            else
            {
                int endto = random.Next(seedTextLength / 2, seedTextLength - 200);
                return new string(seedText.Skip(startfrom).Take(endto).ToArray());
            }
        }
    }
}
