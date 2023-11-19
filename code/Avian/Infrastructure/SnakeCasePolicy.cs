using System.Text;
using System.Text.Json;

namespace Avian.Infrastructure;

public sealed class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public override string ConvertName(string name) => ToSnakeCase(name);

    private static string ToSnakeCase(string enumString)
    {
        var result = new StringBuilder();
        for (var i = 0; i < enumString.Length; i++)
        {
            var symbol = enumString[i];
            if (char.IsUpper(symbol))
            {
                if (i > 0 && enumString[i - 1] != '_')
                {
                    result.Append('_');
                }

                result.Append(char.ToLower(symbol));
            }
            else
            {
                result.Append(symbol);
            }
        }

        return result.ToString();
    }
}