using System;

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
public class MathSign : Token
{
    public EMathSign value;
    private MathSign(EMathSign pMathSign, int pLen)
    {
        type = Type.MathSign;
        value = pMathSign;
        length = pLen;
    }
    private MathSign(EMathSign pMathSign)
    {
        type = Type.MathSign;
        value = pMathSign;
        length = -1;
    }

    public static MathSign GetMathSign(EMathSign pMathSign)
    {
        return new MathSign(pMathSign);
    }

    public static MathSign GetMathSign(string pStr)
    {
        return GetMathSign(0, pStr);
    }

    public static MathSign GetMathSign(int pIdx, string pStr)
    {
        //해당부분이 수식인지 확인
        foreach (string ms in mathSignStr)
        {
            //모든 수식를 대조해본다.
            //부분문자열 생성
            string msStr = pStr.Substring(pIdx, Math.Min(pStr.Length - pIdx, ms.Length));

            if (msStr == ms)
            {
                EMathSign mathSign = EMathSign.Empty;
                int len = 0;
                switch (ms)
                {
                    case "+":
                        {
                            if(pIdx + 1 < pStr.Length && IsDigit(pStr[pIdx + 1]))
                            {
                                //수식이 아니다.
                                continue;
                            }
                            mathSign = EMathSign.Plus;
                            len = 1;
                        }
                        break;
                    case "-":
                        {
                            if (pIdx + 1 < pStr.Length && IsDigit(pStr[pIdx + 1]))
                            {
                                //수식이 아니다.
                                continue;
                            }
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
            case EMathSign.Sin:
            case EMathSign.Cos:
                return 3;
        }
        return -1;
    }

    public override int GetLength()
    {
        if (length == -1)
        {
            switch (value)
            {
                case EMathSign.Plus:
                case EMathSign.Subtract:
                case EMathSign.Multiply:
                case EMathSign.Divide:
                    return length = 1;
                case EMathSign.Sin:
                case EMathSign.Cos:
                    return length = 3;
            }
        }
        return length;
    }
}