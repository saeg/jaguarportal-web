namespace Microsoft.AspNetCore.Mvc.ModelBinding
{
    /// <summary>
    /// Represents the state of an attempt to bind values from an HTTP Request to an action method, which includes
    /// validation information.
    /// </summary>
    public static class ExtensionsUtil
    {
        public static string GetErrors(this ModelStateDictionary modelState)
        {
            return string.Join("; ", modelState.Values
        .SelectMany(v => v.Errors)
        .Select(e => e.ErrorMessage));
        }
    }
}

namespace Microsoft.AspNetCore.Mvc
{
    public static class ExtensionsUtil
    {
        public static string UsernameAuthenticated(this Controller controller)
        {
            return controller?.User?.Identity?.Name ?? string.Empty;
        }
    }
}