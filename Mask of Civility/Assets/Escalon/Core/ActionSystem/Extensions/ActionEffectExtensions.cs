
namespace Escalon.ActionSystem
{
    public static class ActionEffectExtensions
    {
        public static ReceivedEffect<T> ToReceivedEffect<T>(this T targetedByEffect) where T : IActionEffect
        {
            return new ReceivedEffect<T>(targetedByEffect);
        }
        
        public static UsedEffect<T> ToUsedEvent<T>(this T usedEffect) where T : IActionEffect
        {
            return new UsedEffect<T>(usedEffect);
        }
    }
}
