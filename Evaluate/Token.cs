
public enum Type
{
    Number,
    MathSign,
    Bracket
}

public class Token
{
    public Type type;
    protected int length = -1;

    protected static string[] mathSignStr =
        new string[] { "+", "-", "*", "/", "sin", "cos" };
    protected static string[] bracketStr =
        new string[] { "(", ")" };
    protected static bool IsDigit(char c)
    {
        if ('0' <= c && c <= '9')
        {
            //아스키코드 값으로 판독
            return true;
        }
        return false;
    }

    public virtual int GetPriority()
    {
        return -1;
    }

    public virtual int GetLength()
    {
        return length;
    }
}