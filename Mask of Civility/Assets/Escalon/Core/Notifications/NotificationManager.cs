using System;
using System.Threading.Tasks;
using Escalon.Utility;
using UnityEngine;
using Object = System.Object;

namespace Escalon
{
	/// <summary>
	/// Manages the routing of messages and observers
	/// </summary>
	public partial class NotificationManager : Singleton<NotificationManager>
	{
		private IDirector _director;
		
		public void SetDirector(IDirector director)
		{
			_director = director;
		}

		public void AddObserver(Type receiverType, NotificationReceiver.Handler handler, string notificationName, Object sender = null)
		{
			if (handler == null)
			{
				Debug.LogError("Can't add a null event handler for notification, " + notificationName);
				return;
			}
			
			if (string.IsNullOrEmpty(notificationName))
			{
				Debug.LogError("Can't observe an unnamed notification");
				return;
			}
			
			StandardHandler standardHandler = new StandardHandler(handler);
			
			_director?.AddObserver(receiverType, standardHandler, notificationName, sender);
		}

		public void AddObserver(Type receiverType, NotificationReceiver.AwaitableHandler handler, string notificationName, Object sender = null)
		{
			if (handler == null)
			{
				Debug.LogError("Can't add a null event handler for notification, " + notificationName);
				return;
			}
			
			if (string.IsNullOrEmpty(notificationName))
			{
				Debug.LogError("Can't observe an unnamed notification");
				return;
			}
			
			AwaitableHandler awaitableHandler = new AwaitableHandler(handler);
			
			_director.AddObserver(receiverType, awaitableHandler, notificationName, sender);
		}

		public void RemoveObserver(Type receiverType, NotificationReceiver.Handler handler, string notificationName, Object sender = null)
		{
			if (handler == null)
			{
				Debug.LogError("Can't remove a null event handler for notification, " + notificationName);
				return;
			}
			
			if (string.IsNullOrEmpty(notificationName))
			{
				Debug.LogError("A notification name is required to stop observation");
				return;
			}
			
			StandardHandler standardHandler = new StandardHandler(handler);
            
			_director?.RemoveObserver(receiverType, standardHandler, notificationName, sender);
		}
		
		public void RemoveObserver(Type receiverType, NotificationReceiver.AwaitableHandler handler, string notificationName, Object sender = null)
		{
			if (handler == null)
			{
				Debug.LogError("Can't remove a null event handler for notification, " + notificationName);
				return;
			}
			
			if (string.IsNullOrEmpty(notificationName))
			{
				Debug.LogError("A notification name is required to stop observation");
				return;
			}

			AwaitableHandler awaitableHandler = new AwaitableHandler(handler);
			
			_director.RemoveObserver(receiverType, awaitableHandler, notificationName, sender);
		}
		
		public void RemoveReceiver(Type receiverType)
		{
			_director.RemoveReceiver(receiverType);
		}
		
		public async Task PostNotification(string notificationName, Object sender, Object args)
		{
			sender ??= _director;
			await _director.PostNotification(notificationName, sender, args);
		}
	}
}