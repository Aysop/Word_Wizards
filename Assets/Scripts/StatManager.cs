using System;
using UnityEngine;

public class StatManager : MonoBehaviour
{
    private float currentTime = 0f;
    private double accuracy = 100;
    private double total_chars = 0;
    private double wrong_chars = 0;
    private int corr_words = 0;
    private double WPM = 0;

    // returns double of user's typing accuracy
    private double calculate_accuracy()
    {
        accuracy = ((total_chars - wrong_chars) / total_chars) * 100;
        return accuracy;
    }

    // returns string with user's type accuracy
    public string Print_Accuracy()
    {
        calculate_accuracy();
        string output = Math.Round(accuracy, 2) + "%";
        return output;

    }

    // returns string with user's words per minute
    public string Print_WPM()
    {
        calculate_WPM();
        string output = Math.Round(WPM, 2) + "";
        return output;

    }

    // returns double of user's words per minute
    private double calculate_WPM()
    {
        WPM = (corr_words * 60) / currentTime;
        return WPM;
    }

    public void Set_Corr_Words(int x)
    {
        this.corr_words = x;
    }

    public int Get_Corr_Words()
    {
        return corr_words;
    }
    public void Set_Total_Chars(double x)
    {
        this.total_chars = x;
    }

    public double Get_Total_Chars()
    {
        return total_chars;
    }

    public void Set_Time(float x)
    {
        this.currentTime = x;
    }

    public float Get_Time()
    {
        return currentTime;
    }
    public void Set_WPM(double x)
    {
        this.WPM = x;
    }

    public double Get_WPM()
    {
        return WPM;
    }
    public void Set_Wrong_Chars(double x)
    {
        this.wrong_chars = x;
    }

    public double Get_Wrong_Chars()
    {
        return wrong_chars;
    }





}
