namespace CircuitPuzzle
{
    public static class ErrorCodeContainer
    {
        public enum LimiterErrorCodes
        {
            Success = 0,
            ValueLowerThanSet = -1,
            ValueLowerThanSelected = -2
        }
    }
}
