namespace DFAutomaton.Tests
{
    public static class ShoppingStateTree
    {
        public static State<ShoppingActions, ShoppingStates> Create()
        {
            var start = StateFactory<ShoppingActions, ShoppingStates>.Start();

            start
                .ToState(ShoppingActions.Order, ShoppingStates.Ordered)
                .ToState(ShoppingActions.Pay, ShoppingStates.Shipped)
                .ToState(ShoppingActions.Receive, ShoppingStates.Delivered);

            return start;
        }
    }

    public enum ShoppingActions
    {
        Order = 1,

        Pay,

        Receive
    }

    public enum ShoppingStates
    {
        Ordered = 1,

        Shipped,

        Delivered
    }
}