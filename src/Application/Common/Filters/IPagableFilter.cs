namespace Alerting.Application.Common.Filters
{
    public interface IPagableFilter : IFilter
    {
        int? Skip { get; set; }
        int? Limit { get; set; }
        string Sort { get; set; }
    }
}
