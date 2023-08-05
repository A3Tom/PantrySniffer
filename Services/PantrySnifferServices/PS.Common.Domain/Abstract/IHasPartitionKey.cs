namespace PS.Common.Domain.Abstract;
public interface IHasPartitionKey
{
    string PartitionKey { get; }
}
