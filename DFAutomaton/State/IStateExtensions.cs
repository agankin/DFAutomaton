namespace DFAutomaton
{
    public static class IStateExtensions
    {
        public static string Format<TTransition, TState>(this IState<TTransition, TState> state)
            where TTransition : notnull
        {
            var id = state.Id;
            var tag = state.Tag?.ToString();
            
            return string.IsNullOrEmpty(tag)
                ? $"State {id}"
                : $"State {id}: {tag}";
        }
    }
}