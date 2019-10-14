using UnityEngine;
using System;
using System.Collections.Generic;

public class SerialScript
{

    private string serial;
    private int defaultLength = 10;

    public string GenerateSerial(List<string> keys)
    {
        return GenerateSerial(keys, defaultLength);
    }

    public string GenerateSerial(List<string> keys, int len)
    {
        serial = Guid.NewGuid().ToString("N").Substring(0, len);
        if (keys.Contains(serial)) { //If duplicate, recalculate UID
            return GenerateSerial(keys, len);
        }
        return serial;
    }

    public string GetSerial()
    {
        return serial;
    }

    public void SetSerial(string s)
    {
        serial = s;
    }
}
