using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelper
{
    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{\"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }
    
}

[System.Serializable]
public class PreguntaObject {
    public string QuestionId;
    public int Id;
    public int SpecieId;
    public string Text;
    public List<string> Options;
    public int Answer;
    public string Feedback;
}
