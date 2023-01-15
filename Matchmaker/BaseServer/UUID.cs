namespace Matchmaker.Server.BaseServer;

/// <summary>
/// A UUID Object
/// </summary>
public class UUID
{
    private string value = "";

    /// <summary>
    /// Generates a random string of specified length
    /// </summary>
    /// <param name="length">How long a random string to make</param>
    /// <returns>A random string</returns>
    public string GetRandomString(int length)
    {
        // Creating object of random class
        var rand = new Random();


        var str = "";

        for (var i = 0; i < length; i++)
        {
  
            // Generating a random number.
            var randValue = rand.Next(0, 26);
  
            // Generating random character by converting
            // the random number into character.
            var letter = Convert.ToChar(randValue + 65);
  
            // Appending the letter to string.
            str = str + letter;
        }

        return str;
    }
    
    /// <summary>
    /// Get the UUID value
    /// </summary>
    /// <returns>The UUID's value</returns>
    public string GetValue()
    {
        return value;
    }

    public UUID(int length, List<string> usedIDs)
    {
        var tempValue = "";
        var loop = true;

        while (loop)
        {
            tempValue = GetRandomString(length);
            loop = false;
            foreach (var e in usedIDs)
            {
                if (e.Equals(tempValue))
                {
                    loop = true;
                }
            }
        }

        value = tempValue;
    }
}