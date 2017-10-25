namespace Ockham.Nuget.Commands
{
    /// <summary>
    /// Represents valid values of the FileConflictAction of the install and update commands
    /// </summary>
    public enum FileConflictAction
    { 
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// 
        /// </summary>
        Overwrite = 1,

        /// <summary>
        /// 
        /// </summary>
        Ignore = 2
    }
}
