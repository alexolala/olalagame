using UnityEngine;
using System.Collections;

public class Model : MonoBehaviour {



	public int width = 4;
	public int height = 8;
	public Cube[] list = null;
	public Cube getCube (int x, int y)
	{

		if (x < 0 || x >= width) {
			return null;		
		}
		if (y < 0 || y >= height) {
			return null;		
		}
		int n = x + y * 4;
		return list[n];
	}

	public int points = Points.number;

}
