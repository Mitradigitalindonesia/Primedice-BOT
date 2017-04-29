namespace KriPod.Primedice
{
    static class ExtensionMethods
    {
        public static string ToJsonString(this BetCondition betCondition)
        {
            switch (betCondition) {
                case BetCondition.LowerThan:
                    return "<";

                case BetCondition.GreaterThan:
                    return ">";

                default:
                    return null;
            }
        }
    }
}
