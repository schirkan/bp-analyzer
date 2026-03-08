namespace BPAnalyzer.CodeGen.Stages;

/// <summary>
/// Factory for creating stage generators.
/// Provides centralized access to all stage type generators.
/// </summary>
public static class StageGeneratorFactory
{
    private static readonly Dictionary<string, IStageGenerator> _generators = new()
    {
        { "Action", new ActionStageGenerator() },
        { "Process", new ProcessStageGenerator() },
        { "SubSheet", new SubSheetStageGenerator() },
        { "Alert", new AlertStageGenerator() },
        { "ChoiceStart", new ChoiceStageGenerator() },
        { "Decision", new DecisionStageGenerator() },
        { "Calculation", new CalculationStageGenerator() },
        { "MultipleCalculation", new MultipleCalculationStageGenerator() },
        { "Code", new CodeStageGenerator() },
        { "Note", new NoteStageGenerator() },
        { "Exception", new ExceptionStageGenerator() },
        { "Recover", new RecoverStageGenerator() },
        { "Resume", new ResumeStageGenerator() },
        { "WaitStart", new WaitStageGenerator() },
        { "Navigate", new NavigateStageGenerator() },
        { "Read", new ReadStageGenerator() },
        { "Write", new WriteStageGenerator() },
        { "Start", new StartStageGenerator() },
        { "LoopStart", new LoopStartStageGenerator() },
        { "LoopEnd", new LoopEndStageGenerator() }
    };

    /// <summary>
    /// Gets a stage generator for the specified stage type.
    /// </summary>
    public static IStageGenerator? GetGenerator(string stageType)
    {
        return _generators.TryGetValue(stageType, out var generator) ? generator : null;
    }

    /// <summary>
    /// Gets all registered stage generators.
    /// </summary>
    public static IEnumerable<IStageGenerator> GetAllGenerators()
    {
        return _generators.Values;
    }

    /// <summary>
    /// Checks if a stage type has a generator.
    /// </summary>
    public static bool HasGenerator(string stageType)
    {
        return _generators.ContainsKey(stageType);
    }
}
