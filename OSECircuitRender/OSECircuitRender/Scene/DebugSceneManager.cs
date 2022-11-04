using OSECircuitRender.Drawables;
using OSECircuitRender.Interfaces;

namespace OSECircuitRender.Scene
{
    public sealed class DebugSceneManager : ISceneManager
    {
        public SheetScene Scene { get; set; }
        public object DrawableScene { get; set; }

        public bool SetScene(DrawableComponentList drawables)
        {
            Scene = new SheetScene();
            Scene.SetDrawables(drawables);
            return true;
        }

        public object GetSceneForBackend()
        {
            DrawableScene drawableScene = new(Scene);
            DrawableScene = drawableScene;
            return drawableScene;
        }

        public bool SendToBackend(object backendScene)
        {
            return true;
        }
    }
}