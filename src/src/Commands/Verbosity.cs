namespace Ockham.Nuget.Commands
{
    /// <summary>
    /// Maps to Verbosity options available in nuget.exe
    /// </summary>
    public enum Verbosity
    {
        /// <summary>
        /// -Verbosity normal
        /// </summary>
        Normal = 1,
         
        /// <summary>
        /// -Verbosity quiet
        /// </summary>
        Quiet = 2,

        /// <summary>
        /// -Verbosity detailed
        /// </summary>
        Detailed = 3
    }
}
