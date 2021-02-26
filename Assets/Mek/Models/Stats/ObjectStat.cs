using System;
using UnityEngine;

namespace Mek.Models.Stats
{
    public class ObjectStat<T> : StringStat
    {
        public T ObjectValue { get; protected set; }

        private Type _type;

        public ObjectStat(string initial) : base(initial)
        {
            CurrentValue = initial;
            ObjectValue = JsonUtility.FromJson<T>(initial);
        }

        public override TT Get<TT>()
        {
            ValidateType(typeof(TT));

            return (TT)Convert.ChangeType(CurrentValue, typeof(T));
        }

        public override bool Set<TT>(TT value)
        {
            ValidateType(typeof(TT));

            if (TryParseString(JsonUtility.ToJson(value)))
            {
                return base.Set(value);
            }
            return false;
        }

        private bool TryParseString(string value)
        {
            string newValue = Convert.ToString(value);

            if (newValue == JsonUtility.ToJson(CurrentValue))
            {
                return false;
            }

            CurrentValue = newValue;
            ObjectValue = JsonUtility.FromJson<T>(CurrentValue);

            IsDirty = true;
            return true;
        }

        public override Type GetStatType()
        {
            return typeof(string);
        }
    }
}
