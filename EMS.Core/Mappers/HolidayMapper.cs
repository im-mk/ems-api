namespace EMS.Core.Mappers
{
    public class HolidayMapper : IHolidayMapper
    {
        public Dto.Holiday Map(Domain.Db.Holiday holiday)
        {
            return new EMS.Core.Dto.Holiday
            {
                HolidayId = holiday.HolidayId,
                RequestedBy = holiday.RequestedBy.DisplayName,
                DateRequested = holiday.DateRequested.ToShortDateString(),
                DateFrom = holiday.DateFrom.ToShortDateString(),
                DateFromPart = holiday.DateFromPart,
                DateTo = holiday.DateTo.ToShortDateString(),
                DateToPart = holiday.DateToPart,
                Comments = holiday.Comments,
                Status = holiday.Status,
                StatusBy = holiday.StatusBy.DisplayName,
                StatusDate = holiday.StatusDate.ToShortDateString()
            };
        }
    }
}