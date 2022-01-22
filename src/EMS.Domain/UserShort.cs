namespace EMS.Domain
{
    public class UserShort
    {
        public int UserId { get; private set; }
        public string FullName { get; private set; }

        public UserShort(int userId, string fullName)
        {
            UserId = userId;
            FullName = fullName;
        }
    }
}