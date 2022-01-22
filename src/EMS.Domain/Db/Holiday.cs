using System;

namespace EMS.Domain.Db
{
    public class Holiday
    {
        public int HolidayId { get; set; }
        public string RequestedById { get; set; }
        public virtual AppUser RequestedBy { get; set; }
        public DateTimeOffset DateRequested { get; set; }
        public DateTimeOffset DateFrom { get; set; }
        public string DateFromPart { get; set; }
        public DateTimeOffset DateTo { get; set; }
        public string DateToPart { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string StatusById { get; set; }
        public virtual AppUser StatusBy { get; set; }
        public DateTimeOffset StatusDate { get; set; }
    }
}