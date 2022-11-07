using OSECircuitRender.Definitions;
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
            Scene.ShowGrid = true;
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

        public void SetSizeAndScale(Coordinate sheetSize, float gridSize)
        {
            Scene.GridSize = gridSize;
            Scene.SheetSize = sheetSize;
        }
    }
}