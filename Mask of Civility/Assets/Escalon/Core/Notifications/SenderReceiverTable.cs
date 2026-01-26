using System;
using System.Collections.Generic;

using SenderTable = System.Collections.Generic.Dictionary<System.Object, System.Collections.Generic.List<Escalon.IHandlerWrapper>>;

namespace Escalon
{
	/// <summary>
	/// Table for mapping handles to their notification and scope 
	/// </summary>
	public class SenderReceiverTable
	{
		private readonly Dictionary<Type, Dictionary<string, SenderTable>> _table = new Dictionary<Type, Dictionary<string, SenderTable>>();

		public List<IHandlerWrapper> this[Type type, string key, object sender]
		{
			get => _table[type][key][sender];
			set => _table[type][key][sender] = value;
		}
		
		public void TryAdd(Type type, string key, object sender)
		{
			_table.TryAdd(type, new Dictionary<string, SenderTable>());
			_table[type].TryAdd(key, new SenderTable());
			_table[type][key].TryAdd(sender, new List<IHandlerWrapper>());
		}

		public void RemoveReceiver(Type type)
		{
			if (_table.ContainsKey(type))
			{
				_table.Remove(type);
			}
		}
		
		public bool Contains(Type type, string key)
		{
			if (!_table.ContainsKey(type))
			{
				return false;
			}
			if (!_table[type].ContainsKey(key))
			{
				return false;
			}
			
			return true;
		}
		
		public bool Contains(Type type, string key, object sender)
		{
			if (!_table.ContainsKey(type))
			{
				return false;
			}
			if (!_table[type].ContainsKey(key))
			{
				return false;
			}
			if (!_table[type][key].ContainsKey(sender))
			{
				return false;
			}

			return true;
		}
	}
}