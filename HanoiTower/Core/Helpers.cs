namespace HanoiTower.Core
{
    public static class Helpers
    {
        public static TOutput Project<TInput, TOutput>(this TInput input, Func<TInput, TOutput> func) => func(input);
    }
}
