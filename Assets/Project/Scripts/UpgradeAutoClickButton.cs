using UnityEngine;
using UnityEngine.UI;

public class UpgradeAutoClickButton : MonoBehaviour
{
     [SerializeField] private ClickerManager clickerManager;
   
    [SerializeField] private int clickBonus; 
    [SerializeField] private string saveIndex;
    [SerializeField] private int baseCost;
    [SerializeField] private Button currentButton;
    [SerializeField] private Text costText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text clickBonusText;
    [SerializeField] private GameObject closeContainer;
    [SerializeField] private Text textOpen;
    [SerializeField] private Image imageSlider;
    [SerializeField] private AudioClip buyAudioClip;
    [SerializeField] private AudioClip openAudioClip;
    private int currentLevelUpgrade = 0;
    private int activeButton = 0;
    private int cost;
    private AudioSource audioSource;
    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.6f;

        currentButton.onClick.AddListener(BuyClick);
        UpdateText();
    }

    private void Update()
    {
        if (activeButton == 0)
        {
            if (clickerManager.GetCurrentMoney >= baseCost)
            {
                PlayerPrefs.SetInt("ActiveButton" + saveIndex, 1);
                audioSource.clip = openAudioClip;
                audioSource.Play();
                UpdateText();
            }
            
        }
        if (clickerManager.GetCurrentMoney < cost &&  activeButton==1)
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
        activeButton = PlayerPrefs.GetInt("ActiveButton" + saveIndex, activeButton);

        if (activeButton == 1)
        {
            closeContainer.gameObject.SetActive(false);
        }
        else
        {
            closeContainer.gameObject.SetActive(true);
        }

        textOpen.text = baseCost.ToString();
        currentLevelUpgrade =  PlayerPrefs.GetInt("saveIndex" + saveIndex, currentLevelUpgrade);
        cost = (int)Mathf.Round(baseCost * Mathf.Pow(1.5f, currentLevelUpgrade));
        
        
        costText.text = "Cost " + cost.ToString();
        levelText.text = "Level " + (currentLevelUpgrade +1).ToString();
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
            PlayerPrefs.SetInt("saveIndex" + saveIndex, currentLevelUpgrade);
            PlayerPrefs.SetInt("cost" + saveIndex, cost);
            audioSource.clip = buyAudioClip;
            audioSource.Play();
            UpdateText();
        }
    }
}