using Newtonsoft.Json;
using Microsoft.Maui.Graphics;

namespace OSECircuitRender
{
    public sealed class DebugSceneManager : ISceneManager
    {
        public SheetScene Scene { get; set; }
        public object DrawableScene { get; set; }

        public bool SetScene(DrawableComponents drawables)
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
            //PictureCanvas canvas = new(0, 0, 200, 200);
            //((DrawableScene)backendScene).Draw(canvas, RectF.Zero);
            //Console.WriteLine(JsonConvert.SerializeObject(canvas));
            return true;
        }
    }
}