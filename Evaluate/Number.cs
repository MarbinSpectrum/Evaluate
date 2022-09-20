using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Number : Token
{
    public double value;
    private Number(double pNum, int pLen)
    {
        type = Type.Number;
        value = pNum;
        length = pLen;
    }
    private Number(double pNum)
    {
        type = Type.Number;
        value = pNum;
        length = -1;
    }

    public static Number GetNum(double pNum)
    {
        return new Number(pNum);
    }

    public static Number GetNum(string pStr)
    {
        return GetNum(0, pStr);
    }
    public static Number GetNum(int pIdx, string pStr)
    {
        int len = 0;
        if (pStr[pIdx] == '-' || pStr[pIdx] == '+')
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
        if (double.TryParse(numStr, out num))
        {
            return new Number(num, len);
        }
        return null;
    }

    public override int GetLength()
    {
        if(length == -1)
        {
            return length = value.ToString().Length;
        }
        return length;
    }
}