using System;
using System.Collections.Generic;
using System.Linq;
using Cometary;

/// <summary>
///   Indicates that the marked assembly will have its indexing operations
///   modified to start at the given index.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly)]
public class IndexesStartAtAttribute : CometaryAttribute
{
    private readonly int startAt;

    public IndexesStartAtAttribute(int at)
    {
        startAt = at;
    }

    /// <inheritdoc />
    public override IEnumerable<CompilationEditor> Initialize()
    {
        return startAt == 0
            ? Enumerable.Empty<CompilationEditor>()
            : Enumerable.Repeat<CompilationEditor>(new IndexReplacer(startAt), 1);
    }
}

/// <summary>
///   Indicates that the marked assembly will have its indexing operations
///   to start at one.
/// </summary>
public sealed class IndexesStartAtOneAttribute : IndexesStartAtAttribute
{
    public IndexesStartAtOneAttribute() : base(1)
    {
    }
}
