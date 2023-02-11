namespace ACDCs.CircuitRenderer.Drawables
{
    using System.Collections.Generic;
    using Instructions;

    public class TraceDrawablePart
    {
        public PinDrawable FromPin { get; set; }

        public List<LineInstruction> Instructions { get; set; } = new();

        public PinDrawable ToPin { get; set; }

        public TraceDrawablePart(PinDrawable fromPin, PinDrawable toPin)
        {
            FromPin = fromPin;
            ToPin = toPin;
        }
    }
}