using TMPro;
using UnityEngine;

public class HeaderUI : MonoBehaviour
{
    public TMP_Text MuffinAmountText;

    private GameManager gameManager;

    public void Start()
    {
        gameManager = GameManager.Instance;
        SetTexts();
    }

    public void Update()
    {
        SetTexts();
    }

    private void SetTexts()
    {
        MuffinAmountText.text = $"{gameManager.MuffinAmount} muffins";
    }
}