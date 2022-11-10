using OSECircuitRender.Instructions;
using OSECircuitRender.Interfaces;
using System.Linq;

namespace OSECircuitRender.Drawables
{
    public sealed class TerminalDrawable : DrawableComponent
    {
        public TerminalDrawableType TerminalType { get; set; }

        public TerminalDrawable(IWorksheetItem backRef, float x, float y, TerminalDrawableType terminalType = TerminalDrawableType.None, string terminalName = "") : base(typeof(TerminalDrawable))
        {
            TerminalType = terminalType;
            TerminalText = terminalName != "" ? terminalName : TerminalType.ToString();
            if (TerminalText == "Null")
                TerminalText = "";

            switch (terminalType)
            {
                case TerminalDrawableType.None:
                    {
                        break;
                    }
                case TerminalDrawableType.Null:
                case TerminalDrawableType.Gnd:
                    {
                        DrawablePins.Add(new PinDrawable(backRef, 0.5f, 0.5f));

                        DrawInstructions.Add(new LineInstruction(0.5f, 0.5f, 0.5f, 0.9f));
                        DrawInstructions.Add(new LineInstruction(0.2f, 0.9f, 0.8f, 0.9f));
                        DrawInstructions.Add(new LineInstruction(0.3f, 1.1f, 0.7f, 1.1f));
                        DrawInstructions.Add(new LineInstruction(0.4f, 1.3f, 0.6f, 1.3f));
                        DrawInstructions.Add(new TextInstruction(TerminalText, 0, 12, 0.5f, 1.9f));

                        break;
                    }
            }
            Setup(backRef, x, y);
        }

        public string TerminalText { get; set; }

        private void Setup(IWorksheetItem backRef, float x, float y)
        {
            SetSize(1, 1);
            SetPosition(x, y);
            SetRef(backRef);
        }
    }
}