using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using Photon.Pun;

public class WordBank : MonoBehaviourPun
{

    private List<string> originalWords = new List<string>()
    {
        "Pancakes", "Are", "Delicious"
    };

    private List<string> workingWords = new List<string>();
    
    private void Awake()
    {
        workingWords.AddRange(originalWords);
        Shuffle(workingWords);
        ConvertToLower(workingWords);
        
    }

    private void Shuffle(List<string> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int random = Random.Range(i, list.Count);
            string temporary = list[i];

            list[i] = list[random];
            list[random] = temporary;
        }
    }

    private void ConvertToLower(List<string> list)
    { 
        for(int i = 0; i < list.Count; i++)
        {
            list[i] = list[i].ToLower();
        }
    }

    public string GetWord()
    {
        string newWord = string.Empty;

        if(workingWords.Count != 0)
        {
            newWord = workingWords.Last();
            workingWords.Remove(newWord);
        }
        return newWord;
    }

    public int calculateHealth()
    {
        int maxHealth = originalWords.Count;
        return maxHealth;
    }
}