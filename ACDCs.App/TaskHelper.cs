namespace ACDCs.App;

/// <summary>
/// Helper class for dispatching tasks to the main thread.
/// </summary>
public class TaskHelper
{
    /// <summary>
    /// Runs the specified task.
    /// </summary>
    /// <param name="task">The task.</param>
    public static void Run(Action task)
    {
        if (Application.Current == null ||
            !Application.Current.Dispatcher.IsDispatchRequired)
        {
            task.Invoke();
        }
        else
        {
            Application.Current.Dispatcher.Dispatch(task);
        }
    }
}