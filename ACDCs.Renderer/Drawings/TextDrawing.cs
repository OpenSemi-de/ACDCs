﻿using ACDCs.Interfaces;

namespace ACDCs.Renderer.Drawings;

/// <summary>
/// Drawing element for text.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="TextDrawing" /> class.
/// </remarks>
/// <seealso cref="ACDCs.Interfaces.IDrawing" />
/// <param name="id">The identifier.</param>
/// <param name="text">The text.</param>
/// <param name="x">The x.</param>
/// <param name="y">The y.</param>
/// <param name="width">The width.</param>
/// <param name="height">The height.</param>
/// <param name="rotation">The rotation.</param>
public class TextDrawing(string id, string text, float x, float y, float width, float height, float rotation) : IDrawing
{
    /// <summary>
    /// Gets or sets the height.
    /// </summary>
    /// <value>
    /// The height.
    /// </value>
    public float Height { get; set; } = height;

    /// <summary>
    /// Gets or sets the identifier.
    /// </summary>
    /// <value>
    /// The identifier.
    /// </value>
    public string Id { get; set; } = id;

    /// <summary>
    /// Gets or sets the rotation.
    /// </summary>
    /// <value>
    /// The rotation.
    /// </value>
    public float Rotation { get; set; } = rotation;

    /// <summary>
    /// Gets or sets the text.
    /// </summary>
    /// <value>
    /// The text.
    /// </value>
    public string Text { get; set; } = text;

    /// <summary>
    /// Gets or sets the value.
    /// </summary>
    /// <value>
    /// The value.
    /// </value>
    public float Value { get; set; } = 0;

    /// <summary>
    /// Gets or sets the width.
    /// </summary>
    /// <value>
    /// The width.
    /// </value>
    public float Width { get; set; } = width;

    /// <summary>
    /// Gets the x.
    /// </summary>
    /// <value>
    /// The x.
    /// </value>
    public float X { get; set; } = x;

    /// <summary>
    /// Gets the y.
    /// </summary>
    /// <value>
    /// The y.
    /// </value>
    public float Y { get; set; } = y;
}