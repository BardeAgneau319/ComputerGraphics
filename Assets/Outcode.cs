using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Outcode {

    public bool[] Code { get; private set; }
   
    public bool IsUp
    {
        get
        {
            return this.Code[UP];
        }
    }
    public bool IsDown
    {
        get
        {
            return this.Code[DOWN];
        }
    }
    public bool IsLeft
    {
        get
        {
            return this.Code[LEFT];
        }
    }
    public bool IsRight
    {
        get
        {
            return this.Code[RIGHT];
        }
    }
    public bool IsCentered
    {
        get
        {
            return !(this.IsUp || this.IsDown || this.IsLeft || this.IsRight);
        }
    }

    public const int CODESIZE = 4;
    public const int UP = 0;
    public const int DOWN = 1;
    public const int LEFT = 2;
    public const int RIGHT = 3;

    public const int TOPEDGE = -1;
    public const int BOTTOMEDGE = 1;
    public const int LEFTEDGE = -1;
    public const int RIGHTEDGE = 1;


    public static Outcode operator &(Outcode a, Outcode b)
    {
        Outcode res = new Outcode();

        for (int i = 0; i < a.Code.Length; i++)
        {
            res.Code[i] = a.Code[i] && b.Code[i];
        }

        return res;
    }

    public static bool operator ==(Outcode a, Outcode b)
    {
        for (int i = 0; i < a.Code.Length; i++)
        {
            if (a.Code[i] != b.Code[i]) return false;
        }
        return true;
    }

    public static bool operator !=(Outcode a, Outcode b)
    {
        return !(a == b);
    }


    public Outcode(Vector2 p)
    {
        this.Code = new bool[CODESIZE];

        if (p[0] < LEFTEDGE)
        {
            this.Code[LEFT] = true;
            this.Code[RIGHT] = false;
        }
        else if (p[0] > RIGHTEDGE)
        {
            this.Code[LEFT] = false;
            this.Code[RIGHT] = true;
        }
        else
        {
            this.Code[LEFT] = false;
            this.Code[RIGHT] = false;
        }

        if (p[1] < TOPEDGE)
        {
            this.Code[UP] = true;
            this.Code[DOWN] = false;
        }
        else if (p[1] > BOTTOMEDGE)
        {
            this.Code[UP] = false;
            this.Code[DOWN] = true;
        }
        else
        {
            this.Code[UP] = false;
            this.Code[DOWN] = false;
        }
    }

    public Outcode()
    {
        this.Code = new bool[CODESIZE] { false, false, false, false };
    }

    public Outcode(bool[] Code)
    {
        if (Code.Length != CODESIZE) throw new UnityException();
        this.Code = Code;
    }

    public override string ToString()
    {
        string res = "";
        foreach(bool bit in this.Code)
        {
            if (bit) res += "1";
            else res += "0";
        }
        return res;
    }

    public override bool Equals(object obj)
    {
        var outcode = obj as Outcode;
        return outcode != null &&
               base.Equals(obj) &&
               EqualityComparer<bool[]>.Default.Equals(Code, outcode.Code);
    }

    public override int GetHashCode()
    {
        var hashCode = -1634759766;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<bool[]>.Default.GetHashCode(Code);
        return hashCode;
    }
}
