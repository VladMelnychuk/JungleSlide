using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static void GenerateLevel(string levelName, DictionaryObjectPool objectpool)
    {
        var o1 = JObject.Parse(File.ReadAllText("Assests/Levels/" + levelName));
        
        print(o1);
    }
}
