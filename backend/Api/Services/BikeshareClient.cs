using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api.Services;

public sealed class BikeshareClientOptions
{
    [Required]
    [Url]
    public string BaseUrl { get; init; } = "";
    [Required]
    public string ClientIdentifier { get; init; } = "";
}

public interface IBikeshareClient
{
    Task<StationInformationResponse> GetStationInformationAsync(CancellationToken cancellationToken = default);
    Task<StationStatusResponse> GetStationStatusAsync(CancellationToken cancellationToken = default);
}

public sealed class BikeshareClient(HttpClient httpClient) : IBikeshareClient
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Task<StationInformationResponse> GetStationInformationAsync(CancellationToken cancellationToken = default) =>
        GetJsonAsync<StationInformationResponse>("station_information.json", cancellationToken);

    public Task<StationStatusResponse> GetStationStatusAsync(CancellationToken cancellationToken = default) =>
        GetJsonAsync<StationStatusResponse>("station_status.json", cancellationToken);

    private async Task<T> GetJsonAsync<T>(string path, CancellationToken cancellationToken)
    {
        using var response = await httpClient.GetAsync(path, cancellationToken);
        response.EnsureSuccessStatusCode();

        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var payload = await JsonSerializer.DeserializeAsync<T>(stream, JsonOptions, cancellationToken);
        return payload ?? throw new InvalidOperationException($"Empty JSON payload from '{path}'.");
    }
}
public sealed record StationInformationResponse(
    [property: JsonPropertyName("last_updated")] long LastUpdated,
    [property: JsonPropertyName("ttl")] int Ttl,
    [property: JsonPropertyName("data")] StationInformationData Data);

public sealed record StationInformationData(
    [property: JsonPropertyName("stations")] IReadOnlyList<StationInformationStation> Stations);

public sealed record StationInformationStation(
    [property: JsonPropertyName("station_id")] string StationId,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("address")] string Address,
    [property: JsonPropertyName("lat")] double Lat,
    [property: JsonPropertyName("lon")] double Lon,
    [property: JsonPropertyName("capacity")] int Capacity);

public sealed record StationStatusResponse(
    [property: JsonPropertyName("last_updated")] long LastUpdated,
    [property: JsonPropertyName("ttl")] int Ttl,
    [property: JsonPropertyName("data")] StationStatusData Data);

public sealed record StationStatusData(
    [property: JsonPropertyName("stations")] IReadOnlyList<StationStatusStation> Stations);

public sealed record StationStatusStation(
    [property: JsonPropertyName("is_installed")] bool IsInstalled,
    [property: JsonPropertyName("is_renting")] bool IsRenting,
    [property: JsonPropertyName("num_bikes_available")] int NumBikesAvailable,
    [property: JsonPropertyName("num_docks_available")] int NumDocksAvailable,
    [property: JsonPropertyName("last_reported")] long LastReported,
    [property: JsonPropertyName("is_returning")] bool IsReturning,
    [property: JsonPropertyName("station_id")] string StationId);