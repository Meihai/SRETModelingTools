using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
//符号数学库
using MathNet.Symbolics;
using Expr = MathNet.Symbolics.SymbolicExpression;


namespace ModelingToolsConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            //System.Console.ReadKey();

            var x = Expr.Variable("x");
            var y = Expr.Variable("y");
            var a = Expr.Variable("a");
            var b = Expr.Variable("b");
            var c = Expr.Variable("c");
            var d = Expr.Variable("d");
            // returns string "2*a"
            Console.WriteLine("a+a={0}",(a+a).ToString());
            //returns string "a^2" 
            Console.WriteLine("a*a={0}", (a * a).ToString());
            //returns string "1+1/x"
            Console.WriteLine("2+1/x-1={0}",(2+1/x-1).ToString());
            //returns string "1/(a*b)"
            Console.WriteLine("(a / b / (c * a)) * (c * d / a) / d={0}", ((a / b / (c * a)) * (c * d / a) / d).ToString());

            //三种不同表达式
            // returns string "1/(a*b)"
            Console.WriteLine("String Format:1/(a*b)={0}",(1 / (a * b)).ToString());
            // returns string "a^(-1)*b^(-1)"
            Console.WriteLine("Internal String Format:1/(a*b)={0}", (1 / (a * b)).ToInternalString());
            // returns string "\frac{1}{ab}"
            Console.WriteLine("Latex String Format:1/(a*b)={0}", (1 / (a * b)).ToLaTeX());

            //Expr.Parse("1/(a*b");// throws an exception
            // Returns string "1/(a*b)"
            Console.WriteLine("Expr.Parse(\"1/(a*b)\").ToString()={0}", Expr.Parse("1/(a*b)").ToString());

            var symbols = new Dictionary<string, FloatingPoint> { { "a", 2.0 }, { "b", 3.0 } };
            //Returns Returns 0.166666666666667
            Console.WriteLine("(1 / (a * b)).Evaluate(symbols).RealValue={0}", (1 / (a * b)).Evaluate(symbols).RealValue);
            //Compilation to a function
            Func<double, double, double> f = (1 / (a * b)).Compile("a", "b");
            // returns 0.166666666666667
            Console.WriteLine("f(2.0,3.0)={0}", f(2.0, 3.0));

            // Returns string "3/8 + cos(2*x)/2 + cos(4*x)/8" 转化为没有幂的三角函数
            // 基本原理:cos(x)^4=cos(x)^2*cos(x)^2=0.5*(1+cos(2x))*0.5*(1+cos(2x))
            //=0.25(1+2cos(2x)+cos(2x)^2)=0.25+0.5cos(2x)+0.25(0.5*(1+cos(4x))=0.375+0.5cos(2x)+0.125cos(4x)
            Console.WriteLine("x.Cos().Pow(4).TrigonometricContract().ToString()={0}", x.Cos().Pow(4).TrigonometricContract().ToString());


            // Returns string "1 + x - x^2/2 - x^3/6"
            // 计算sin(x)+cos(x)的4阶泰勒公式
            Console.WriteLine("Taylor(4, x, 0, x.Sin() + x.Cos()).ToString()={0}", Taylor(4, x, 0, x.Sin() + x.Cos()).ToString());

        }

        // Taylor Expansion
        public static Expr Taylor(int k, Expr symbol, Expr al, Expr xl)
        {
            int factorial = 1;
            Expr accumulator = Expr.Zero;
            Expr derivative = xl;
            for (int i = 0; i < k; i++)
            {
                var subs = derivative.Substitute(symbol, al);
                derivative = derivative.Differentiate(symbol);
                accumulator = accumulator + subs / factorial * ((symbol - al).Pow(i));
                factorial *= (i + 1);
            }
            return accumulator.Expand();
        }

    }
}
