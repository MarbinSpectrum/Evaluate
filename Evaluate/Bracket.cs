
public enum EBracket
{
    Front,
    Back,
}

public class Bracket : Token
{
    public EBracket value;
    private Bracket(EBracket pBracket, int pLen)
    {
        type = Type.Bracket;
        value = pBracket;
        length = pLen;
    }
    private Bracket(EBracket pBracket)
    {
        type = Type.Bracket;
        value = pBracket;
        length = -1;
    }
    public static Bracket GetBracket(EBracket pBracket)
    {
        return new Bracket(pBracket);
    }

    public static Bracket GetBracket(string pStr)
    {
        return GetBracket(0, pStr);
    }

    public static Bracket GetBracket(int pIdx, string pStr)
    {
        if (pStr[pIdx] == '(')
            return new Bracket(EBracket.Front, 1);
        else if (pStr[pIdx] == ')')
            return new Bracket(EBracket.Back, 1);

        //괄호가 아니다.
        return null;
    }

    public override int GetPriority()
    {
        switch (value)
        {
            case EBracket.Front:
                return 0;
            case EBracket.Back:
                return 0;
        }
        return -1;
    }

    public override int GetLength()
    {
        if (length == -1)
        {
            switch (value)
            {
                case EBracket.Front:
                case EBracket.Back:
                    return length = 1;
            }
        }
        return length;
    }
}