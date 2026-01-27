using UnityEngine;
using System.Collections;
using Unity.Mathematics;

public static class FalloffGenerator {

	public static float[,] GenerateFalloffMap(int2 size) {
		float[,] map = new float[size.x,size.y];

		for (int i = 0; i < size.x; i++) {
			for (int j = 0; j < size.y; j++) {
				float x = i / (float)size.x * 2 - 1;
				float y = j / (float)size.y * 2 - 1;

				float value = Mathf.Max (Mathf.Abs (x), Mathf.Abs (y));
				map [i, j] = Evaluate(value);
			}
		}

		return map;
	}

	static float Evaluate(float value) {
		float a = 3;
		float b = 2.2f;

		return Mathf.Pow (value, a) / (Mathf.Pow (value, a) + Mathf.Pow (b - b * value, a));
	}
}
