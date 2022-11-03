using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;
using System.Linq;
using System;

namespace OSECircuitRender
{
    public sealed class Worksheet
    {
        public int SheetNum { get; set; }

        [JsonIgnore]
        public ISceneManager SceneManager { get; set; }

        public WorksheetItems Items = new();

        public DrawableComponents GetDrawableComponents()
        {
            return new DrawableComponents(Items
            .Select(item =>
            (IDrawableComponent)item.DrawableComponent
            ));
        }

        public bool CalculateScene()
        {
        	Log.L("Calculating scene");
            if (SceneManager == null)
            {
            	Log.L("Creating Manager");
                SceneManager = new DebugSceneManager();
            }

            if (SceneManager.SetScene(GetDrawableComponents()))
            {
            	Log.L("Getting backend scene");
                object backendScene = SceneManager.GetSceneForBackend();
                if (backendScene != null)
                {
                    if (SceneManager.SendToBackend(backendScene))
                    {
                    	Log.L("Backend received scene");
                        return true;
                    }
                }
            }
            return false;
        }

    }
}