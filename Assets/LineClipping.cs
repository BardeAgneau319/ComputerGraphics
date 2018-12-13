using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineClipping {

    public static bool LineClip(ref Vector2 p1, ref Vector2 p2)
    {
        Outcode outcodeP1 = new Outcode(p1);
        Outcode outcodeP2 = new Outcode(p2);

        if (TrivialyAccept(outcodeP1, outcodeP2)) return true;
        if (TrivialyReject(outcodeP1, outcodeP2)) return false;

        float m = (p2.y - p1.y) / (p2.x - p1.x);

        return LineIntersect(ref p1, outcodeP1, m) && LineIntersect(ref p2, outcodeP2, m);
    }

    private static bool TrivialyAccept(Outcode outcodeP1, Outcode outcodeP2)
    {
        return outcodeP1.IsCentered && outcodeP2.IsCentered;
    }

    private static bool TrivialyReject(Outcode outcodeP1, Outcode outcodeP2)
    {
        return !(outcodeP1 & outcodeP2).IsCentered;
    }

    private static bool LineIntersect(ref Vector2 p, Outcode outcode, float m)
    {
        if (outcode.IsCentered) return true;

        float x, y;
        bool isIn = false;
        Vector2 newP;

        if (outcode.IsUp || outcode.IsDown)
        {
            if (outcode.IsUp) y = Outcode.TOPEDGE;
            else y = Outcode.BOTTOMEDGE;

            x = p.x + ((y - p.y) / m);

            newP = new Vector2(x, y);

            if(new Outcode(newP).IsCentered)
            {
                p = newP;
                isIn = true;
            }
        }

        if(!isIn && (outcode.IsLeft || outcode.IsRight))
                {
            if (outcode.IsLeft) x = Outcode.LEFTEDGE;
            else x = Outcode.RIGHTEDGE;

            y = m * (x - p.x) + p.y;

            newP = new Vector2(x, y);

            if(new Outcode(newP).IsCentered)
            {
                p = newP;
                isIn = true;
            }
        }

        return isIn;
    }
}
