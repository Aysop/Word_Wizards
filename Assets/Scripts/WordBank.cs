using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Text.RegularExpressions;

public class WordBank : MonoBehaviourPun
{
    private HashSet<string> originalWords = new HashSet<string>();
    private List<string> workingWords = new List<string>();
    private string poem = "";
    private string title = "";
    private string author = "";
    string[] strlist;

    public void NewPoem()
    {
        Poem p = APIHelper.GetNewPoem();
        title = p.title;
        author = p.author;
        foreach (string arrItem in p.lines)
        {
            if (!arrItem.Equals("")){
                //originalWords.Add(arrItem);
                poem = poem + " " + arrItem;
            }
        }

        poem = Regex.Replace(poem, @"[^\w\d\s]", "");
        strlist = poem.Split(' ');

        foreach(string word in strlist)
        {
            //UnityEngine.Debug.Log(word);
            if (!word.Equals(""))
                originalWords.Add(word);
        }

        workingWords.AddRange(originalWords.ToList());
        workingWords = Shuffle(workingWords);
        for (int i = workingWords.Count - 1; i > 10;  i--)
        {
            workingWords.RemoveAt(i);
        }

    }

    
    private void Awake()
    {
        NewPoem();
        ConvertToLower(workingWords);
        Debug.Log(workingWords.Count);
    }

    private List<string> Shuffle(List<string> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int random = UnityEngine.Random.Range(i, list.Count);
            string temporary = list[i];

            list[i] = list[random];
            list[random] = temporary;
        }
        return list;
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

    public string GetTitle()
    {
        return title;
    }
    public string GetAuthor()
    {
        return author;
    }


    public int calculateHealth()
    {
        int maxHealth = workingWords.Count;
        return maxHealth;
    }
}