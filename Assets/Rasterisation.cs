using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Rasterisation {

	public static Vector2Int[][][] RasteriseModel(Vector2Int[][] model)
	{
		Vector2Int[][][] drawnModel = new Vector2Int[model.Length][][];
		for(int i=0; i<drawnModel.Length; i++)
		{
			drawnModel[i] = RasterisePolygon(model[i]);
		}
		return drawnModel;
	}
	
	public static Vector2Int[][] RasterisePolygon(Vector2Int[] poly)
	{
		Vector2Int[][] drawnPoly = new Vector2Int[poly.Length][];
		for(int i = 0; i < drawnPoly.Length - 1; i++)
		{
			drawnPoly[i] = RasteriseLine(poly[i], poly[i+1]);
		}
		drawnPoly[drawnPoly.Length - 1] = RasteriseLine(poly[poly.Length - 1], poly[0]);

		return drawnPoly;
	}


	public static Vector2Int[] RasteriseLine(Vector2Int p1, Vector2Int p2)
	{
		Vector2Int begin, end;
		bool negated = false, inverted = false;
		int dx, dy;

		if(p1.x <= p2.x)
		{
			begin = p1;
			end = p2;
		}
		else
		{
			begin = p2;
			end = p1;
		}

		dx = end.x - begin.x;
		dy = end.y - begin.y;

		if(dy < 0)
		{
			NegateY(ref begin);
			NegateY(ref end);
			dy = -dy;
			negated = true;
		}

		if(dy > dx)
		{
			InverteY(ref begin);
			InverteY(ref end);
			int tmp = dx;
			dx = dy;
			dy = tmp;
			inverted = true;
		}

		Vector2Int[] seg = PerformBrenshenhams(begin, end, dx, dy);

		if(negated && inverted)
		{
			for(int i=0; i<seg.Length; i++)
			{
				InverteY(ref seg[i]);
				NegateY(ref seg[i]);
			}
		}
		else if (inverted)
		{
			for(int i=0; i<seg.Length; i++)
			{
				InverteY(ref seg[i]);
			}
		}
		else if(negated)
		{
			for(int i=0; i<seg.Length; i++)
			{
				NegateY(ref seg[i]);
			}
		}

		return seg;
			
	}


    private static Vector2Int[] PerformBrenshenhams(Vector2Int p1, Vector2Int p2, int dx, int dy)
    {
		Vector2Int[] seg = new Vector2Int[dx + 1];
		int pos = 2 * dy;
		int neg = 2 * (dy - dx);
		int p = 2*dy - dx;

		seg[0] = new Vector2Int(p1.x, p1.y);
		for(int i = 1; i < seg.Length; i++)
		{
			if(p<0)
			{
				seg[i] = new Vector2Int(seg[i-1].x + 1, seg[i-1].y);
				p += pos;
			}
			else
			{
				seg[i] = new Vector2Int(seg[i-1].x + 1, seg[i-1].y + 1);
				p += neg;
			}
		}

		return seg;
    }

    private static void NegateY(ref Vector2Int p)
	{
		p.y = -(p.y);
	}

	private static void InverteY(ref Vector2Int p)
	{
		int tmp = p.x;
		p.x = p.y;
		p.y = tmp;
	}
}
