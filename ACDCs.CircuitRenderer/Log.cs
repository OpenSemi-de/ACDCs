namespace ACDCs.CircuitRenderer
{
    using System;

    public static class Log
    {
        public static Action<string>? Method;

        public static void L(string text)
        {
            Method?.Invoke(text);
        }
    }
}