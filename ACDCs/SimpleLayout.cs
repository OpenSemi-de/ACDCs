using MetroLog;

namespace ACDCs;

public static partial class MauiProgram
{
    /// <summary>
    /// A log layout for the debug window.
    /// </summary>
    /// <seealso cref="MetroLog.Layouts.Layout" />
    public class LogLayout : MetroLog.Layouts.Layout
    {
        /// <summary>
        /// Gets the formatted string.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="info">The information.</param>
        /// <returns></returns>
        public override string GetFormattedString(LogWriteContext context, LogEventInfo info)
        {
            return $"{info.TimeStamp:G} - {info.Level}: {info.Message}";
        }
    }
}