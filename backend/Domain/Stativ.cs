
namespace Domain;

public class Stativ
{
    public StativId Id { get; set; }
    public string Navn { get; set; }
    public string Adresse { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Kapasitet { get; set; }
    public StativStatus Status { get; set; }

    public Stativ(StativId id, string navn, string adresse, double latitude, double longitude, int kapasitet, StativStatus status)
    {
        Id = id;
        Navn = navn;
        Adresse = adresse;
        Latitude = latitude;
        Longitude = longitude;
        Kapasitet = kapasitet;
        Status = status;
    }
}

public record StativId(string Id);