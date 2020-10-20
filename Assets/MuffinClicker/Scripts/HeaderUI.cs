using TMPro;
using UnityEngine;

public class HeaderUI : MonoBehaviour
{
    public TMP_Text MuffinAmountText;
    public GameManager Manager;

    public void Start()
    {
        SetTexts();
    }

    public void Update()
    {
        SetTexts();
    }

    private void SetTexts()
    {
        MuffinAmountText.text = $"{Manager.MuffinAmount} muffins";
    }
}