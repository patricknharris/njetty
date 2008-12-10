#region Apache License
//
// Licensed to the Apache Software Foundation (ASF) under one or more 
// contributor license agreements. See the NOTICE file distributed with
// this work for additional information regarding copyright ownership. 
// The ASF licenses this file to you under the Apache License, Version 2.0
// (the "License"); you may not use this file except in compliance with 
// the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace NJetty.Util.Util
{

    /// <summary>
    /// IntrospectionUtil
    /// </summary>
    /// <author>  
    ///     <a href="mailto:leopoldo.agdeppa@gmail.com">Leopoldo Lee Agdeppa III</a>
    /// </author>
    /// <date>
    /// December 2008
    /// </date>
    public class IntrospectionUtil
    {

        const BindingFlags _BINDINGFLAGS = BindingFlags.NonPublic  | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static;
        
        public static MethodInfo FindMethod(Type type, string methodName, Type[] args, bool checkInheritance, bool strictArgs)
        {
            if (type == null)
                throw new ArgumentNullException("No Type");
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("No method name");

            MethodInfo method = null;
            MethodInfo[] methods = type.GetMethods(_BINDINGFLAGS);
            for (int i=0;i<methods.Length && method==null;i++)
            {
                if (methods[i].Name.Equals(methodName) && CheckParams(methods[i].GetParameters(), (args==null?new Type[] {}:args), strictArgs))
                {
                    method = methods[i];
                }
                
            }

            if (method!=null)
            {
                return method;
            }
            else if (checkInheritance)
            {
                return FindInheritedMethod(type.Assembly, type.BaseType, methodName, args, strictArgs);
            }
            else
            {

                throw new ArgumentException("No such method " + methodName + " on class " + type.Name);
            }

        }



        public static PropertyInfo FindProperty(Type type, string properttName, bool checkInheritance)
        {
            if (type == null)
                throw new ArgumentNullException("No Type");
            if (string.IsNullOrEmpty(properttName))
                throw new ArgumentNullException("No property name");

            PropertyInfo property = null;
            PropertyInfo[] properties = type.GetProperties(_BINDINGFLAGS);
            for (int i = 0; i < properties.Length && property == null; i++)
            {
                if (properties[i].Name.Equals(properttName))
                {
                    property = properties[i];
                }

            }

            if (property != null)
            {
                return property;
            }
            else if (checkInheritance)
            {
                return FindInheritedProperty(type.Assembly, type.BaseType, properttName);
            }
            else
            {

                throw new ArgumentException("No such method " + properttName + " on class " + type.Name);
            }

        }
        
        
        
        

        public static FieldInfo FindField (Type type, string targetName, Type targetType, bool checkInheritance, bool strictType)
        {
            if (type == null)
                throw new ArgumentNullException("No Type");
            if (string.IsNullOrEmpty(targetName))
                throw new ArgumentNullException("No field name");
            
            try
            {
                FieldInfo field = type.GetField(targetName, _BINDINGFLAGS);
                if (field == null)
                {
                    return FindInheritedField(type.Assembly, type.BaseType, targetName, targetType, strictType);
                }
               

                if (strictType)
                {
                    if (field.FieldType.Equals(targetType))
                        return field;
                }
                else
                {
                    if (field.FieldType.IsAssignableFrom(targetType))
                        return field;
                }


                if (checkInheritance)
                {
                        return FindInheritedField(type.Assembly, type.BaseType, targetName, targetType, strictType);
                }
                else
                    throw new ArgumentException("No field with name " + targetName + " in class " + type.Name + " of type " + targetType);
            }
            catch (ArgumentException)
            {
                return FindInheritedField(type.Assembly,type.BaseType, targetName,targetType,strictType);
            }
        }
        
        
        
        
        
        public static bool IsInheritable (Assembly assembly, MethodInfo member)
        {
            if (member==null)
                return false;


            if (member.IsPublic)
                return true;
            if (!member.IsPrivate && assembly.Equals(member.DeclaringType.Assembly))
                return true;
           
            return false;
        }



        public static bool IsInheritable(Assembly assembly, PropertyInfo member)
        {
            if (member == null)
                return false;


            return IsInheritable(assembly, member.GetSetMethod(true)) || IsInheritable(assembly, member.GetGetMethod(true));
        }



        public static bool IsInheritable(Assembly assembly, FieldInfo member)
        {
            if (member == null)
                return false;


            if (member.IsPublic)
                return true;
            if (!member.IsPrivate && assembly.Equals(member.DeclaringType.Assembly))
                return true;

            return false;
        }



        public static bool CheckParams(ParameterInfo[] formalParams, Type[] actualParams, bool strict)
        {
            Type[] types = null;

            if (formalParams != null)
            {
                types = new Type[formalParams.Length];

                for (int i = 0; i < formalParams.Length; i++)
                {
                    types[i] = formalParams[i].ParameterType;
                }
            }

            return CheckParams(types, actualParams, strict);

        }
        
        public static bool CheckParams (Type[] formalParams, Type[] actualParams, bool strict)
        {
            if (formalParams==null && actualParams==null)
                return true;
            if (formalParams==null && actualParams!=null)
                return false;
            if (formalParams!=null && actualParams==null)
                return false;

            if (formalParams.Length!=actualParams.Length)
                return false;

            if (formalParams.Length==0)
                return true; 
            
            int j=0;
            if (strict)
            {
                while (j<formalParams.Length && formalParams[j].Equals(actualParams[j]))
                    j++;
            }
            else
            { 
                while ((j<formalParams.Length) && (formalParams[j].IsAssignableFrom(actualParams[j])))
                {
                    j++;
                }
            }

            if (j!=formalParams.Length)
            {
                return false;
            }

            return true;
        }
        
        
        public static bool IsSameSignature (MethodInfo methodA, MethodInfo methodB)
        {
            if (methodA==null)
                return false;
            if (methodB==null)
                return false;

            ParameterInfo[] parametersA = methodA.GetParameters();
            ParameterInfo[] parametersB = methodB.GetParameters();

            if (methodA.Name.Equals(methodB.Name) && parametersA.Length == parametersB.Length)
            {
                for (int i = 0; i < parametersA.Length; i++)
                {
                    if(!(
                        parametersA[i].ParameterType.Equals(parametersB[i].ParameterType)
                        && parametersA[i].IsIn == parametersB[i].IsIn
                        && parametersA[i].IsOptional == parametersB[i].IsOptional
                        && parametersA[i].IsOut == parametersB[i].IsOut
                        
                        ))
                    {
                        return false;
                    }
                }

                return true;
            }   
            
            return false;
        }
        
        public static bool IsTypeCompatible (Type formalType, Type actualType, bool strict)
        {
            if (formalType==null && actualType != null)
                return false;
            if (formalType!=null && actualType==null)
                return false;
            if (formalType==null && actualType==null)
                return true;

            if (strict)
                return formalType.Equals(actualType);
            else
                return formalType.IsAssignableFrom(actualType);
        }

        
        
        
        public static bool ContainsSameMethodSignature (MethodInfo method, Type c, bool checkAssembly)
        {
            if (checkAssembly)
            {
                if (!c.Assembly.Equals(method.DeclaringType.Assembly))
                    return false;
            }
            
            bool samesig = false;
            MethodInfo[] methods = c.GetMethods(_BINDINGFLAGS);
            for (int i=0; i<methods.Length && !samesig; i++)
            {
                if (IntrospectionUtil.IsSameSignature(method, methods[i]))
                    samesig = true;
            }
            return samesig;
        }


        public static bool ContainsSamePropertyName(PropertyInfo property, Type c, bool checkAssembly)
        {
            if (checkAssembly)
            {
                if (!c.Assembly.Equals(property.DeclaringType.Assembly))
                    return false;
            }

            bool sameName = false;
            PropertyInfo[] properties = c.GetProperties(_BINDINGFLAGS);
            for (int i = 0; i < properties.Length && !sameName; i++)
            {
                if (properties[i].Name.Equals(property.Name))
                    sameName = true;
            }
            return sameName;
        }
        
        public static bool ContainsSameFieldName(FieldInfo field, Type c, bool checkAssembly)
        {
            if (checkAssembly)
            {
                if (!c.Assembly.Equals(field.DeclaringType.Assembly))
                    return false;
            }
            
            bool sameName = false;
            FieldInfo[] fields = c.GetFields(_BINDINGFLAGS);
            for (int i=0;i<fields.Length && !sameName; i++)
            {
                if (fields[i].Name.Equals(field.Name))
                    sameName = true;
            }
            return sameName;
        }
        
        
        
        protected static MethodInfo FindInheritedMethod (Assembly assembly, Type type, string methodName, Type[] args, bool strictArgs)
        {
            if (type==null)
                throw new ArgumentNullException("No Type");
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("No method name");
            
            MethodInfo method = null;
            MethodInfo[] methods = type.GetMethods(_BINDINGFLAGS);
            for (int i=0;i<methods.Length && method==null;i++)
            {
                if (methods[i].Name.Equals(methodName) 
                        && IsInheritable(assembly,methods[i])
                        && CheckParams(methods[i].GetParameters(), args, strictArgs))
                    method = methods[i];
            }
            if (method!=null)
            {
                return method;
            }
            else if (type.BaseType == null)
            {
                throw new ArgumentException("Method Not Found!!!");
            }
            else
                return FindInheritedMethod(type.Assembly, type.BaseType, methodName, args, strictArgs);
        }



        protected static PropertyInfo FindInheritedProperty(Assembly assembly, Type type, string propertyName)
        {
            if (type == null)
                throw new ArgumentNullException("No Type");
            if (string.IsNullOrEmpty(propertyName))
                throw new ArgumentNullException("No property name");

            PropertyInfo property = null;
            PropertyInfo[] properties = type.GetProperties(_BINDINGFLAGS);
            for (int i = 0; i < properties.Length && property == null; i++)
            {
                if (properties[i].Name.Equals(propertyName)
                        && IsInheritable(assembly, properties[i]))
                    property = properties[i];
            }
            if (property != null)
            {
                return property;
            }
            else if (type.BaseType == null)
            {
                throw new ArgumentException("No such property " + propertyName + " on class " + type.Name);
            }
            else
                return FindInheritedProperty(type.Assembly, type.BaseType, propertyName);
        }
        
        protected static FieldInfo FindInheritedField (Assembly assembly, Type type, string fieldName, Type fieldType, bool strictType)
        {
            if (type==null)
                throw new ArgumentNullException ("No Type");
            if (fieldName==null)
                throw new ArgumentNullException("No field name");
            try
            {
                FieldInfo field = type.GetField(fieldName, _BINDINGFLAGS);
                bool b = IsInheritable(assembly, field);
                
                if (IsInheritable(assembly, field) && IsTypeCompatible(fieldType, field.FieldType, strictType))
                    return field;
                else if (type.BaseType == null)
                {
                    throw new ArgumentException("Field Not Found!!!");
                }
                else
                    return FindInheritedField(type.Assembly, type.BaseType, fieldName, fieldType, strictType);
            }
            catch (ArgumentException)
            {
                return FindInheritedField(type.Assembly, type.BaseType,fieldName, fieldType, strictType); 
            }
        }
    }
}
