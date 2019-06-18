using UnityEngine;
using System.Collections;

namespace ArbolClass {

    [System.Serializable]
    public class Arbol{
        public string SpecieId;
        public int Id;
        public string Name;
        public string Family;
        public Gallery[] Gallery;
        public string audioSpecie;
    }

    [System.Serializable]
    public class Gallery{
        public string PhotoId;
        public int Id;
        public string Description;
        public string audioname;
        public string imagename;
    }
}