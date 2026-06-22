using Api.Services;
using CQRS;

namespace Api.Concepts.Stativ.Queries;

public record GetStativQuery : IQuery<IReadOnlyList<StativViewModel>>;

public record StativViewModel(
    string StativId,
    string Navn,
    string Adresse,
    double Latitude,
    double Longitude,
    int Kapasitet,
    StativStatusViewModel Status
);

public record StativStatusViewModel(
    int AntallLedigePlasser,
    int AntallSykler,
    bool IsRenting,
    bool IsReturning,
    DateTimeOffset SistOppdatert
);

public class HentAlleStativerQueryHandler(IBikeshareClient bikeshareClient)
    : IQueryHandler<GetStativQuery, IReadOnlyList<StativViewModel>>
{
    public async Task<IReadOnlyList<StativViewModel>> Handle(GetStativQuery query, CancellationToken cancellationToken = default)
    {
        var stationInformation = await bikeshareClient.GetStationInformationAsync(cancellationToken);
        var stationStatusInformation = await bikeshareClient.GetStationStatusAsync(cancellationToken);
        return stationInformation.Data.Stations
            .Select(station => new StativViewModel(
                station.StationId,
                station.Name,
                station.Address,
                station.Lat,
                station.Lon,
                station.Capacity,
                new StativStatusViewModel(
                    stationStatusInformation.Data.Stations.FirstOrDefault(s => s.StationId == station.StationId)?.NumBikesAvailable ?? 0,
                    stationStatusInformation.Data.Stations.FirstOrDefault(s => s.StationId == station.StationId)?.NumDocksAvailable ?? 0,
                    stationStatusInformation.Data.Stations.FirstOrDefault(s => s.StationId == station.StationId)?.IsRenting ?? false,
                    stationStatusInformation.Data.Stations.FirstOrDefault(s => s.StationId == station.StationId)?.IsReturning ?? false,
                    DateTimeOffset.FromUnixTimeSeconds(stationStatusInformation.Data.Stations.FirstOrDefault(s => s.StationId == station.StationId)?.LastReported ?? 0)
                )
            ))
            .ToArray();
    }
}
