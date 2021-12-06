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



    private void Awake()
    {
        NewPoem();
        ConvertToLower(workingWords);
        Debug.Log(workingWords.Count);
    }


    //preps new poem for user's word bank
    public void NewPoem()
    {
        // calls API to get new poem to use as user's word bank
        Poem p = APIHelper.GetNewPoem();

        // sets title var to poem's title
        title = p.title;

        // sets author var as poem's author
        author = p.author;

        // extracts poem's lines from list
        foreach (string arrItem in p.lines)
        {
            // checks if theyre not empty strings
            if (!arrItem.Equals("")){
                // appends them to poem var if not empty
                poem = poem + " " + arrItem;
            }
        }

        // strips poem of all symbols and punctuation
        poem = Regex.Replace(poem, @"[^\w\d\s]", "");

        // splits poem by word into list
        strlist = poem.Split(' ');

        // ensures no string is empty in list
        foreach(string word in strlist)
        {
            if (!word.Equals(""))
                originalWords.Add(word);
        }

        // add range equal to list size to other list
        workingWords.AddRange(originalWords.ToList());

        // shuffle the words in the list
        workingWords = Shuffle(workingWords);

        // remove all other words in list
        for (int i = workingWords.Count - 1; i > 25;  i--)
        {
            workingWords.RemoveAt(i);
        }

    }

    // shuffles words in list
    private List<string> Shuffle(List<string> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            // gets random number between i and list length
            int random = UnityEngine.Random.Range(i, list.Count);

            // holds i's current value as a temporary 
            string temporary = list[i];

            // changes i to another random word in the list
            list[i] = list[random];

            // changes the random word's index to i's original
            list[random] = temporary;
        }

        // returns shuffled list
        return list;
    }


    // converts all words in a list to lower case
    private void ConvertToLower(List<string> list)
    { 
        for(int i = 0; i < list.Count; i++)
        {
            // converts current word at i'th index of list to lowercase
            list[i] = list[i].ToLower();
        }
    }

    // gets a new word from the word bank
    public string GetWord()
    {
        // initialzes var for a new word
        string newWord = string.Empty;

        // ensures bank isnt empty 
        if(workingWords.Count != 0)
        {
            // gets the last word in the list to return
            newWord = workingWords.Last();

            // removes that word from list
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


    public int totalWords()
    {
        int total_words = workingWords.Count;
        return total_words;
    }
}