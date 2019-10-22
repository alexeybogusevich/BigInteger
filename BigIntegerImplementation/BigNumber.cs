using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BigIntegerImplementation
{
    public class BigNumber
    {
        string number;
        bool negative;

        public BigNumber(string s)
        {
            negative = false;
            int i = 0;
            if (s[0] == '-')
            {
                negative = true;
                ++i;
            }
            while (i < s.Length - 1 && s[i] == '0')
                ++i;
            number = s.Substring(i);
        }

        public BigNumber(string s, bool sign)
        {
            int i = 0;
            while (i < s.Length - 1 && s[i] == '0')
                i++;
            number = s.Substring(i);
            negative = sign;
        }


        public string GetNumber()
        {
            if (negative)
                return "-" + number;
            else
                return number;
        }

        public static BigNumber operator +(BigNumber a, BigNumber b)
        {
            if(a.negative && !b.negative)
            {
                a.negative = false;
                return b - a;
            }
            else if(!a.negative && b.negative)
            {
                b.negative = false;
                return a - b;
            }
            else if(a.negative && b.negative)
            {
                a.negative = false;
                b.negative = false;
                return new BigNumber("-1") * (a + b);
            }

            string s1 = new string(a.number.Reverse().ToArray());
            string s2 = new string(b.number.Reverse().ToArray());
            if (s1.Length > s2.Length)
            {
                int i = s1.Length - s2.Length;
                for (; i > 0; i--)
                {
                    s2 += "0";
                }
            }
            else
            {
                int i = s2.Length - s1.Length;
                for (; i > 0; i--)
                {
                    s1 += "0";
                }
            }
            string result = "";
            int toAdd = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                result += ((Convert.ToInt32(s1[i].ToString()) + Convert.ToInt32(s2[i].ToString()) + toAdd) % 10).ToString();
                toAdd = (Convert.ToInt32(s1[i].ToString()) + Convert.ToInt32(s2[i].ToString()) + toAdd) / 10;
            }
            if (toAdd == 1)
                result += '1';
            return new BigNumber(new string(result.Reverse().ToArray()));
        }

        public static bool operator <(BigNumber a, BigNumber b)
        {
            if (a.number.Length < b.number.Length && a.negative == b.negative)
                return true;
            if (a.number.Length > b.number.Length && a.negative == b.negative)
                return false;
            if (a.negative && !b.negative)
                return true;
            if (!a.negative && b.negative)
                return false;
            for (int i = 0; i < a.number.Length; i++)
            {
                if (Convert.ToInt32(a.number[i].ToString()) < Convert.ToInt32(b.number[i].ToString())) return true;
                if (Convert.ToInt32(a.number[i].ToString()) > Convert.ToInt32(b.number[i].ToString())) return false;
            }
            return false;
        }

        public static bool operator >(BigNumber a, BigNumber b)
        {
            if (a.number.Length < b.number.Length && a.negative == b.negative)
                return false;
            if (a.number.Length > b.number.Length && a.negative == b.negative)
                return true;
            if (a.negative && !b.negative)
                return false;
            if (!a.negative && b.negative)
                return true;
            for (int i = 0; i < a.number.Length; i++)
            {
                if (Convert.ToInt32(a.number[i].ToString()) > Convert.ToInt32(b.number[i].ToString())) return true;
                if (Convert.ToInt32(a.number[i].ToString()) < Convert.ToInt32(b.number[i].ToString())) return false;
            }
            return false;
        }

        public static bool operator ==(BigNumber a, BigNumber b)
        {
            return !(a > b) && !(a < b);
        }

        public static bool operator !=(BigNumber a, BigNumber b)
        {
            return !(a == b);
        }

        public static bool operator >=(BigNumber a, BigNumber b)
        {
            return (a > b) || (a == b);
        }

        public static bool operator <=(BigNumber a, BigNumber b)
        {
            return (a < b) || (a == b);
        }

        public static BigNumber operator -(BigNumber a)
        {
            return new BigNumber(a.number, true);
        }

        public static BigNumber operator -(BigNumber a, BigNumber b)
        {
            if(a.negative && b.negative)
            {
                a.negative = false;
                b.negative = false;
                return b - a;
            }
            else if(!a.negative && b.negative)
            {
                b.negative = false;
                return a + b;
            }
            else if(a.negative && !b.negative)
            {
                a.negative = false;
                return new BigNumber("-1") * (a + b);
            }

            if (b > a)
                return new BigNumber((b - a).number, true);

            string s1 = new string(a.number.Reverse().ToArray());
            string s2 = new string(b.number.Reverse().ToArray());
            if (s1.Length > s2.Length)
            {
                int i = s1.Length - s2.Length;
                for (; i > 0; i--)
                {
                    s2 += "0";
                }
            }
            else
            {
                int i = s2.Length - s1.Length;
                for (; i > 0; i--)
                {
                    s1 += "0";
                }
            }
            string result = "";
            int toAdd = 0;
            for (int i = 0; i < s1.Length; i++)
            {
                if ((Convert.ToInt32(s1[i].ToString()) - Convert.ToInt32(s2[i].ToString()) - toAdd) < 0)
                {
                    result += ((Convert.ToInt32(s1[i].ToString()) - Convert.ToInt32(s2[i].ToString()) + 10 - toAdd) % 10).ToString();
                    toAdd = 1;
                }
                else
                {
                    result += ((Convert.ToInt32(s1[i].ToString()) - Convert.ToInt32(s2[i].ToString()) - toAdd) % 10).ToString();
                    toAdd = 0;
                }
            }
            if (toAdd == -1)
                result += "1-";
            return new BigNumber(new string(result.Reverse().ToArray()));
        }

        public static BigNumber operator *(BigNumber a, BigNumber b)
        {
            string s1 = new string(a.number.Reverse().ToArray());
            string s2 = new string(b.number.Reverse().ToArray());
            if (s1.Length > s2.Length)
            {
                int i = s1.Length - s2.Length;
                for (; i > 0; i--)
                {
                    s2 += "0";
                }
            }
            else
            {
                int i = s2.Length - s1.Length;
                for (; i > 0; i--)
                {
                    s1 += "0";
                }
            }
            string q = "";
            BigNumber answer = new BigNumber("0");
            for (int i = 0; i < s1.Length; i++)
            {
                int toAdd = 0;
                string result = "";
                for (int j = 0; j < s2.Length; j++)
                {
                    result += ((Convert.ToInt32(s1[i].ToString()) * Convert.ToInt32(s2[j].ToString()) + toAdd) % 10).ToString();
                    toAdd = (Convert.ToInt32(s1[i].ToString()) * Convert.ToInt32(s2[j].ToString()) + toAdd) / 10;
                }
                if (toAdd > 0)
                {
                    result += new string(toAdd.ToString().Reverse().ToArray());
                }
                answer = answer + new BigNumber(new string(result.Reverse().ToArray()) + q);
                q += "0";
            }
            if ((a.negative && !b.negative) || (!a.negative && b.negative))
                answer.negative = true;
            return answer;
        }

        public static BigNumber operator /(BigNumber a, BigNumber b)
        {
            bool neg = (a.negative && !b.negative) || (!a.negative && b.negative);
            a.negative = false;
            b.negative = false;

            if (b.number == "0")
                throw new DivideByZeroException();
            if (a < b) return new BigNumber("0");
            string left = a.number.Substring(0, b.number.Length);
            string result = "";
            for (int i = b.number.Length; i <= a.number.Length; i++)
            {
                int ok = 0;
                for (int j = 1; j <= 9; j++)
                {
                    if (b * new BigNumber(j.ToString()) <= new BigNumber(left))
                        ok = j;
                    else
                        break;
                }
                result += ok.ToString();
                if (i < a.number.Length)
                    left = (new BigNumber(left) - b * new BigNumber(ok.ToString())).number + a.number[i];
            }

            BigNumber answer = new BigNumber(result);
            answer.negative = neg;

            return answer;
        }

        public static BigNumber operator %(BigNumber a, BigNumber b)
        {
            if (b.number == "0")
                throw new DivideByZeroException();
            return a - (a / b) * b;
        }

        public static BigNumber mod(BigNumber a, BigNumber b)
        {
            return a % b;
        }

        public static BigNumber operator ^(BigNumber a, BigNumber b)
        {
            //b to base of 2
            string b_bin = "";
            while(b > new BigNumber("0"))
            {
                b_bin = (b % new BigNumber("2")).number + b_bin;
                b = b / new BigNumber("2");
            }

            string res = a.number;

            for(int i = 1; i<b_bin.Length; ++i)
            {
                BigNumber temp = new BigNumber(res);
                temp = temp * temp;
                if (b_bin[i] == '1')
                {
                    temp = temp * a;
                }
                res = temp.number;
            }

            return new BigNumber(res);
        }

        public static BigNumber Shift(BigNumber a)
        {
            if(a == new BigNumber("2"))
            {
                return new BigNumber("1");
            }
            if(a == new BigNumber("1"))
            {
                return new BigNumber("0");
            }
            string a_bin = "";
            while (a > new BigNumber("0"))
            {
                a_bin = (a % new BigNumber("2")).number + a_bin;
                a = a / new BigNumber("2");
            }
            StringBuilder sb = new StringBuilder();
            sb.Append("0");
            sb.Append(a_bin.Substring(0, a_bin.Length - 1));
            string s = sb.ToString();
            BigNumber res = new BigNumber("0");
            for(int i = 0; i < s.Length; ++i)
            {
                if(s[i] == '1')
                {
                    BigNumber temp = new BigNumber("2") ^ new BigNumber((s.Length - i - 1).ToString());
                    res = res + temp;
                }
            }
            return res;
        }

        public static BigNumber ModExp(BigNumber x, BigNumber y, BigNumber n)
        {
            if(y == new BigNumber("0"))
            {
                return new BigNumber("1");
            }
            BigNumber z = ModExp(x, y / new BigNumber("2"), n);
            if(y % new BigNumber("2") == new BigNumber("0"))
            {
                return (z * z) % n;
            }
            else
            {
                return (x * z * z) % n;
            }
        }

        public static BigNumber GCD(BigNumber a, BigNumber b)
        {
            BigNumber zero = new BigNumber("0");
            while (a > zero && b > zero)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }
            if (a > b)
                return a;
            else
                return b;
        }

        public static string IntSqrt(BigNumber a)
        {
            if(a.negative)
            {
                Console.WriteLine("Square root for a negative value does not exist!");
                return "";
            }

            return sq(a).number;
        }
        
        public static BigNumber sq(BigNumber x)
        {
            if (x < new BigNumber("4"))
            {
                return new BigNumber("1");
            }
            BigNumber r = new BigNumber("2") * sq((x - x % new BigNumber("4")) / new BigNumber("4"));
            if ((r + new BigNumber("1"))* (r + new BigNumber("1")) <= x)
            {
                return r + new BigNumber("1");
            }
            else
            {
                return r;
            }
        }

        public static BigNumber Core(BigNumber x, BigNumber k)
        {
            if(x < k)
            {
                return new BigNumber("0");
            }
            return new BigNumber("1") + Core(x - k, k + new BigNumber("2"));
        }

        public static BigNumber LCM(BigNumber a, BigNumber b)
        {
            return a / GCD(a, b) * b;
        }

        public static BigNumber modPow(BigNumber num, BigNumber pow, BigNumber mod)
        {
            BigNumber test;
            BigNumber n = num;
            for (test = new BigNumber("1"); pow != new BigNumber("0"); pow = Shift(pow))
            {
                if (pow.number[pow.number.Length - 1] == '1')
                    test = ((test % mod) * (n % mod)) % mod;
                n = ((n % mod) * (n % mod)) % mod;
            }

            return test; /* note this is potentially lossy */
        }

        public override string ToString()
        {
            return negative? "-" + number : number;
        }
    }
}
