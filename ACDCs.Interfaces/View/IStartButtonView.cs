namespace ACDCs.Interfaces.View;

/// <summary>
/// Interface for StartButtonView
/// </summary>
public interface IStartButtonView
{
    /// <summary>
    /// Sets the start menu.
    /// </summary>
    /// <param name="startMenu">The start menu.</param>
    /// <returns></returns>
    Task SetStartMenu(IStartMenuView startMenu);
}