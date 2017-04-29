namespace KriPod.Primedice
{
    /// <summary>Represents the condition of a <see cref="Bet"/>.</summary>
    public enum BetCondition
    {
        /// <summary>The roll should be lower than the target in order to win.</summary>
        LowerThan = 0,

        /// <summary>The roll should be greater than the target in order to win.</summary>
        GreaterThan = 1
    }
}
