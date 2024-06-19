namespace ACDCs.Interfaces;

/// <summary>
/// Interface to the scene manager class.
/// </summary>
public interface ISceneManager
{
    /// <summary>
    /// Draws on the specified canvas.
    /// </summary>
    /// <param name="canvas">The canvas.</param>
    void Draw(ICanvas canvas);

    /// <summary>
    /// Loads the scene from a json string.
    /// </summary>
    /// <param name="jsonScene">The json scene string.</param>
    public void LoadJson(string jsonScene);
}