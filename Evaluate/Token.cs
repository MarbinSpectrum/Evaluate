using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum Type
{
    Number,
    MathSign,
    Bracket
}

public enum EMathSign
{
    Empty,
    Plus,
    Subtract,
    Multiply,
    Divide,
    Sin,
    Cos
}

public enum EBracket
{
    Front,
    Back,
}

public class Token
{
    public Type type;
    public int Length;

    protected static string[] mathSignStr =
        new string[] { "+", "-", "*", "/", "sin", "cos" };
    protected static string[] bracketStr =
        new string[] { "(", ")" };
    protected static bool IsDigit(char c)
    {
        if ('0' <= c && c <= '9')
            return true;
        return false;
    }
    protected static double Pow(double pBase, int n)
    {
        if (n == 0)
            return 1;

        double a = Pow(pBase, n / 2);
        if (n % 2 == 0)
            return a * a;
        else
        {
            if (n < 0)
                return a * a * (1 / pBase);
            else
                return a * a * pBase;
        }
    }

    public virtual int GetPriority()
    {
        return -1;
    }
}

public class Number : Token
{
    public double value;
    private Number(double pNum, int pLen)
    {
        type = Type.Number;
        value = pNum;
        Length = pLen;
    }

    public static Number GetNum(string pStr)
    {
        return GetNum(0, pStr);
    }
    public static Number GetNum(int pIdx, string pStr)
    {    
        int len = 0;
        if(pStr[pIdx] == '-' || pStr[pIdx] == '+')
        {
            //숫자 앞에 부호가 있는 경우일수도 있다.
            len++;
        }
        else if (IsDigit(pStr[pIdx]) == false)
        {
            //숫자가 아니다
            return null;
        }

        int startPos = len + pIdx;
        for (int i = startPos; i < pStr.Length; i++)
        {
            if (IsDigit(pStr[i]) || pStr[i] == '.')
            {
                //숫자나 소수점이다 수의 일부다.  
                len++;
            }
            else
            {
                //숫자도 아니고 소수점도 아니다.
                break;
            }
        }

        string numStr = pStr.Substring(pIdx, len);
        double num = 0;
        if(double.TryParse(numStr,out num))
        {
            return new Number(num, len);
        }
        return null;
    }
}

public class MathSign : Token
{
    public EMathSign value;
    private MathSign(EMathSign pMathSign, int pLen)
    {
        type = Type.MathSign;
        value = pMathSign;
        Length = pLen;
    }

    public static MathSign GetMathSign(string pStr)
    {
        return GetMathSign(0, pStr);
    }

    public static MathSign GetMathSign(int pIdx, string pStr)
    {
        //해당부분이 부호인지 확인
        foreach (string ms in mathSignStr)
        {
            //모든 부호를 대조해본다.
            //부분문자열 생성
            string msStr = pStr.Substring(pIdx, Math.Min(pStr.Length - pIdx, ms.Length));

            if (msStr == ms)
            {
                //부호이다.
                EMathSign mathSign = EMathSign.Empty;
                int len = 0;
                switch(ms)
                {
                    case "+":
                    {
                        mathSign = EMathSign.Plus;
                        len = 1;
                    }
                    break;
                    case "-":
                    {
                        mathSign = EMathSign.Subtract;
                        len = 1;
                    }
                    break;
                    case "*":
                    {
                        mathSign = EMathSign.Multiply;
                        len = 1;
                    }
                    break;
                    case "/":
                    {
                        mathSign = EMathSign.Divide;
                        len = 1;
                    }
                    break;
                    case "sin":
                    {
                        mathSign = EMathSign.Sin;
                        len = 3;
                    }
                    break;
                    case "cos":
                    {
                        mathSign = EMathSign.Cos;
                        len = 3;
                    }
                    break;
                }

                //부호객체생성
                return new MathSign(mathSign, len);
            }
        }
        return null;
    }

    public override int GetPriority()
    {
        switch (value)
        {
            case EMathSign.Plus:
            case EMathSign.Subtract:
                return 1;
            case EMathSign.Multiply:
            case EMathSign.Divide:
                return 2;
                break;
            case EMathSign.Sin:
            case EMathSign.Cos:
                return 3;
        }
        return -1;
    }
}

public class Bracket : Token
{
    public EBracket value;
    private Bracket(EBracket pBracket, int pLen)
    {
        type = Type.Bracket;
        value = pBracket;
        Length = pLen;
    }
    public static Bracket GetBracket(string pStr)
    {
        return GetBracket(0, pStr);
    }
    public static Bracket GetBracket(int pIdx,string pStr)
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
}