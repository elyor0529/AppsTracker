﻿using System;

namespace AppsTracker.Domain.Settings
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
    public sealed class SettingsNodeAttribute : Attribute
    {
        private object defaultValue;

        public SettingsNodeAttribute(object nodeDefaultValue = null)
        {
            defaultValue = nodeDefaultValue;
        }

        public object DefaultValue { get { return defaultValue; } }
    }
}
