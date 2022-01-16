namespace EMS.Core.Mappers
{
    public interface IHolidayMapper
    {
        Dto.Holiday Map(Domain.Db.Holiday holiday);
    }
}