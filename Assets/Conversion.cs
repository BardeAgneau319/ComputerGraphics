using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conversion {

	public int ResX {get; set;}
	public int ResY {get; set;}

	public Conversion(int resX, int resY)
	{
		this.ResX = resX;
		this.ResY = resY;
	}

	public Vector2Int Convert(Vector2 point)
	{
		return new Vector2Int((int) (((point.x+1)/2) * (ResX-1)),  (int) ((1-point.y)/2 * (ResY-1)));
	}

}
