using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace BeinConsoleAppStaj.Data;

public class FootballPlayer
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "PlayerId must be an integer.")]
    public int PlayerId { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "PlayerName must be between 1 and 100 characters.")]
    public string PlayerName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "FirstName must be between 1 and 50 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "LastName must be between 1 and 50 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Range(1, 99, ErrorMessage = "Number must be between 1 and 99.")]
    public int? Number { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Position must be between 1 and 50 characters.")]
    public string Position { get; set; } = string.Empty;

    [Required]
    [Range(16, 50, ErrorMessage = "Age must be between 16 and 50.")]
    public int Age { get; set; }

    [Required]
    [RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\d{4}$", ErrorMessage = "BirthDate must be in the format dd/MM/yyyy.")]
    public string BirthDate { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "BirthPlace can't be longer than 100 characters.")]
    public string BirthPlace { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "BirthCountry can't be longer than 100 characters.")]
    public string BirthCountry { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Nationality can't be longer than 50 characters.")]
    public string Nationality { get; set; } = string.Empty;

    [RegularExpression(@"^[0-9]{1,3}\s?cm$", ErrorMessage = "Height must be a numeric value followed by 'cm' (e.g., 184 cm).")]
    public string Height { get; set; } = string.Empty;

    [RegularExpression(@"^[0-9]{1,3}\s?kg$", ErrorMessage = "Weight must be a numeric value followed by 'kg' (e.g., 75 kg).")]
    public string ? Weight { get; set; } = string.Empty;


}





 
public static class FootballPlayerExtensions
{
    public static FootballPlayer ToFootballPlayer(this JsonElement jsonElement)
    {




        return new FootballPlayer
        {

            PlayerName = jsonElement.GetProperty("player_name").GetString(),
            FirstName = jsonElement.GetProperty("firstname").GetString(),
            LastName = jsonElement.GetProperty("lastname").GetString(),
            Number = jsonElement.TryGetProperty("number", out var NumberElement) && NumberElement.ValueKind != JsonValueKind.Null ? NumberElement.GetInt32() : (int?)null,
            Position = jsonElement.GetProperty("position").GetString(),
            Age = jsonElement.GetProperty("age").GetInt32(),
            BirthDate = jsonElement.GetProperty("birth_date").GetString(),
            BirthPlace = jsonElement.GetProperty("birth_place").GetString(),
            BirthCountry = jsonElement.GetProperty("birth_country").GetString(),
            Nationality = jsonElement.GetProperty("nationality").GetString(),
            Height = jsonElement.GetProperty("height").GetString(),
            Weight = jsonElement.TryGetProperty("weight", out var WeightElement) && WeightElement.ValueKind != JsonValueKind.Null ? WeightElement.GetString() : null,

        };
    }

    public static List<FootballPlayer> ToFootballPlayers(this JsonElement jsonElement)
    {
        var players = new List<FootballPlayer>();


        if (jsonElement.TryGetProperty("api", out JsonElement apiElement))
        {

            if (apiElement.TryGetProperty("players", out JsonElement playersElement))
            {
                foreach (var element in playersElement.EnumerateArray())
                {
                    players.Add(element.ToFootballPlayer());
                }
            }
            else
            {
                throw new KeyNotFoundException("The given key 'players' was not present in the JSON.");
            }
        }
        else
        {
            throw new KeyNotFoundException("The given key 'api' was not present in the JSON.");
        }

        return players;
    }
}