namespace EMS.Core.Dto
{
    public class Holiday
    {
        public int HolidayId { get; set; }
        public string RequestedBy { get; set; }
        public string DateRequested { get; set; }
        public string DateFrom { get; set; }
        public string DateFromPart { get; set; }
        public string DateTo { get; set; }
        public string DateToPart { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string StatusBy { get; set; }
        public string StatusDate { get; set; }
    }
}