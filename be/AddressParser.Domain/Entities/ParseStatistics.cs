public class ParseStatistics
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public int TotalParsed { get; set; }
    public int TotalFailed { get; set; }
    public DateTime? LastSuccess { get; set; }
    public DateTime? LastFailure { get; set; }
    public double? AvgParseDurationMs { get; set; }
}
