using System;
using UnityEngine;
using UnityEngine.UI;

public class ClickerManager : MonoBehaviour
{
    [SerializeField] private Button clickButton;
    [SerializeField] private Text textMoney;
    [SerializeField] private Text clickCountText;
    [SerializeField] private Text autoClickCountText;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private Transform pointSpawnEffect;
    private int currentMoney;
    private int amountOfMoneyPerClick = 1;
    private int amountOfMoneyPerAutoClick = 1;
    public int GetCurrentMoney => currentMoney;

    private float timerAutoClick = 1;
    private void Start()
    {
        amountOfMoneyPerAutoClick = PlayerPrefs.GetInt("AutoClick", 0);
        amountOfMoneyPerClick = PlayerPrefs.GetInt("Click", 1);
        currentMoney = PlayerPrefs.GetInt("Money", 0);
        clickButton.onClick.AddListener(Click);
        UpdateText();
    }

    private void UpdateText()
    {
        textMoney.text = currentMoney.ToString();
        clickCountText.text = amountOfMoneyPerClick.ToString();
        autoClickCountText.text = amountOfMoneyPerAutoClick.ToString();
    }

    private void Update()
    {
        if (amountOfMoneyPerAutoClick > 0)
        {
            if (timerAutoClick > 0)
            {
                timerAutoClick -= Time.deltaTime;
                
            }
            else
            {
                timerAutoClick = 1;
                AddMoney(amountOfMoneyPerAutoClick);
            }
        }
    }

    private void Click()
    {
        AddMoney(amountOfMoneyPerClick);
    }

    public void AddClick(int bonusClick)
    {
        amountOfMoneyPerClick += bonusClick;
        PlayerPrefs.SetInt("Click", amountOfMoneyPerClick);
        UpdateText();
    }

    public void AddAutoClick(int autoClick)
    {
        amountOfMoneyPerAutoClick += autoClick;
        PlayerPrefs.SetInt("AutoClick", amountOfMoneyPerAutoClick);
        UpdateText();
    }
    public void AddMoney(int count)
    {
        Instantiate(effect, pointSpawnEffect.transform);
        currentMoney += count;
        PlayerPrefs.SetInt("Money", currentMoney);
        UpdateText();
    }
    
}