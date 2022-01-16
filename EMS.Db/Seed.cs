using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EMS.Domain.Db;
using Microsoft.AspNetCore.Identity;

namespace EMS.Db
{
    public static class Seed
    {
        public static async Task SeedData(DataContext context, UserManager<AppUser> userManager)
        {
            AppUser user1 = new AppUser
            {
                DisplayName = "test",
                UserName = "test",
                Email = "test@test.com"
            };

            var user2 = new AppUser
            {
                DisplayName = "bob",
                UserName = "bob",
                Email = "bob@bob.com"
            };

            var user3 = new AppUser
            {
                DisplayName = "kk",
                UserName = "kk",
                Email = "kk@kk.com"
            };

            if(!userManager.Users.Any())
            {
                var users = new List<AppUser>
                {
                    user1,               
                    user2,
                    user3
                };

                foreach (var user in users)
                {
                    var result = await userManager.CreateAsync(user, "Pa$$w0rd");
                }
            }

            if(!context.Values.Any())
            {
                var values = new List<Value>
                {
                    new Value { Name = "100" },
                    new Value { Name = "102" },
                    new Value { Name = "104" }
                };

                context.Values.AddRange(values);
            }

            if(!context.YearHolidays.Any())
            {
                var yearHolidays = new List<YearHoliday>
                {
                    new YearHoliday { Year = 2020, Total = 20 },
                    new YearHoliday { Year = 2021, Total = 20 }
                };
                context.YearHolidays.AddRange(yearHolidays);
            }

            if (!context.Holidays.Any())
            {
                var holidays = new List<Holiday>
                {
                    new Holiday
                    {
                        RequestedBy = user1,
                        DateRequested = new System.DateTime(2019, 1, 1),
                        DateFrom = new System.DateTime(2019, 1, 1),
                        DateFromPart = "am",
                        DateTo = new System.DateTime(2019, 1, 5),
                        DateToPart = "pm",
                        Comments = "Going to ibiza.",
                        Status = "Approved",
                        StatusBy = user2,
                        StatusDate = new System.DateTime(2019, 1, 5)
                    },
                    new Holiday
                    {
                        RequestedBy = user1,
                        DateRequested = new System.DateTime(2019, 2, 1),
                        DateFrom = new System.DateTime(2019, 2, 2),
                        DateFromPart = "am",
                        DateTo = new System.DateTime(2019, 2, 5),
                        DateToPart = "pm",
                        Comments = "Going to egypt.",
                        Status = "Rejected",
                        StatusBy = user2,
                        StatusDate = new System.DateTime(2019, 2, 1)
                    },
                    new Holiday
                    {
                        RequestedBy = user1,
                        DateRequested = new System.DateTime(2019, 1, 1),
                        DateFrom = new System.DateTime(2019, 1, 2),
                        DateFromPart = "am",
                        DateTo = new System.DateTime(2019, 1, 5),
                        DateToPart = "pm",
                        Comments = "Going to america.",
                        Status = "Requested",
                        StatusBy = user2,
                        StatusDate = new System.DateTime(2019, 1, 1)
                    },
                    new Holiday
                    {
                        RequestedBy = user2,
                        DateRequested = new System.DateTime(2019, 1, 1),
                        DateFrom = new System.DateTime(2019, 1, 2),
                        DateFromPart = "am",
                        DateTo = new System.DateTime(2019, 1, 5),
                        DateToPart = "pm",
                        Comments = "Going to america.",
                        Status = "Requested",
                        StatusBy = user1,
                        StatusDate = new System.DateTime(2019, 1, 1)
                    }
                };

                context.AddRange(holidays);
            }

            if (!context.Documents.Any())
            {
                var documents = new List<Document>
                {
                    new Document
                    {
                        DocumentId = new Guid("5fbe888b-4ee0-41e7-b619-af477eb4a609"),
                        Title = "contract1",
                        Path = "/documents/5fbe888b-4ee0-41e7-b619-af477eb4a609.txt",
                        DateUploaded = new System.DateTime(2019, 1, 1)
                    },
                    new Document
                    {
                        DocumentId = new Guid("e25b8fa3-61db-4875-b913-660419e43f42"),
                        Title = "contract2",
                        Path = "/documents/e25b8fa3-61db-4875-b913-660419e43f42.txt",
                        DateUploaded = new System.DateTime(2020, 1, 1)
                    }
                };

                context.AddRange(documents);
            }

            context.SaveChanges();
        }
    }
}