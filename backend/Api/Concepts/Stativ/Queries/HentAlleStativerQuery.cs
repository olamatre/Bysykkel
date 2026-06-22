using Api.Services;

namespace Api.Concepts.Stativ.Queries;

public record GetStativQuery;

public record StativViewModel(
    string StativId,
    string Navn,
    string Adresse,
    double Latitude,
    double Longitude,
    int Kapasitet
);

public interface IHentAlleStativerQueryHandler
{
    Task<IReadOnlyList<StativViewModel>> HandleAsync(GetStativQuery query, CancellationToken cancellationToken = default);
}

public sealed class HentAlleStativerQueryHandler(IBikeshareClient bikeshareClient) : IHentAlleStativerQueryHandler
{
    public async Task<IReadOnlyList<StativViewModel>> HandleAsync(GetStativQuery query, CancellationToken cancellationToken = default)
    {
        var stationInformation = await bikeshareClient.GetStationInformationAsync(cancellationToken);
        return stationInformation.Data.Stations
            .Select(station => new StativViewModel(
                station.StationId,
                station.Name,
                station.Address,
                station.Lat,
                station.Lon,
                station.Capacity))
            .ToArray();
    }
}
