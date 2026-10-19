using System.Collections.Generic;
using UnityEngine;

public class RoundModel
{
    public int DiceCount { get; private set; } = 5;
    public int LoseMin { get; private set; }
    public int DrawMin { get; private set; }
    public int WinMin { get; private set; }
    public int Sum { get; private set; }
    public string ValuesText { get; private set; } = "—";
    public string Outcome { get; private set; } = "—";

    public void SetDiceCount(int count)
    {
        DiceCount = Mathf.Max(1, count);
    }

    public void SetLose(int value)
    {
        LoseMin = value;
    }

    public void SetDraw(int value)
    {
        DrawMin = value;
    }

    public void SetWin(int value)
    {
        WinMin = value;
    }

    public void RandomizeThresholds()
    {
        LoseMin = Random.Range(0, 5);
        DrawMin = LoseMin + Random.Range(4, 10);
        WinMin = DrawMin + Random.Range(4, 10);
    }

    public void ResetRoll()
    {
        ValuesText = "—";
        Sum = 0;
        Outcome = "—";
    }

    public void ApplyValues(List<int> values)
    {
        Sum = 0;
        ValuesText = "";
        for (int i = 0; i < values.Count; i++)
        {
            Sum += values[i];
            ValuesText += values[i];
            if (i < values.Count - 1)
                ValuesText += ", ";
        }

        if (Sum >= WinMin)
            Outcome = "Победа!";
        else if (Sum >= DrawMin)
            Outcome = "Ничья";
        else
            Outcome = "Поражение";
    }
}