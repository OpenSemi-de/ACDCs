// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Sources
{
    public class VoltageSourceItem : SourceItem
    {
        public VoltageSourceItem() :
            base(SourceDrawableType.Voltage)
        {
        }

        public override string DefaultValue => "5v";

        public override bool IsInsertable => true;
    }
}
