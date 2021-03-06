﻿//---------------------------------------------------------------------------------
// Copyright 2013 Ceton Corp
// Copyright 2013 Tomasz Cielecki (tomasz@ostebaronen.dk)
// Licensed under the Apache License, Version 2.0 (the "License"); 
// You may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0 

// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, 
// INCLUDING WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR 
// CONDITIONS OF TITLE, FITNESS FOR A PARTICULAR PURPOSE, 
// MERCHANTABLITY OR NON-INFRINGEMENT. 

// See the Apache 2 License for the specific language governing 
// permissions and limitations under the License.
//---------------------------------------------------------------------------------

using System;
using Cheesebaron.MvxPlugins.Settings.Interfaces;
using MonoTouch.Foundation;

namespace Cheesebaron.MvxPlugins.Settings.Touch
{
    public class Settings
        : ISettings
    {
        public T GetValue<T>(string key, T defaultValue = default(T))
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var type = typeof(T);
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }

            object returnVal;
            var defaults = NSUserDefaults.StandardUserDefaults;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    returnVal = defaults.BoolForKey(key);
                    break;
                case TypeCode.Int64:
                case TypeCode.Double:
                    returnVal = defaults.DoubleForKey(key);
                    break;
                case TypeCode.Int32:
                    returnVal = defaults.IntForKey(key);
                    break;
                case TypeCode.Single:
                    returnVal = defaults.FloatForKey(key);
                    break;
                case TypeCode.String:
                    returnVal = defaults.StringForKey(key);
                    break;
                default:
                    returnVal = defaultValue;
                    break;
            }
            return (T)returnVal;
        }

        public bool AddOrUpdateValue<T>(string key, T value = default(T))
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var type = value.GetType();
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }
            var defaults = NSUserDefaults.StandardUserDefaults;
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Boolean:
                    defaults.SetBool(Convert.ToBoolean(value), key);
                    break;
                case TypeCode.Int64:
                case TypeCode.Double:
                    defaults.SetDouble(Convert.ToDouble(value), key);
                    break;
                case TypeCode.Int32:
                    defaults.SetInt(Convert.ToInt32(value), key);
                    break;
                case TypeCode.Single:
                    defaults.SetFloat(Convert.ToSingle(value), key);
                    break;
                case TypeCode.String:
                    defaults.SetString(Convert.ToString(value), key);
                    break;
                default:
                    throw new ArgumentException(string.Format("Type {0} is not supported", type), "value");
            }
            return defaults.Synchronize();
        }

        public bool Contains(string key)
        {
            var defaults = NSUserDefaults.StandardUserDefaults;
            try
            {
                var stuff = defaults[key];
                return stuff != null;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteValue(string key)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentException("Key must have a value", "key");

            var defaults = NSUserDefaults.StandardUserDefaults;
            defaults.RemoveObject(key);
            return defaults.Synchronize();
        }

        public bool ClearAllValues()
        {
            var defaults = NSUserDefaults.StandardUserDefaults;
            defaults.RemovePersistentDomain(NSBundle.MainBundle.BundleIdentifier);
            return defaults.Synchronize();
        }
    }
}
