namespace FinanceApp.Domain.Common;

public class BaseDomain
{
    public Guid Id { get; set; }
    public DateTime? CreatedDate { get; set; }
    public string? CreatedBy {get;set;}
    public DateTime? LastModifiedDate {get;set;}
    public string? LastModifiedBy { get; set; }
}