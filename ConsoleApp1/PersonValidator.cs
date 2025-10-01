namespace ConsoleApp1;

public class PersonValidator
{
    public bool Validate(Person person, out string ErrorMessage)
    {

        if (string.IsNullOrEmpty(person.Name) || person.Name.Length < 2)
        {
            ErrorMessage = "blad name";
            return false;
        }
        if (person.Age < 18 || person.Age > 120)
        {
            ErrorMessage = "blad age";
            return false;
        }
        if(string.IsNullOrEmpty(person.Email) || !person.Email.Contains("@"))
        {
            ErrorMessage = "blad email";
            return false;
        }
        ErrorMessage = "";
        return true;

    }
}
