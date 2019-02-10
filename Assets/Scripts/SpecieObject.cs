using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Gallery
{
    public string PhotoId;
    public int Id;
    public string Description;
}

[System.Serializable]
public class SpecieObject {

    public string SpecieId;
    public int Id;
    public string Name;
    public string Family;
    public List<Gallery> Gallery;
}
