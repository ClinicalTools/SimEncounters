using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OtherTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var rand = new System.Random(2);
        var bytes = new byte[5];
        var encoding = new System.Text.UTF8Encoding();
        rand.NextBytes(bytes);
        var str = string.Concat(bytes.Select(b => b.ToString("X2")).ToArray());
        Debug.Log(str);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
