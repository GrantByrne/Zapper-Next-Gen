namespace Zapper.Client.IRCodes;

public class ImportExternalCodeSetResponse
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int? CodeSetId { get; set; }
}