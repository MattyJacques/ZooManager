// Sifaka Game Studios (C) 2017

using System;

namespace Assets.Scripts.Core.Attributes
{
    public enum UnityReflectionAttributeType
    {
        StringType,
        BoolType,
        FloatType
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class UnityReflectionAttribute : Attribute
    {
        public readonly UnityReflectionAttributeType CurrentType;

        public UnityReflectionAttribute(UnityReflectionAttributeType inType)
        {
            CurrentType = inType;
        }
    }
}
