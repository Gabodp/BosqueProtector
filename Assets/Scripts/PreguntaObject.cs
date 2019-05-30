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
        public T[] array = null;
    }
    
}
[System.Serializable]
public class StationP
{
    public string StationId;
    public int Id;
    public string APIKey;
    public string Name;
    public int GameStation;
    public string Latitude;
    public string Longitude;
    public string AndroidVersion;
    public string ServicesVersion;
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
    public List<StationP> Stations;
    public string Category;
}
