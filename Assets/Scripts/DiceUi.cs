using TMPro;
using UnityEngine;

public class DiceUi : MonoBehaviour
{
    [SerializeField] private TMP_Text rolledText;
    [SerializeField] private TMP_Text sumText;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private TMP_Text loseText;
    [SerializeField] private TMP_Text drawText;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private TMP_Text outcomeText;

    public void Apply(RoundModel model)
    {
        if (rolledText != null)
            rolledText.text = "Выпало: " + model.ValuesText;
        if (sumText != null)
            sumText.text = model.ValuesText == "—" ? "Сумма: —" : "Сумма: " + model.Sum;
        if (countText != null)
            countText.text = "Кубиков: " + model.DiceCount;
        if (loseText != null)
            loseText.text = "Поражение: от " + model.LoseMin;
        if (drawText != null)
            drawText.text = "Ничья: от " + model.DrawMin;
        if (winText != null)
            winText.text = "Победа: от " + model.WinMin;
        if (outcomeText != null)
            outcomeText.text = model.Outcome;
    }
}