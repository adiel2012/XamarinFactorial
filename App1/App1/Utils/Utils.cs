using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace App1.Utils
{
    public static class Utils
    {
    }


    public class IntToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            return (int)value != 0;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            return (bool)value ? 1 : 0;
        }
    }


    public class FactorialToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            return (int)value > 0 ? $"The Factorial is {value}" : "" ;
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            return -1;
        }
    }

    
    public class StringToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            int result = -1;
            int.TryParse(value.ToString(), out result);
            return result;            
        }
        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            return value?.ToString() ?? "";
        }
    }


    public abstract class ExtendedBindableObject : BindableObject
    {
        public void RaisePropertyChanged<T>(Expression<Func<T>> property)
        {
            var name = GetMemberInfo(property).Name;
            OnPropertyChanged(name);
        }

        private MemberInfo GetMemberInfo(Expression expression)
        {
            MemberExpression operand;
            LambdaExpression lambdaExpression = (LambdaExpression)expression;
            if (lambdaExpression.Body as UnaryExpression != null)
            {
                UnaryExpression body = (UnaryExpression)lambdaExpression.Body;
                operand = (MemberExpression)body.Operand;
            }
            else
            {
                operand = (MemberExpression)lambdaExpression.Body;
            }
            return operand.Member;
        }
    }
}
