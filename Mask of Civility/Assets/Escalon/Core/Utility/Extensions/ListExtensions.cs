using System;
using System.Collections.Generic;
using System.Linq;

namespace Escalon
{
	public static class ListExtensions
	{
		public static T Random<T>(this List<T> list, Random random)
		{
			int index = random.Next(0, list.Count);
			return list[index];
		}

		public static T Random<T>(this T[] list, Random random)
		{
			int index = random.Next(0, list.Length);
			return list[index];
		}

		public static List<T> Shuffle<T>(this List<T> list, Random random)
		{
			return list.OrderBy(x => random.Next(0, 1)).ToList();
		}

		public static T[] Shuffle<T>(this T[] list, Random random)
		{
			return list.OrderBy(x => random.Next(0, 1)).ToArray();
		}

		public static T First<T>(this List<T> list)
		{
			return list[0];
		}

		public static T Last<T>(this List<T> list)
		{
			return list[list.Count - 1];
		}

		public static List<T> ChooseUnity<T>(this List<T> list, int count, Random random)
		{
			var shuffled = list.Shuffle(random);
			return shuffled.GetRange(0, count);
		}

		public static T Draw<T>(this List<T> list, Random random)
		{
			if (list.Count == 0)
				return default(T);

			int index = random.Next(0, list.Count);
			var result = list[index];
			list.RemoveAt(index);
			return result;
		}

		public static List<T> Draw<T>(this List<T> list, int count, Random random)
		{
			int resultCount = Math.Min(count, list.Count);
			List<T> result = new List<T>(resultCount);
			for (int i = 0; i < resultCount; ++i)
			{
				T item = list.Draw(random);
				result.Add(item);
			}

			return result;
		}

		public static string ToStringPretty(this List<string> list)
		{
			var text = string.Empty;
			for (int i = 0; i < list.Count; ++i)
			{
				text += list[i].ToString() + (i < list.Count - 1 ? "," : "");
			}

			return text;
		}
	}
}