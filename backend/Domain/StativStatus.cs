namespace Domain;

public class StativStatus
{
    public StativId StativId { get; set; }
    public DateTimeOffset SistOppdatert { get; set; }
    public int AntallSyklerTilgjengelig { get; set; }
    public int AntallLedigePlasser { get; set; }
    public bool ErTilgjengeligForUtlevering { get; set; }
    public bool ErTilgjengeligForParkering { get; set; }

    public StativStatus(StativId stativId, DateTimeOffset sistOppdatert, int antallSyklerTilgjengelig, int antallLedigePlasser, bool erTilgjengeligForUtlevering, bool erTilgjengeligForParkering)
    {
        StativId = stativId;
        SistOppdatert = sistOppdatert;
        AntallSyklerTilgjengelig = antallSyklerTilgjengelig;
        AntallLedigePlasser = antallLedigePlasser;
        ErTilgjengeligForUtlevering = erTilgjengeligForUtlevering;
        ErTilgjengeligForParkering = erTilgjengeligForParkering;
    }
}