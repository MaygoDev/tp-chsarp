namespace CarDealer.Utils;

public class PhoneNumberParser
{
    
    public static string Parse(string rawPhoneNumber)
    {
        string numbersOnly = rawPhoneNumber
            .Replace("+33", "") // Remove france country code
            .Replace("(", "").Replace(")", ""); // Remove parentheses
        
        numbersOnly = new string(numbersOnly.Where(char.IsDigit).ToArray());
        
        if (numbersOnly.Length == 9 && !numbersOnly.StartsWith("0"))
        {
            numbersOnly = "0" + numbersOnly;
        }
        
        return numbersOnly;
    }
    
}