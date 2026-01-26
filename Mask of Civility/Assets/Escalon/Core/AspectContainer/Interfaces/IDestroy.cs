namespace Escalon
{
	public interface IDestroy
	{
		void Disable();
	}

	public static class DestroyExtensions
	{
		public static void Destroy(this IContainer container)
		{
			foreach (IAspect aspect in container.Aspects())
			{
				var item = aspect as IDestroy;
				if (item != null)
					item.Disable();
			}
		}
	}
}