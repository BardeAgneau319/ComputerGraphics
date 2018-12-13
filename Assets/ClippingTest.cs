using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClippingTest : MonoBehaviour {

	bool Compare(Vector2[] actual, Vector2[] expected)
	{
		return actual[0]==expected[0] && actual[1] == expected[1];
	}

	string SegmentToString(Vector2[] segment)
	{
		return "(" + segment[0].x + ", " + segment[0].y + ") (" + segment[1].x + ", " + segment[1].y + ")";
	}

	// Use this for initialization
	void Start () {
		// Trivial acceptance
		Vector2[] actualSeg = new Vector2[]{new Vector2(0.5f, 0.5f), new Vector2(-0.5f, -0.5f)};
		Vector2[] expectedSeg = (Vector2[])actualSeg.Clone();
		string output = "TRIVIAL ACCEPTANCE\n";
		output += "Initial segment: " + SegmentToString(actualSeg) + "\n";
		output += "Expected segment: " + SegmentToString(expectedSeg) + "\n";
		if(LineClipping.LineClip(ref actualSeg[0], ref actualSeg[1]) && Compare(actualSeg, expectedSeg))
		{
			output += "Final segment: " + SegmentToString(actualSeg) + "\n";
			output += ("Trivialy aceptance works: the segment hasn't been clipped");
		}
		else
		{
			output += ("Trivialy aceptance doesn't work: the segment has been clipped or hasn't been drawn");
		}
		print(output);

		// Trivial rejection
		actualSeg = new Vector2[]{new Vector2(0.5f, 1.5f), new Vector2(-0.5f, 1.5f)};
		output = "TRIVIAL REJECTION\n";
		output += "Initial segment: " + SegmentToString(actualSeg) + "\n";
		if(!LineClipping.LineClip(ref actualSeg[0], ref actualSeg[1]))
		{
			output += ("Trivialy rejections works: the segment hasn't been drawn");
		}
		else
		{
			output += ("Trivialy rejection doesn't work: the segment has been drawn");
		}
		print(output);

		// Non trivial acceptance and line clipping
		actualSeg = new Vector2[]{new Vector2(0f, 1.1f), new Vector2(1.1f, 0f)};
		expectedSeg = new Vector2[]{new Vector2(0.1f, 1f), new Vector2(1f, 0.1f)};
		output = "NON TRIVIAL ACCEPTANCE & LINE CLIPPING\n";
		output += "Initial segment: " + SegmentToString(actualSeg) + "\n";
		output += "Expected segment: " + SegmentToString(expectedSeg) + "\n";
		if(LineClipping.LineClip(ref actualSeg[0], ref actualSeg[1]))
		{
			output += "Final segment: " + SegmentToString(actualSeg) + "\n";
			if(Compare(actualSeg, expectedSeg))
			{
				output += ("Line clipping works: the segment has been clipped to the expected positions");
			}
			else
			{
				output += ("Line clipping doesn't work: the segment hasn't been clipped to the expected positions");
			}
		}
		else
		{
			output += ("Line clipping doesn't work: the segment has been rejected");
		}
		print(output);

		// Non trivial rejection
		actualSeg = new Vector2[]{new Vector2(1f, 1.5f), new Vector2(1.5f, 0f)};
		output = "NON TRIVIAL REJECTION\n";
		output += "Initial segment: " + SegmentToString(actualSeg) + "\n";
		if(!LineClipping.LineClip(ref actualSeg[0], ref actualSeg[1]))
		{
			output += ("Non trivial rejections works: the segment has been rejected");
		}
		else
		{
			output += ("Non trivial rejection doesn't work: the segment hasn't been rejected");
		}
		print(output);



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
