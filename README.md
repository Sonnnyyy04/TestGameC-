```C#
static string GenerateNumber()
    {
        Random rand = new Random();
        string number = "";

        while (number.Length < 4)
        {
            int digit = rand.Next(0, 10);
            if (!number.Contains(digit.ToString()))
            {
                number += digit.ToString();
            }
        }
        return number;
    }
```
