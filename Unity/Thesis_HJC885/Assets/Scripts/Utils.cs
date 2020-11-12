using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class Utils 
{
    public class Point
    {
        public float x { get; set; }
        public float y { get; set; }
    
    }

    public class solutions_texthandler
    {
        public Vector3 bulletdest;
        public Vector3 bulletdestpic;
        public List<Vector3> movement;

        public solutions_texthandler()
        {
            movement = new List<Vector3>();
        }

        public solutions_texthandler(Vector3 bulletdest, Vector3 bulletdestpic, List<Vector3> movement) : this()
        {
            this.movement = movement;
            this.bulletdestpic = bulletdestpic;
            this.bulletdest = bulletdest;
        }

    }


    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }

    public static System.Random random = new System.Random();

    public static int GetIndexOfStructKey(List<solutions_texthandler> dic, Vector3 key)
    {
        int i = 0;

        foreach (solutions_texthandler item in dic)
        {
            if (key == item.bulletdest)
            {
                return i;
            }
            i++;
        }


        return -1;
        
    }
}
