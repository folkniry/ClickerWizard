using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class UpgradeAutoClickButton : MonoBehaviour
{
     [SerializeField] private ClickerManager clickerManager;
   
    [SerializeField] private int clickBonus; 
    [SerializeField] private int saveIndex;
    [SerializeField] private int baseCost;
    [SerializeField] private Button currentButton;
    [SerializeField] private Text costText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text clickBonusText;
    [SerializeField] private GameObject closeContainer;
    [SerializeField] private Text textOpen;
    [SerializeField] private Image imageSlider;
   // [SerializeField] private AudioClip buyAudioClip;
   // [SerializeField] private AudioClip openAudioClip;
    private int currentLevelUpgrade = 0;
    private bool activeButton = false;
    private int cost;
    //private AudioSource audioSource;
    private void Start()
    {
       // audioSource = gameObject.AddComponent<AudioSource>();
       // audioSource.volume = 0.6f;

        currentButton.onClick.AddListener(BuyClick);
        UpdateText();
    }

    private void Update()
    {
        if (activeButton == false)
        {
            if (clickerManager.GetCurrentMoney >= baseCost)
            {
                //PlayerPrefs.SetInt("ActiveButton" + saveIndex, 1);
                YandexGame.savesData.ActiveButtonAuto[saveIndex] = true;
                AudioManager.instance.tapSfx.Play();
                // audioSource.clip = openAudioClip;
                //audioSource.Play();
                UpdateText();
            }
            
        }
        if (clickerManager.GetCurrentMoney < cost &&  activeButton==true)
        {
            if(!imageSlider.gameObject.activeSelf)
                imageSlider.gameObject.SetActive(true);
            
            imageSlider.fillAmount = (float)clickerManager.GetCurrentMoney / cost;
            currentButton.interactable = false;
        }
        else
        {
            if(imageSlider.gameObject.activeSelf)
                imageSlider.gameObject.SetActive(false);
            
            currentButton.interactable = true;
        }

       
        
    }

    private void UpdateText()
    {
        // activeButton = PlayerPrefs.GetInt("ActiveButton" + saveIndex, activeButton);
        activeButton = YandexGame.savesData.ActiveButtonAuto[saveIndex];

        if (activeButton == true)
        {
            closeContainer.gameObject.SetActive(false);
        }
        else
        {
            closeContainer.gameObject.SetActive(true);
        }

        textOpen.text = baseCost.ToString();
        currentLevelUpgrade = YandexGame.savesData.LevelUpgradeAuto[saveIndex];
        //currentLevelUpgrade =  PlayerPrefs.GetInt("saveIndex" + saveIndex, currentLevelUpgrade);
        cost = (int)Mathf.Round(baseCost * Mathf.Pow(1.5f, currentLevelUpgrade));
        
        
        costText.text = ShortScaleString.parseInt(cost,1,1000,true).ToString();
        levelText.text = (currentLevelUpgrade +1).ToString();
        clickBonusText.text = "$+" + clickBonus.ToString();
    }
    private void BuyClick()
    {
        if (clickerManager.GetCurrentMoney >= cost)
        {
            clickerManager.AddMoney(-cost);
            currentLevelUpgrade += 1;
            clickerManager.AddAutoClick(clickBonus);
            cost = (int)Mathf.Round(baseCost * Mathf.Pow(1.5f, currentLevelUpgrade));
            YandexGame.savesData.LevelUpgradeAuto[saveIndex] = currentLevelUpgrade;
            YandexGame.savesData.CostAuto[saveIndex] = cost;
            //PlayerPrefs.SetInt("saveIndex" + saveIndex, currentLevelUpgrade);
            //PlayerPrefs.SetInt("cost" + saveIndex, cost);
            // audioSource.clip = buyAudioClip;
            //audioSource.Play();
            AudioManager.instance.tapSfx.Play();
            UpdateText();
            SendUpgrade(saveIndex.ToString());
        }
    }
    


    public void SendUpgrade(string rew)
    {
        var eventParams2 = new Dictionary<string, string>
                {
                    { "UpgradeAuto", rew }
                };

        YandexMetrica.Send("UpgradeAuto", eventParams2);
    }
}