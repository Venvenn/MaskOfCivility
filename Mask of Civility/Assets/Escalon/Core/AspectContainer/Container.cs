using System.Collections.Generic;

namespace Escalon
{
	public interface IContainer
	{
		T AddAspect<T> (string key = null) where T : IAspect, new (); //D
		T AddAspect<T> (T aspect, string key = null) where T : IAspect; //D
		void RemoveAspect (string key); //D
		void RemoveAspect<T> (string key = null);
		void RemoveAspect (IAspect aspect);
		T GetAspect<T> (string key = null) where T : IAspect;
		bool ContainsAspect(string key);
		bool ContainsAspect<T>() where T : IAspect;
		ICollection<IAspect> Aspects (); //D
	}

	public class Container : IContainer, System.ICloneable
	{
		Dictionary<string, IAspect> aspects = new Dictionary<string, IAspect>(); //D
		private List<IUpdateable> _updateableAspects = new List<IUpdateable>();

		public T AddAspect<T>(string key = null) where T : IAspect, new()
		{
			//D
			return AddAspect<T>(new T(), key);
		}

		public T AddAspect<T>(T aspect, string key = null) where T : IAspect
		{
			//D
			key ??= typeof(T).Name;
			aspects.Add(key, aspect);
			aspect.Container = this;

			if (aspect is IUpdateable updateable)
			{
				_updateableAspects.Add(updateable);
			}
			
			return aspect;
		}

		public void RemoveAspect(string key)
		{
			if (aspects[key] is IUpdateable updateable)
			{
				_updateableAspects.Remove(updateable);
			}
			
			aspects.Remove(key);
		}

		public void RemoveAspect<T>(string key = null)
		{
			key ??= typeof(T).Name;
			
			if (aspects[key] is IUpdateable updateable)
			{
				_updateableAspects.Remove(updateable);
			}
			
			aspects.Remove(key);
		}

		public void RemoveAspect(IAspect aspect)
		{
			RemoveAspect(aspect.GetType().Name);
		}

		public T GetAspect<T>(string key = null) where T : IAspect
		{ 
			key ??= typeof(T).Name;
			T aspect = aspects.ContainsKey(key) ? (T)aspects[key] : default(T);
			return aspect;
		}

		public bool ContainsAspect(string key)
		{
			return aspects.ContainsKey(key);
		}
		public bool ContainsAspect<T>()
			where T : IAspect
		{
			return ContainsAspect(typeof(T).Name);
		}
		
		public IAspect GetAspect(string key)
		{
			return aspects.TryGetValue(key, out IAspect aspect) ? aspect : default;
		}

		public ICollection<IAspect> Aspects()
		{
			//D
			return aspects.Values;
		}

		public List<IUpdateable> GetUpdateableAspects()
		{
			return _updateableAspects;
		}

		public virtual object Clone()
		{
			return CloneAsType<Container>();
		}

		public T CloneAsType<T>()
			where T : Container, new()
		{
			var clone = new T();
			foreach (var aspect in this.aspects)
			{
				if (aspect.Value is System.ICloneable)
				{
					clone.AddAspect((IAspect)(aspect.Value as System.ICloneable).Clone(), aspect.Key);
				}
			}

			return clone;
		}
	}
}