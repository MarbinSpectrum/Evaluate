using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Evaluate
{
    private static Token Lexer(int pIdx, string pStr)
    {
        Number number = Number.GetNum(pIdx, pStr);
        if (number != null)
        {
            return number;
        }

        MathSign mathSign = MathSign.GetMathSign(pIdx, pStr);
        if (mathSign != null)
        {
            return mathSign;
        }

        Bracket bracket = Bracket.GetBracket(pIdx, pStr);
        if (bracket != null)
        {
            return bracket;
        }
        return null;
    }

    private static List<Token> Tokenizer(string pStr)
    {
        List<Token> tokens = new List<Token>();
        int idx = 0;
        while (idx < pStr.Length)
        {
            Token token = Lexer(idx, pStr);
            if (token != null)
            {
                //토큰을 넣어준다.
                tokens.Add(token);
                idx += token.GetLength();
                continue;
            }           
            idx++;
        }

        List<Token> resultTokens = new List<Token>();
        Token frontToken = null; //괄호가 아닌 앞에 있는 토큰

        for (int i = 0; i < tokens.Count; i++)
        {
            //특정타입의 토큰이 연속해서 나오는것은 바람직한 모양이아니다.
            //토큰사이에 다른 타입의 토큰을 섞어준다.
            if (tokens[i].type == Type.MathSign)
            {
                //해당 토큰이 수식일때
                if (frontToken != null && frontToken.type == Type.MathSign)
                {
                    //앞의 토큰도 수식이면
                    MathSign msData = (MathSign)tokens[i];
                    MathSign frontMsData = (MathSign)frontToken;

                    if (msData.value == EMathSign.Sin || msData.value == EMathSign.Cos)
                    {
                        //sin cos을 이항연산같은 방법으로 구하고자한다.
                        //0을 넣어준다.
                        resultTokens.Add(Number.GetNum("0"));
                    }
                    else if (frontMsData.value == EMathSign.Multiply || frontMsData.value == EMathSign.Divide)
                    {
                        //1로 나누거나 1로 곱해도 값은 변화지 않는다.
                        //1을 넣어준다.
                        resultTokens.Add(Number.GetNum("1"));
                    }
                    else if (frontMsData.value == EMathSign.Plus || frontMsData.value == EMathSign.Subtract)
                    {
                        //0로 더하거나 0로 빼도 값은 변화지 않는다.
                        //0을 넣어준다.
                        resultTokens.Add(Number.GetNum("0"));
                    }
                }
                else if (frontToken == null)
                {
                    //수식의 양끝에는 숫자가 있어야한다.
                    //임시로 0을 넣어준다.
                    resultTokens.Add(Number.GetNum("0"));
                }
            }
            else if (tokens[i].type == Type.Number)
            {
                //해당 토큰이 숫자일때
                if (frontToken != null && frontToken.type == Type.Number)
                {
                    //앞의 토큰도 숫자이면
                    resultTokens.Add(MathSign.GetMathSign("+"));
                }
            }
            if (tokens[i].type != Type.Bracket)
            {
                //괄호가 아니다. 변경해준다.
                frontToken = tokens[i];
            }
            resultTokens.Add(tokens[i]);
        }

        return resultTokens;
    }

    private static List<Token> Postfix(List<Token> infix)
    {
        //중위식을 후위식으로 변경해준다.
        Stack<Token> stack = new Stack<Token>();
        List<Token> resultTokens = new List<Token>();

        foreach(Token token in infix)
        {
            if (token.type == Type.Number)
            {   
                //숫자값은 그냥 바로 결과리스트에 넣어준다.
                resultTokens.Add(token);
            }
            else if (token.type == Type.MathSign)
            {
                if(stack.Count == 0)
                {
                    //스택이 비어있으면
                    //스택에 넣어준다.
                    stack.Push(token);
                }
                else if (stack.Peek().GetPriority() < token.GetPriority())
                {
                    //스택값의 우선순위보다
                    //현재 토큰의 우선순위가 낮다.
                    //스택에 넣어준다.
                    stack.Push(token);
                }
                else
                {
                    while(stack.Count() > 0 && 
                        stack.Peek().GetPriority() >= token.GetPriority())
                    {
                        //스택값의 우선순위가
                        //현재토큰의 우선순위보다 높다.
                        //현재 스택값을 결과리스트에 넣어준다.
                        resultTokens.Add(stack.Pop());
                    }

                    //이제 스택값보다 현재토큰의 우선순위가 더 높다.
                    //스택에 넣어준다.
                    stack.Push(token);
                }

            }
            else if (token.type == Type.Bracket)
            {
                Bracket bracket = (Bracket)token;
                if(bracket.value == EBracket.Front)
                {
                    //열린괄호이다 스택에 넣어준다.
                    stack.Push(token);
                }
                else if (bracket.value == EBracket.Back)
                {
                    //닫힌괄호가 나오면
                    while(stack.Count > 0)
                    {
                        Token stackTop = stack.Pop();

                        //스택에서 열린괄호가 나올때까지
                        //결과리스트에 토큰을 넣어준다.
                        if (stackTop.type == Type.Bracket)
                        {
                            Bracket stackBracket = (Bracket)stackTop;
                            if (stackBracket.value == EBracket.Front)
                            {
                                //열린괄호가 나왔다 
                                //넣는것을 중단
                                break;
                            }
                        }
                        else
                        {
                            resultTokens.Add(stackTop);
                        }
                    }
                }
            }
        }

        while (stack.Count > 0)
        {
            //스택에 남은 값들을
            //전부 결과리스트에 넣어준다.
            resultTokens.Add(stack.Pop());
        }

        return resultTokens;
    }

    private static Number Calculation(List<Token> postfix)
    {
        //후위식을 이용해 정답을 계산
        Stack<Token> stack = new Stack<Token>();
        foreach (Token token in postfix)
        {
            if (token.type == Type.Number)
            {
                //숫자는 그냥 구해준다.
                stack.Push(token);
            }
            else if (token.type == Type.MathSign)
            {
                //이항연산을 하기위해서
                //스택의 숫자 두개를 가져온다.
                Number num0 = (Number)stack.Pop();
                Number num1 = (Number)stack.Pop();
                MathSign mathSign = (MathSign)token;
                switch (mathSign.value)
                {
                    //연산자에 알맞는값을 스택에 다시 넣어준다.
                    case EMathSign.Plus:
                    {
                        double v = num1.value + num0.value;
                        Number number = Number.GetNum(v.ToString());
                        stack.Push(number);
                    }
                    break;
                    case EMathSign.Subtract:
                    {
                        double v = num1.value - num0.value;
                        Number number = Number.GetNum(v.ToString());
                        stack.Push(number);
                    }
                    break;
                    case EMathSign.Multiply:
                    {
                        double v = num1.value * num0.value;
                        Number number = Number.GetNum(v.ToString());
                        stack.Push(number);
                    }
                    break;
                    case EMathSign.Divide:
                    {
                        double v = num1.value / num0.value;
                        Number number = Number.GetNum(v.ToString());
                        stack.Push(number);
                    }
                    break;
                    case EMathSign.Cos:
                    {
                        double v = Math.Cos(num1.value + num0.value);
                        Number number = Number.GetNum(v.ToString());
                        stack.Push(number);
                    }
                    break;
                    case EMathSign.Sin:
                    {
                        double v = Math.Sin(num1.value + num0.value);
                        Number number = Number.GetNum(v.ToString());
                        stack.Push(number);
                    }
                    break;
                }
            }
        }

        //스택에 마지막으로 남은 값이 결과다.
        return (Number)stack.Pop();
    }

    public static void evaluate(string str)
    {
        //공백이 없는 문자열을 만든다.
        str = str.Replace(" ", string.Empty);

        List<Token> infix = Tokenizer(str);
        List<Token> postfix = Postfix(infix);
        Number number = Calculation(postfix);

        Console.WriteLine(number.value);
    }
}
