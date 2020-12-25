using EasyCrawling.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Markup;

namespace EasyCrawling.Helpers
{
    public class EnumHelper 
    {
        public static string StringValueOf<enumType>(enumType actionType, ReturnType returnType = ReturnType.TEXT)
        {          
            FieldInfo fi = actionType.GetType().GetField(actionType.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            ClassDescriptionAttribute[] classAttributes = 
                (ClassDescriptionAttribute[])fi.GetCustomAttributes(typeof(ClassDescriptionAttribute), false);


            if (attributes.Length > 0 && returnType == ReturnType.TEXT)
            {
                return attributes[0].Description;
            }
            else if (classAttributes.Length > 0 && returnType == ReturnType.CLASS)
            {
                return classAttributes[0].User.AssemblyQualifiedName;
            }
            else
            {
                return actionType.ToString();
            }
        }
    }
}
