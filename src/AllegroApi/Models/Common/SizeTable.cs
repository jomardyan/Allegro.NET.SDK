using System.Text.Json.Serialization;

namespace AllegroApi.Models.Common;

/// <summary>
/// Response containing list of size tables.
/// </summary>
public record PublicTablesDto
{
    /// <summary>
    /// List of size tables.
    /// </summary>
    [JsonPropertyName("tables")]
    public List<PublicTableDto>? Tables { get; init; }
}

/// <summary>
/// Size table information.
/// </summary>
public record PublicTableDto
{
    /// <summary>
    /// Table identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Table name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Template identifier.
    /// </summary>
    [JsonPropertyName("templateId")]
    public string? TemplateId { get; init; }

    /// <summary>
    /// Category identifier.
    /// </summary>
    [JsonPropertyName("categoryId")]
    public string? CategoryId { get; init; }

    /// <summary>
    /// Table rows.
    /// </summary>
    [JsonPropertyName("rows")]
    public List<SizeTableRow>? Rows { get; init; }
}

/// <summary>
/// Size table row.
/// </summary>
public record SizeTableRow
{
    /// <summary>
    /// Row identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Row cells with measurements.
    /// </summary>
    [JsonPropertyName("cells")]
    public List<SizeTableCell>? Cells { get; init; }
}

/// <summary>
/// Size table cell.
/// </summary>
public record SizeTableCell
{
    /// <summary>
    /// Cell identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Cell value.
    /// </summary>
    [JsonPropertyName("value")]
    public string? Value { get; init; }
}

/// <summary>
/// Request to create or update a size table.
/// </summary>
public record SizeTablePutRequest
{
    /// <summary>
    /// Table name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Template identifier.
    /// </summary>
    [JsonPropertyName("templateId")]
    public string? TemplateId { get; init; }

    /// <summary>
    /// Table rows.
    /// </summary>
    [JsonPropertyName("rows")]
    public List<SizeTableRow>? Rows { get; init; }
}

/// <summary>
/// Response containing all size table templates.
/// </summary>
public record SizeTableTemplatesResponse
{
    /// <summary>
    /// List of size table templates.
    /// </summary>
    [JsonPropertyName("templates")]
    public List<SizeTableTemplateResponse> Templates { get; init; } = new();
}

/// <summary>
/// A single size table template.
/// </summary>
public record SizeTableTemplateResponse
{
    /// <summary>
    /// Size table template identifier.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Size table template name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Optional image associated with the template.
    /// </summary>
    [JsonPropertyName("image")]
    public SizeTableTemplateImageResponse? Image { get; init; }

    /// <summary>
    /// Template headers (column names).
    /// </summary>
    [JsonPropertyName("headers")]
    public List<SizeTableHeader> Headers { get; init; } = new();

    /// <summary>
    /// Template cell values.
    /// </summary>
    [JsonPropertyName("values")]
    public List<SizeTableCells> Values { get; init; } = new();
}

/// <summary>
/// Image associated with a size table template.
/// </summary>
public record SizeTableTemplateImageResponse
{
    /// <summary>
    /// URL of the image.
    /// </summary>
    [JsonPropertyName("url")]
    public string Url { get; init; } = string.Empty;

    /// <summary>
    /// Captions for the image.
    /// </summary>
    [JsonPropertyName("captions")]
    public List<SizeTableCaption> Captions { get; init; } = new();
}

/// <summary>
/// A caption entry for a size table template image.
/// </summary>
public record SizeTableCaption
{
    /// <summary>
    /// Caption index.
    /// </summary>
    [JsonPropertyName("index")]
    public string Index { get; init; } = string.Empty;

    /// <summary>
    /// Caption value/text.
    /// </summary>
    [JsonPropertyName("value")]
    public string Value { get; init; } = string.Empty;
}

/// <summary>
/// A size table header (column name).
/// </summary>
public record SizeTableHeader
{
    /// <summary>
    /// Header name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}

/// <summary>
/// A row of cells in a size table.
/// </summary>
public record SizeTableCells
{
    /// <summary>
    /// Cell values for this row.
    /// </summary>
    [JsonPropertyName("cells")]
    public List<string> Cells { get; init; } = new();
}
