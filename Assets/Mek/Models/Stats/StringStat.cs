﻿using System;

namespace Mek.Models.Stats
{
    public class StringStat : BaseStat
    {
        public string CurrentValue { get; protected set; }

        public StringStat(string initial)
        {
            CurrentValue = initial;
        }

        public override T Get<T>()
        {
            ValidateType(typeof(T));

            return (T)Convert.ChangeType(CurrentValue, typeof(string));
        }

        public override bool Set<T>(T value)
        {
            ValidateType(typeof(T));

            var newValue = Convert.ToString(value);

            CurrentValue = newValue;

            return base.Set(newValue);
        }

        public override Type GetStatType()
        {
            return typeof(string);
        }
    }
}