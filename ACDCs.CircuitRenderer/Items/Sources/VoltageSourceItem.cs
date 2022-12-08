// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using ACDCs.CircuitRenderer.Drawables;

namespace ACDCs.CircuitRenderer.Items.Sources
{
    public class VoltageSourceItem: SourceItem
    {
        public  VoltageSourceItem() :
            base(SourceDrawableType.Voltage)
        {
        }

        public static new string DefaultValue { get; set; } = "5v";

        public static new bool IsInsertable { get; set; } = true;
    }
}
