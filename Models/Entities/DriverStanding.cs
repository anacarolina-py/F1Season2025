using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Competition.Models.Entities
{
    public class DriverStanding
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; private set; }
        public string DriverId { get; private set; }
        public int TotalPoints { get; private set; }
        public int Wins { get; private set; }

        public DriverStanding(string driverId)
        {
            DriverId = driverId;
            TotalPoints = 0;
            Wins = 0;
        }
        public void AddResult(int points, int position)
        {
            TotalPoints += points;
            if (position == 1)
            {
                Wins++;
            }
        }
    }
}
