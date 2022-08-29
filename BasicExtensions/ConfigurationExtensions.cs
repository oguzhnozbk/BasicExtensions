using System.Reflection;

namespace BasicExtensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsRunningUnderIde() => System.Diagnostics.Debugger.IsAttached;
        public static string GetAppPath() => System.IO.Directory.GetCurrentDirectory();
        public static string GetMethodName(this MethodBase currentMethod) => currentMethod.Name;
        public static string GetMethodFullName(this MethodBase currentMethod) => $"{currentMethod.DeclaringType.FullName}.{currentMethod.Name}";

    }
}
