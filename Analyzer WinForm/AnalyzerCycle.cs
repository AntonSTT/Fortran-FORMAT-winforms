using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer_WinForm
{
    static class AnalyzerCycle
    {


        private enum States
        {
            Start,
            Error,
            Final,
            A1,
            A2,
            A3,
            A4,
            A5,
            A6,
            A7,
            A8,
            SP0,
            EL0,
            EL01,
            EL02,
            EL03,
            EL1,
            EL11,
            EL2,
            EL21,
            EL3,
            EL31,
            EL4,
            EL41,
            EL42,
            ELR
        }

        public static int textCount;
        public static int slashCount;
        private static int pos; //position
        private static string str; // analyzed string
        private static ErrTypes err; //тип ошибки
        private static int errpos; //место ошибки
        public static int firstNumPos;
        public static int secondNumPos;
        public static StringBuilder firstNumSb = new StringBuilder();
        public static StringBuilder secondNumSb = new StringBuilder();
        public static StringBuilder anyconst = new StringBuilder();

        private static void SetError(ErrTypes errtype, int errp)
        {
            err = errtype;
            errpos = errp;
        }

        public static Resulter Check(string input)
        {
            pos = 0;
            str = input;

            SetError(ErrTypes.None, -1);
            RunAnalyzer();
            return new Resulter(errpos, err);
        }

        public static StringBuilder stringBuilder = new StringBuilder();
        public static string Semantics()
        {
            stringBuilder = new StringBuilder();
            StringBuilder Sb = new StringBuilder();
            string[] temparr;
            string tempst;
            int pos1 = 0;
            while(str[pos1] != '(')
            {
                pos1++;
            }

            int pos2 = 0;
            while (str[pos2]!= ')')
            {
                pos2++;
            }

            tempst = str.Substring(pos1 + 1, pos2 - pos1 - 1);

            temparr = tempst.Split(' ');
            foreach (string i in temparr)
            {
                Sb.Append(i);
            }

            temparr = Sb.ToString().Split(',');
            foreach (string x in temparr)
            {
                if (x[0] == '\'')
                {
                    stringBuilder.Append(x.Substring(1, x.Length - 2));
                }

                if (Char.IsDigit(x[0]))
                {
                    StringBuilder constant = new StringBuilder();
                    int j = 0;
                    while(x[j] != 'X')
                    {
                        constant.Append(x[j]);
                        j++;
                    }
                    int count = Convert.ToInt32(constant.ToString());
                    for(int i = 0; i < count; i++)
                    {
                        stringBuilder.Append('_');
                    }
                }

                if (x[0] == 'F')
                {
                    int j = 0;
                    StringBuilder constant = new StringBuilder();
                    for (int i = 1; i < x.Length; i++)
                    {
                        if (x[i] == '.')
                        {
                            j = i;
                            break;
                        }
                        constant.Append(x[i]);
                    }

                    int value = Convert.ToInt32(constant.ToString());

                    constant = new StringBuilder();



                    for (int i = j + 1; i < x.Length; i++)
                    {
                        constant.Append(x[i]);
                    }
                    int val2 = Convert.ToInt32(constant.ToString());

                    stringBuilder.Append('F');
                    for(int i = 0; i < value - 2 - val2; i++)
                    {
                        stringBuilder.Append('I');
                    }
                    stringBuilder.Append('.');
                    for (int i = 0; i < val2; i++)
                    {
                        stringBuilder.Append('I');
                    }

                }

                if (x[0] == 'I')
                {
                    StringBuilder constant = new StringBuilder();
                    for (int i = 1; i < x.Length; i++)
                    {
                        constant.Append(x[i]);
                    }

                    int value = Convert.ToInt32(constant.ToString());

                    stringBuilder.Append('F');

                    for (int i = 0; i < value - 1; i++)
                    {
                        stringBuilder.Append('I');
                    }
                }

                if (x[0] == '/')
                {
                    int sc = 0;
                    for (int i = 0; i < x.Length; i++)
                    {
                        if (x[i] == '/')
                        {
                            sc++;
                            stringBuilder.Append('\n');
                        }
                        if (sc > 3)
                        {
                            return "Ошибка: должно быть не больше трех знаков /";
                        }
                    }
                }
                
            }
            return stringBuilder.ToString();

        }

        private static bool RunAnalyzer()
        {
            anyconst = new StringBuilder();
            firstNumSb = new StringBuilder();
            secondNumSb = new StringBuilder();
            textCount = 0;
            slashCount = 0;
            firstNumPos = 0;
            secondNumPos = 0;
            States state = States.Start; //current state = starting state

            while((state != States.Final)&& (state != States.Error))
            {
                if(pos >= str.Length)
                {
                    if (str[str.Length - 1] == '(')
                    {
                        state = States.Error;
                        SetError(ErrTypes.expectedParameter, pos);
                    }
                    else if (str.Length >= 6 && str.Substring(0, 6) == "FORMAT")
                    {
                        state = States.Error;
                        SetError(ErrTypes.expectedParameterList, pos);
                    }
                    else
                    {
                        state = States.Error;
                        SetError(ErrTypes.FORMATexpected, pos);
                    }

                }
                else
                {
                    switch(state)
                    {
                        case States.Start://awaiting F or space
                            if(str[pos] == ' ')
                            {
                                state = States.Start;
                            }
                            else if (str[pos] == 'F')
                            {
                                state = States.A1;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.FORMATexpected, pos);
                            }
                            break;
                        case States.A1: //awaiting O for FORMAT
                            if(str[pos] == 'O')
                            {
                                state = States.A2;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.FORMATexpected, pos);
                            }
                            break;
                        case States.A2: //awaiting R for FORMAT
                            if(str[pos] == 'R')
                            {
                                state = States.A3;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.FORMATexpected, pos);
                            }
                            break;
                        case States.A3: //awaiting M for FORMAT
                            if(str[pos] == 'M')
                            {
                                state = States.A4;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.FORMATexpected, pos);
                            }
                            break;
                        case States.A4: //awaiting A for FORMAT
                            if (str[pos] == 'A')
                            {
                                state = States.A5;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.FORMATexpected, pos);
                            }
                            break;
                        case States.A5: //awaiting T for format
                            if (str[pos] == 'T')
                            {
                                state = States.A6;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.FORMATexpected, pos);
                            }
                            break;
                        case States.A6: //awaiting '(' or space
                            if(str[pos] == ' ')
                            {
                                state = States.A6;
                            }
                            else if(str[pos] == '(')
                            {
                                state = States.SP0;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedParameterList, pos);
                            }
                            break;
                        case States.SP0: //space before elements + transition to elements, awaiting element beginning
                            if (str[pos] == ' ')
                            {
                                state = States.SP0;
                            }
                            else if (str[pos] == 'F') //F <const1>.<const2>
                            {
                                state = States.EL0;
                                firstNumPos = pos+1;
                            }
                            else if (Char.IsDigit(str[pos]) && str[pos] != '0') //<const>X
                            {
                                anyconst = new StringBuilder();
                                anyconst.Append(str[pos]);
                                state = States.EL3;
                            }
                            else if (str[pos] == '\'') //'текст'
                            {
                                textCount = 0;
                                state = States.EL1;
                            }
                            else if (str[pos] == 'I') //I<const>
                            {
                                state = States.EL2;
                            }
                            else if (str[pos] == '/')// /
                            {
                                slashCount = 0;
                                state = States.EL41;
                                slashCount++;
                                
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedParameter, pos);
                            }
                            break;
                        case States.EL0: //awaiting constant first number
                            if (Char.IsDigit(str[pos]) && str[pos] != '0')
                            {
                                firstNumSb = new StringBuilder();
                                firstNumSb.Append(str[pos]);
                                state = States.EL01;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedConstant, pos);
                            }
                            break;
                        case States.EL01: //awaiting constant body or separator
                            if (Char.IsDigit(str[pos]))
                            {
                                firstNumSb.Append(str[pos]);
                                state = States.EL01;
                                
                                
                            }
                            else if (str[pos] == '.')
                            {
                                state = States.EL02;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedSeparator, pos);
                            }
                            break;
                        case States.EL02: //awaiting constant head
                            if (Char.IsDigit(str[pos]) && str[pos] != '0')
                            {
                                secondNumSb = new StringBuilder();
                                secondNumSb.Append(str[pos]);
                                state = States.EL03;
                                secondNumPos = pos;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedConstant, pos);
                            }
                            break;
                        case States.EL03: //awaiting constant body
                            if (Char.IsDigit(str[pos]))
                            {
                                secondNumSb.Append(str[pos]);
                                state = States.EL03;

                            }
                            else if(Convert.ToInt32(firstNumSb.ToString())>256)
                            {
                                state = States.Error;
                                SetError(ErrTypes.SemanticsConstTooLarge, firstNumPos);
                            }
                            else if (Convert.ToInt32(secondNumSb.ToString()) > 256)
                            {
                                state = States.Error;
                                SetError(ErrTypes.SemanticsConstTooLarge, secondNumPos);
                            }
                            else if (Convert.ToInt32(firstNumSb.ToString()) < Convert.ToInt32(secondNumSb.ToString()) +2)
                            {
                                state = States.Error;
                                SetError(ErrTypes.SemanticsConstantDiff, firstNumPos);
                            }
                            else if (str[pos] == ',')
                            {
                                state = States.SP0;
                            }
                            else if (str[pos] == ')')
                            {
                                state = States.Final;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedEndParameter, pos);
                            }
                            break;
                        case States.EL1: //'text' awaited (any symbol or ')
                            if (str[pos] != '\'')
                            {
                                textCount++;
                                state = States.EL1;
                            }
                            else if (textCount > 50)
                            {
                                state = States.Error;
                                SetError(ErrTypes.SemanticsStringLong, pos-textCount + 50);
                            }
                            else if (str[pos] == '\'')
                            {
                                state = States.EL11;
                            }
                            break;
                        case States.EL11: //, after text or end of parlist
                            if (str[pos] == ',')
                            {
                                state = States.SP0;
                            }
                            else if (str[pos] == ' ')
                            {
                                state = States.A7;
                            }
                            else if (str[pos] == ')')
                            {
                                state = States.Final;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedEndParameter, pos);
                            }
                            break;
                        case States.EL2: //I<const>, awaiting const head
                            if (Char.IsDigit(str[pos]) && str[pos] != '0')
                            {
                                anyconst = new StringBuilder();
                                anyconst.Append(str[pos]);
                                state = States.EL21;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedConstant, pos);
                            }
                            break;
                        case States.EL21: //awaiting const body or end of parameter(, or end of parlist)
                            if (Char.IsDigit(str[pos]))
                            {
                                anyconst.Append(str[pos]);
                                state = States.EL21;
                            }
                            else if (Convert.ToInt32(anyconst.ToString())>256)
                            {
                                state = States.Error;
                                SetError(ErrTypes.SemanticsConstTooLarge, pos - anyconst.Length);
                            }
                            else if (str[pos] == ',')
                            {
                                state = States.SP0;
                            }
                            else if (str[pos] == ' ')
                            {
                                state = States.A7;
                            }
                            else if (str[pos] == ')')
                            {
                                state = States.Final;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedEndParameter, pos);
                            }
                            break;
                        case States.EL3: //awaiting constant body or X
                            if (Char.IsDigit(str[pos]))
                            {
                                anyconst.Append(str[pos]);
                                state = States.EL3;
                            }
                            else if (Convert.ToInt32(anyconst.ToString()) > 256)
                            {
                                state = States.Error;
                                SetError(ErrTypes.SemanticsConstTooLarge, pos - anyconst.Length);
                            }
                            else if (str[pos] == 'X')
                            {
                                state = States.EL31;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedX, pos);
                            }
                            break;
                        case States.EL31: //awaiting , or end of parlist
                            if (str[pos] == ',')
                            {
                                state = States.SP0;
                            }
                            else if (str[pos] == ' ')
                            {
                                state = States.A7;
                            }
                            else if (str[pos] == ')')
                            {
                                state = States.Final;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedEndParameter, pos);
                            }
                            break;
                        case States.EL41: //awaiting /(many), or end of parameter list
                            if (str[pos] == '/')
                            {
                                state = States.EL41;
                                slashCount++;
                            }
                            else if (slashCount > 3)
                            {
                                state = States.Error;
                                SetError(ErrTypes.SemanticsTooManySlash, pos-slashCount+3);
                            }
                            else if (str[pos] == ',')
                            {
                                state = States.SP0;
                            }
                            else if (str[pos] == ' ')
                            {
                                state = States.A7;
                            }
                            else if (str[pos] == ')')
                            {
                                state = States.Final;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedEndParameter, pos);
                            }
                            break;
                        case States.A7: //optional spaces before end
                            if (str[pos] == ' ')
                            {
                                state = States.A7;
                            }
                            else if (str[pos] == ')')
                            {
                                state = States.Final;
                            }
                            else
                            {
                                state = States.Error;
                                SetError(ErrTypes.expectedParameterListEnd, pos);
                            }
                            break;

                    }
                }
                pos++;
            }
            return (state == States.Final);
        }

    }
}
