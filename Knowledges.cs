using Azure;
using BeinConsoleAppStaj.Data;

namespace BeinConsoleAppStaj.Entities;

public class Knowledges
{



    public void CheckKnowledges()
    {
        var topscorers = new List<FootballPlayer>
            {
                new FootballPlayer
                {
                     PlayerId = 1,
                     PlayerName = "Adnan",
                     FirstName = "AdnanEray",
                     LastName = "Kart",
                     Number=null,
                     Position = "GoalKeeper",
                     Nationality = "Turkey",
                    Age=21,
                    BirthDate="15/09/2003",
                    BirthPlace="Turkey",
                    Height="187 cm",
                    Weight="84 kg",

                }
            };
       

    }
}


