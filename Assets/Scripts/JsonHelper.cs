using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.data;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.data = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] data;
    }

    public static T[] FromJsons<T>(string json)
    {
        Wrappers<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrappers<T>>(json);
        return wrapper.datas;
    }

    public static string ToJsons<T>(T[] array)
    {
        Wrappers<T> wrapper = new Wrappers<T>();
        wrapper.datas = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrappers<T>
    {
        public T[] datas;
    }

    public static T[] FromJsonl<T>(string json)
    {
        Wrapperl<T> wrapper = UnityEngine.JsonUtility.FromJson<Wrapperl<T>>(json);
        return wrapper.lights;
    }

    public static string ToJsonl<T>(T[] array)
    {
        Wrapperl<T> wrapper = new Wrapperl<T>();
        wrapper.lights = array;
        return JsonUtility.ToJson(wrapper);
    }

    [Serializable]
    private class Wrapperl<T>
    {
        public T[] lights;
    }
}