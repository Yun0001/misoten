using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class, new()
{
   private  static T obj = null;
   private  Singleton() { }

    public static T instance
    {
        get
        {
            if (obj == null)
                obj = new T();
            return obj;
        }
    }
}
