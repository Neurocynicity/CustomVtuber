using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field)]
public class InterfaceAttribute : PropertyAttribute
{
    public Type InterfaceType;

    public InterfaceAttribute(Type interfaceType)
    {
        InterfaceType = interfaceType;
    }
}
