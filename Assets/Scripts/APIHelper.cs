using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;


public static class APIHelper
{
    public static Poem GetNewPoem()
    {
        // API URL
        string url = "https://poetrydb.org/random";

        // API Request
        HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);

        // Acceptable API Return
        httpRequest.Accept = "application/json";

        // API Response
        HttpWebResponse httpResponse = (HttpWebResponse)httpRequest.GetResponse();

        // Response Reader
        StreamReader reader = new StreamReader(httpResponse.GetResponseStream());

        // Response as a string
        string result = reader.ReadToEnd();

        // Removes extra parenthesis on response
        result = result.Remove(0, 1);
        result = result.Remove(result.Length - 1);

        // Serializes response into object
        return JsonUtility.FromJson<Poem>(result);

    }
}
