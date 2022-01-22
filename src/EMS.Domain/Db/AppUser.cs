using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace EMS.Domain.Db
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public ICollection<Holiday> Holidays { get; set; }
        public ICollection<Holiday> HolidayStatuses { get; set; }
    }
}