using System.Threading.Tasks;

namespace Escalon
{
	/// <summary>
	/// The main way you should interact with that notification system from gameplay code
	/// </summary>
	public static partial class Notification
	{
		public static async void PostNotification(string notificationName, object args)
		{
			await NotificationManager.Instance.PostNotification(notificationName, null, args);
		}
		
		public static async void PostNotification<T>(this T sender, string notificationName)
		{
			await NotificationManager.Instance.PostNotification(notificationName, sender, null);
		}
		
		public static async void PostNotification<T>(this T sender, string notificationName, object args)
		{
			await NotificationManager.Instance.PostNotification(notificationName, sender, args);
		}
		
		public static async Task PostAwaitableNotification<T>(this T sender, string notificationName)
		{
			await NotificationManager.Instance.PostNotification(notificationName, sender, null);
		}
		
		public static async Task PostAwaitableNotification<T>(this T sender, string notificationName, object args)
		{
			await NotificationManager.Instance.PostNotification(notificationName, sender, args);
		}

		public static void AddObserver<T>(NotificationReceiver.Handler handler, string notificationName,  System.Object sender = null) where T : FlowState
		{
			NotificationManager.Instance.AddObserver(typeof(T), handler, notificationName, sender);
		}
		
		public static void AddObserver (this FlowState obj, NotificationReceiver.Handler handler, string notificationName, System.Object sender = null)
		{
			NotificationManager.Instance.AddObserver(obj.GetType(), handler, notificationName, sender);
		}
		
		public static void AddObserver<T>(NotificationReceiver.AwaitableHandler handler, string notificationName,  System.Object sender = null) where T : FlowState
		{
			NotificationManager.Instance.AddObserver(typeof(T), handler, notificationName, sender);
		}
		
		public static void AddObserver (this FlowState obj, NotificationReceiver.AwaitableHandler handler, string notificationName, System.Object sender = null)
		{
			NotificationManager.Instance.AddObserver(obj.GetType(), handler, notificationName, sender);
		}
		
		public static void RemoveObserver<T>(NotificationReceiver.Handler handler, string notificationName,  System.Object sender = null) where T : FlowState
		{
			NotificationManager.Instance.RemoveObserver(typeof(T), handler, notificationName, sender);
		}
		public static void RemoveObserver<T>(NotificationReceiver.AwaitableHandler handler, string notificationName,  System.Object sender = null) where T : FlowState
		{
			NotificationManager.Instance.RemoveObserver(typeof(T), handler, notificationName, sender);
		}
		
		public static void RemoveObserver (this FlowState obj, NotificationReceiver.Handler handler, string notificationName, System.Object sender = null)
		{
			NotificationManager.Instance.RemoveObserver(obj.GetType(), handler, notificationName, sender);
		}
        
		public static void RemoveReceiver<T>() where T : FlowState
		{
			NotificationManager.Instance.RemoveReceiver(typeof(T));
		}
		
		public static void RemoveReceiver(this FlowState obj)
		{
			NotificationManager.Instance.RemoveReceiver(obj.GetType());
		}
		
		public static string ToMessage(System.Type type)
		{
			return $"{type.Name}.Notification";
		}
		
		public static string ToMessage<T>()
		{
			return ToMessage(typeof(T));
		}
	}
}