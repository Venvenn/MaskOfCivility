using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Escalon
{
	/// <summary>
	/// Manages the receiving and invoking of messages and their responses
	/// </summary>
	public partial class NotificationReceiver
	{
		public delegate void Handler(object sender, object args); 
		public delegate Task AwaitableHandler(object sender, object args); 
		
		private SenderReceiverTable _table = new SenderReceiverTable();
		private HashSet<List<IHandlerWrapper>> _invoking = new HashSet<List<IHandlerWrapper>>();
		
		public void AddObserver(Type receiverType, IHandlerWrapper handler, string notificationName, Object sender = null)
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

			Object senderKey = sender ?? this;
			
			_table.TryAdd(receiverType, notificationName, senderKey);

			List<IHandlerWrapper> list = _table[receiverType, notificationName, senderKey];
			if (!list.Contains(handler))
			{
				if (_invoking.Contains(list))
				{
					_table[receiverType, notificationName, senderKey] = list = new List<IHandlerWrapper>(list);
				}
				list.Add(handler);
			}
		}

		public void RemoveObserver(Type receiverType, IHandlerWrapper handler, string notificationName, Object sender = null)
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

			
			Object senderKey = sender ?? this;
			
			// No need to take action if we dont monitor this notification
			if (!_table.Contains(receiverType, notificationName, senderKey))
				return;

			List<IHandlerWrapper> list = _table[receiverType, notificationName, senderKey];
			
			int index = list.IndexOf(handler);
			if (index != -1)
			{
				if (_invoking.Contains(list))
				{
					_table[receiverType, notificationName, senderKey] = list = new List<IHandlerWrapper>(list);
				}
	
				list.RemoveAt(index);
			}
		}

		public void RemoveReceiver(Type receiverType)
		{
			_table.RemoveReceiver(receiverType);
		}
		
		public async Task PostNotification(Type receiverType, string notificationName, Object sender, Object args)
		{
			if (string.IsNullOrEmpty(notificationName))
			{
				Debug.LogError("A notification name is required");
				return;
			}
			
			if (_table.Contains(receiverType, notificationName, sender))
			{
				await InvokeHandlers(sender, args, _table[receiverType, notificationName, sender]);
			}
			if (_table.Contains(receiverType, notificationName, this))
			{
				await InvokeHandlers(sender, args, _table[receiverType, notificationName, this]);
			}
		}

		private async Task InvokeHandlers(Object sender, Object args, List<IHandlerWrapper> handlers)
		{
			_invoking.Add(handlers);
			foreach (IHandlerWrapper handler in handlers)
			{
				await handler.Execute(sender, args);
			}
			_invoking.Remove(handlers);
		}
	}
}