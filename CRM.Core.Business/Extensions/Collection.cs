
using FluentValidation.Results;

namespace CRM.Core.Business.Extensions;

public static class Collection
{
    public static Dictionary<string, string[]> ToCustomDictionnary(this ValidationResult @this)
    {
        var grouped = from error in @this.Errors
                      select new { error.PropertyName, error.ErrorMessage } into err
                      group err by err.PropertyName;

        return grouped
            .ToDictionary(x => x.Key, x => x.Select(x => x.ErrorMessage).ToArray());
    }
}
