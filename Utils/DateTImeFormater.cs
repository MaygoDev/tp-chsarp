namespace CarDealer.Utils;

public class DateTImeFormater
{
    
    public static DateTime FormatDateTime(string dateTimeStr)
    {
        DateTime dateTime = DateTime.ParseExact(dateTimeStr, "dd/MM/yyyy", null);
        return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
    }
    
}