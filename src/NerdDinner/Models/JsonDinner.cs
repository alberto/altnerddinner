namespace NerdDinner.Models
{
    public class JsonDinner {
        public int      DinnerID    { get; set; }
        public string   Title       { get; set; }
        public double   Latitude    { get; set; }
        public double   Longitude   { get; set; }
        public string   Description { get; set; }
        public int      RSVPCount   { get; set; }
    }
}