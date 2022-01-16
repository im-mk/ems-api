namespace EMS.Core
{
    public static class HelperExtensions 
    {
        public static bool IsDefault<T>(this T value)
        {
            if (value == null)
                return false;
            return value.Equals(default(T));
        }

        public static T Update<T>(T value, T newValue)
        {
            return value.IsDefault() ? newValue : value;
        }
    }
}