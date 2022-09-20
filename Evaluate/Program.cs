using System;

public class Program
{
    private static void Main(string[] args)
    {
        Evaluate.evaluate("1 + 2 * 3"); // 7
        Evaluate.evaluate("(1 + 2) * 3"); // 9
        Evaluate.evaluate("1 / 32.5 + 167 * (3498 - 1155) * -721 * (4885 - 1) / 0.5"); // -2755685654567.969
        Evaluate.evaluate("sin(cos(1)) * cos(1)"); // 0.2779289443079115
    }
}