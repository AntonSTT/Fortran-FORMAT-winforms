using System;
using System.Collections.Generic;
using System.Text;

namespace Analyzer_WinForm
{
    public class Resulter
    {
        public int errPos;

        public ErrTypes err;

        public Resulter(int errPos, ErrTypes err)
        {
            this.errPos = errPos;
            this.err = err;
        }

        public string ErrMessage
        {
            get
            {
                switch(err)
                {
                    case ErrTypes.None: { return "Нет ошибок"; }
                    case ErrTypes.OutOfRange: { return "Ошибка: Выход за границы строки"; }
                    case ErrTypes.FORMATexpected: { return "Ошибка: ожидался оператор FORMAT"; }
                    case ErrTypes.expectedParameterList: { return "Ожидалась открывающая скобка"; }
                    case ErrTypes.expectedParameterListEnd: { return "Ошибка: ожидался конец списка параметров"; }
                    case ErrTypes.expectedParameter: { return "Ошибка: Ожидался пробел, 'F', '/', 'I', текстовая строка или цифра от 1 до 9"; }
                    case ErrTypes.expectedConstant: { return "Ошибка: Ожидалась константа"; }
                    case ErrTypes.expectedSeparator: { return "Ошибка: Ожидалась цифра от 0 до 9 или точка"; }
                    case ErrTypes.expectedEndParameter: { return "Ошибка: Ожидался конец параметра"; }
                    case ErrTypes.expectedX: { return "Ошибка: Ожидался X"; }
                    case ErrTypes.expectedNewLine: { return "Ошибка: Ожидался /"; }
                    case ErrTypes.SemanticsTooManySlash: { return "Ошибка: не должно быть больше трёх символов /"; }
                    case ErrTypes.SemanticsStringLong: { return "Ошибка: длинна текста не должна превышать 50"; };
                    case ErrTypes.SemanticsConstantDiff: { return "Ошбика: Константа до разделителя меньше константы после + 2"; };
                    case ErrTypes.SemanticsConstTooLarge: { return "Ошибка: Значение константы превышает 256"; }
                    default: { return "Ошибка: Неизвестная ошибка"; }
                }
            }
        }
    }

    public enum ErrTypes
    {
        None,
        OutOfRange,

        FORMATexpected,
        expectedParameterList,
        expectedParameterListEnd,
        expectedParameter,
        expectedConstant,
        expectedSeparator,
        expectedEndParameter,
        expectedX,
        expectedNewLine,

        SemanticsTooManySlash,
        SemanticsStringLong,
        SemanticsConstantDiff,
        SemanticsConstTooLarge,
    }
}
