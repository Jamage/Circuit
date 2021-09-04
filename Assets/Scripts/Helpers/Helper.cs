using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Helper
{
    public static T GetRandomEnum<T>()
    {
        int index = UnityEngine.Random.Range(0, Enum.GetValues(typeof(T)).Length);
        return (T)Enum.GetValues(typeof(T)).GetValue(index);
    }
}
