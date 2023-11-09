namespace Avian.Domain;

public sealed class DomainException : Exception
{
    public DomainException(string subCode, string message) : base(message)
    {
        SubCode = subCode;
    }

    public string SubCode { get; set; }
}