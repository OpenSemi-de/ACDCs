using OSECircuitRender.Drawables;
using OSECircuitRender.Scene;

namespace OSECircuitRender.Interfaces
{
    public interface ISceneManager
    {
        bool SetScene(DrawableComponentList drawables);

        SheetScene Scene { get; set; }

        object GetSceneForBackend();

        bool SendToBackend(object backendScene);
    }
}