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

    public void Set_Wrong_Chars(double x)
    {
        this.wrong_chars = x;
    }

    public double Get_Wrong_Chars()
    {
        return wrong_chars;
    }

    public void Set_WPM(double x)
    {
        this.WPM = x;
    }

    public double Get_WPM()
    {
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


    private double calculate_avg()
    { 
        accuracy = ((total_chars - wrong_chars) / total_chars) * 100;
        return accuracy;
    }

    public string Print_String()
    {
        calculate_avg();
        string output = Math.Round(accuracy,2) + "%";
        return output;

    }

    public string Print_WPM()
    {
        calculate_WPM();
        string output = Math.Round(WPM,2) + "";
        return output;

    }

    private double calculate_WPM()
    {
        WPM = corr_words / (currentTime / 60);
        return WPM;
    }


}
