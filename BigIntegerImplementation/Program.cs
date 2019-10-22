using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BigIntegerImplementation
{
    class Program
    {

        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("Choose an action to perform: " +
                    "\n1 - process basic operations" +
                    "\n2 - process operations by module" +
                    "\n3 - solve system of comparsions" +
                    "\n-----type \"stop\" when finished-----");

                int p;
                string operation = Console.ReadLine();
                if(operation.Equals("stop"))
                {
                    return;
                }

                Int32.TryParse(operation, out p);

                if (p == 1)
                {
                    basicOperations();
                }
                else if (p == 2)
                {
                    moduleOperaions();
                }
                else if (p == 3)
                {
                    comparsionSystem();
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }
        }

        private static void comparsionSystem()
        {
            Regex regex = new Regex(@"^-?[0-9]+$");
            Console.WriteLine("Enter comparsion system as the following: \nA B (x = A(mod B))");
            Console.WriteLine("Print \"done\" when finished");
            string s = "";
            List<BigNumber> b = new List<BigNumber>();
            List<BigNumber> m = new List<BigNumber>();
            while(!s.Equals("done"))
            {
                s = Console.ReadLine();
                if(s.Split(' ').Length != 2)
                {
                    if(s.Equals("done"))
                    {
                        break;
                    }
                    Console.WriteLine("Invalid syntax");
                    continue;
                }
                if(regex.IsMatch(s.Split(' ')[0]) && regex.IsMatch(s.Split(' ')[1]))
                {
                    b.Add(new BigNumber(s.Split(' ')[0]));
                    m.Add(new BigNumber(s.Split(' ')[1]));
                }
                else
                {
                    Console.WriteLine("Invalid syntax!");
                }
            }

            List<BigNumber> listM = new List<BigNumber>();
            BigNumber M = new BigNumber("1");
            foreach(BigNumber element in m)
            {
                M = M * element;
            }

            foreach (BigNumber element in m)
            {
                listM.Add(M / element);
            }

            List<BigNumber> listN = new List<BigNumber>();

            for(int i = 0; i<listM.Count; ++i)
            {
                try
                {
                    listN.Add(extendedEuclid(listM[i], m[i]));
                }
                catch(DivideByZeroException ex)
                {
                    Console.WriteLine("Multiplicative inverse does not exist.");
                    return;
                }
            }

            BigNumber x = new BigNumber("0");
            for(int i = 0; i<m.Count; ++i)
            {
                x = x + b[i] * listN[i] * listM[i];
            }

            Console.WriteLine($"x = {x.GetNumber()}") ;
        }

        private static void moduleOperaions()
        {
            Regex regex = new Regex(@"^-?[0-9]+$");
            Console.WriteLine("Enter a module: ");
            string module = Console.ReadLine();

            if (!regex.IsMatch(module))
            {
                Console.WriteLine("NaN");
                return;
            }

            Console.WriteLine("Actions available: + - / * ^\nEnter an expression: ");
            string s = Console.ReadLine();

            if (s.Split(' ').Length == 3)
            {
                if (!regex.IsMatch(s.Split(' ')[0]) || !regex.IsMatch(s.Split(' ')[2]))
                {
                    Console.WriteLine("NaN");
                    return;
                }

                BigNumber a = new BigNumber(s.Split(' ')[0]);
                BigNumber b = new BigNumber(s.Split(' ')[2]);
                BigNumber m = new BigNumber(module);

                switch (s.Split(' ')[1])
                {
                    case "+":
                        BigNumber temp = a + b;
                        if (a + b >= m)
                            temp = temp - m;
                        Console.WriteLine(temp.GetNumber(), "(mod", m, ")");
                        break;
                    case "-":
                        temp = a + b;
                        if (a - b < new BigNumber("0"))
                            temp = temp + m;
                        Console.WriteLine(temp.GetNumber(), "(mod", m, ")");
                        break;
                    case "*":
                        BigNumber temp_mult = (a * b) / m;
                        Console.WriteLine(((a * b) - temp_mult*m).GetNumber(), "(mod",m,")");
                        break;
                    case "/":
                        BigNumber h;
                        try
                        {
                            h = extendedEuclid(b, m);
                        }
                        catch (DivideByZeroException ex)
                        {
                            return;
                        }
                        //Console.WriteLine(h);
                        temp_mult = (a * h) / m;
                        Console.WriteLine(((a * h) - temp_mult * m).GetNumber(), "(mod", m, ")");
                        break;
                    case "^":
                        Console.WriteLine(BigNumber.ModExp(a, b, m));
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid syntax!");
                return;
            }
        }

        private static BigNumber extendedEuclid(BigNumber b, BigNumber m)
        {
            BigNumber x1 = new BigNumber("1");
            BigNumber x2 = new BigNumber("0");
            BigNumber x3 = m;
            BigNumber y1 = new BigNumber("0");
            BigNumber y2 = new BigNumber("1");
            BigNumber y3 = b;

            while(y3 != new BigNumber("0") && y3 != new BigNumber("1"))
            {
                BigNumber g = x3 / y3;
                BigNumber t1 = x1 - g * y1;
                BigNumber t2 = x2 - g * y2;
                BigNumber t3 = x3 - g * y3;

                x1 = y1;
                x2 = y2;
                x3 = y3;

                y1 = t1;
                y2 = t2;
                y3 = t3;
            }

            if(y3 == new BigNumber("0"))
            {
                Console.WriteLine("Multiplicative inverse does not exist.");
                throw new DivideByZeroException();
            }
            else
            {
                return y2;
            }
        }

        public static void basicOperations()
        {
            Console.WriteLine("Actions available: + - / * ^ > < == IntSqrt\nEnter an expression: ");
            string s = Console.ReadLine();
            Regex regex = new Regex(@"^-?[0-9]+$");

            if (s.Split(' ').Length == 2)
            {
                if (!regex.IsMatch(s.Split(' ')[0]))
                {
                    Console.WriteLine("NaN");
                    return;
                }
                BigNumber x = new BigNumber(s.Split(' ')[0]);
                if (s.Split(' ')[1] == "IntSqrt")
                {
                    try
                    {
                        Console.WriteLine(BigNumber.IntSqrt(x));
                        return;
                    }
                    catch (StackOverflowException e)
                    {
                        Console.WriteLine("Number is too big for this operation!");
                        return;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Syntax");
                }
                return;
            }
            else if (s.Split(' ').Length == 3)
            {
                if (!regex.IsMatch(s.Split(' ')[0]) || !regex.IsMatch(s.Split(' ')[2]))
                {
                    Console.WriteLine("NaN");
                    return;
                }
                BigNumber a = new BigNumber(s.Split(' ')[0]);
                BigNumber b = new BigNumber(s.Split(' ')[2]);
                switch (s.Split(' ')[1])
                {
                    case "+":
                        Console.WriteLine((a + b));
                        break;
                    case "-":
                        Console.WriteLine((a - b));
                        break;
                    case "*":
                        Console.WriteLine((a * b));
                        break;
                    case "/":
                        Console.WriteLine((a / b));
                        break;
                    case "%":
                        Console.WriteLine((a % b));
                        break;
                    case "^":
                        Console.WriteLine((a ^ b));
                        break;
                    case ">":
                        Console.WriteLine((a > b));
                        break;
                    case "<":
                        Console.WriteLine((b > a));
                        break;
                    case "==":
                        Console.WriteLine((b == a));
                        break;
                    case "GCD":
                        Console.WriteLine(BigNumber.GCD(a, b));
                        break;
                    case "LCM":
                        Console.WriteLine(BigNumber.LCM(a, b));
                        break;
                    default:
                        Console.WriteLine("Invalid input.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid syntax!");
                return;
            }
        }
    }
}
