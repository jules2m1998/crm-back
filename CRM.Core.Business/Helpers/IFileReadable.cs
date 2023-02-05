
namespace CRM.Core.Business.Helpers;

public enum FIleReadStatus
{
    Valid,
    Invalid,
    Exist

}
public interface IFileReadable
{
    FIleReadStatus Status { get; set; }
    Dictionary<string, List<string>>? Errors { get;set; }
}
