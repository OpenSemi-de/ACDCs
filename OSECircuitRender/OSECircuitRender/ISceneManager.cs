namespace OSECircuitRender
{
    public interface ISceneManager
    {
        bool SetScene(DrawableComponents drawables);

        SheetScene Scene { get; set; }

        object GetSceneForBackend();

        bool SendToBackend(object backendScene);
    }
}