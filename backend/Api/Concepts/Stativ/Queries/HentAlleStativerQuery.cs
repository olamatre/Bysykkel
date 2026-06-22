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
    int Kapasitet
);

public sealed class HentAlleStativerQueryHandler(IBikeshareClient bikeshareClient)
    : IQueryHandler<GetStativQuery, IReadOnlyList<StativViewModel>>
{

    public async Task<IReadOnlyList<StativViewModel>> Handle(GetStativQuery query, CancellationToken cancellationToken = default)
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
