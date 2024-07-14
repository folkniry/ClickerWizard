using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using YG;
using UnityEngine.SocialPlatforms.Impl;


public class ClickerManager : MonoBehaviour
{
    [SerializeField] private Button clickButton;
    [SerializeField] private Text textMoney;
    [SerializeField] private Text clickCountText;
    [SerializeField] private Text autoClickCountText;
    [SerializeField] private ParticleSystem effect;
    [SerializeField] private Transform pointSpawnEffect;
    [SerializeField] private Transform wizard;
    private int SecCounter = 2;
    private float currentMoney;
    private int amountOfMoneyPerClick = 1;
    private int amountOfMoneyPerAutoClick = 1;
    public float GetCurrentMoney => currentMoney;

    private float timerAutoClick = 1;

    private float timerAdsMax = 100f;

    private float timerAds = 70f;

    private float timerMusic = 0f;

    private bool MusicOn = false;

    [SerializeField] public GameObject pauseUI;
    [SerializeField] public Text pauseText;
    //[SerializeField] private AudioClip audioClip;
    private float speed = 50.0f; 
    public float amount = 0f; 

    private void OnEnable() => YandexGame.GetDataEvent += LoadData;
    private void OnDisable() => YandexGame.GetDataEvent -= LoadData;
    private void Start()
    {
        //audioSource = gameObject.AddComponent<AudioSource>();
       // audioSource.volume = 1f;
        clickButton.onClick.AddListener(Click);     
    }
    private void LoadData()
    {
        amountOfMoneyPerAutoClick = YandexGame.savesData.amountOfMoneyPerAutoClick;
        amountOfMoneyPerClick = YandexGame.savesData.amountOfMoneyPerClick;
        currentMoney = YandexGame.savesData.currentMoney;
        if (YandexGame.savesData.music){
            AudioManager.instance.ToogleMusic(true);
        }
        else
        {
            AudioManager.instance.ToogleMusic(false);
        }
        UpdateText();
    }   

    private void UpdateText()
    {
        textMoney.text = ShortScaleString.parseFloat(currentMoney,1,1000,true).ToString();
        clickCountText.text = ShortScaleString.parseFloat(amountOfMoneyPerClick, 1, 1000, true).ToString();
        autoClickCountText.text = ShortScaleString.parseFloat(amountOfMoneyPerAutoClick, 1, 1000, true).ToString();
    }


    public void PauseGame(bool timer = false)
    {
        if (timer)
        {
            if (YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
            {
                Time.timeScale = 0f;
                AudioListener.pause = true;
                YandexMetrica.Send("FullView");
                pauseUI.SetActive(true);
                SecCounter++;
                StartCoroutine(TimerAds());
            }
        }
        else
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
        }
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        pauseUI.SetActive(false);
    }
    IEnumerator TimerAds()
    {
        while (true)
        {
            if (SecCounter > 1)
            {
                SecCounter--;                
                pauseText.text = SecCounter.ToString();
                yield return new WaitForSecondsRealtime(1.0f);
            }
            if (SecCounter == 1)
            {                
                SecCounter = 2;
                ResumeGame();
                YandexGame.FullscreenShow();
                while (!YandexGame.nowFullAd)
                {
                    yield return null;
                }
                yield break;
            }
        }
    }

    public void clickWizard()
    {
        amount = 0.01f;
    }

    private void Update()
    {                
        if (amount > 0){            
            wizard.position = new Vector3(Mathf.Sin(Time.time * speed) * amount, wizard.position.y, wizard.position.z);
            //print(Mathf.Sin(Time.time * speed) * amount);
            amount -= Time.deltaTime * 0.05f;
        }
        else
        {
            wizard.position = new Vector3(0, wizard.position.y, wizard.position.z);
            amount = 0f;
        }
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
        //реклама
        if (timerAds>0)
        {
            timerAds -= Time.deltaTime;
            //print(timerAds);
        }
        else
        {
            int score_ = 0;
            for (int i = 0; i < 5;i++)
            {
                score_=(YandexGame.savesData.LevelUpgradeAuto[i]+YandexGame.savesData.LevelUpgradeClick[i])*10;
            }
            YandexGame.NewLeaderboardScores("Bestwizard", score_);
            timerAds = UnityEngine.Random.Range(60, timerAdsMax);
            PauseGame(true);
        }


        //музыка
        if (timerMusic > 0)
        {
            timerMusic -= Time.deltaTime;
            //print(timerMusic);
            if (timerMusic > 1f&& !MusicOn)
            {
                MusicOn = true;
                if (!AudioManager.instance.MusicSfx.isPlaying){
                    wizard.GetComponent<GifAnimate>().startGif();
                    AudioManager.instance.MusicSfx.Play();
                }
            }
        }
        else
        {
            AudioManager.instance.MusicSfx.Stop();
            wizard.GetComponent<GifAnimate>().stopGif();
            MusicOn = false;
            timerMusic = 0;            
        }

        
    }

    private void Click()
    {
        if (!YandexGame.savesData.FirstClick)
        {
            YandexMetrica.Send("StartGame");
            YandexGame.savesData.FirstClick = true;
        }
        timerMusic += 0.25f;
        AddMoney(amountOfMoneyPerClick);
    }

    public void AddClick(int bonusClick)
    {
        amountOfMoneyPerClick += bonusClick;
        YandexGame.savesData.amountOfMoneyPerClick = amountOfMoneyPerClick;
        //PlayerPrefs.SetInt("Click", amountOfMoneyPerClick);
        UpdateText();
    }

    public void AddAutoClick(int autoClick)
    {
        amountOfMoneyPerAutoClick += autoClick;
        YandexGame.savesData.amountOfMoneyPerAutoClick = amountOfMoneyPerAutoClick;
        //PlayerPrefs.SetInt("AutoClick", amountOfMoneyPerAutoClick);
        UpdateText();
    }
    public void AddMoney(float count)
    {
        Instantiate(effect, pointSpawnEffect.transform);
        currentMoney += count;
        YandexGame.savesData.currentMoney = currentMoney;
        YandexGame.SaveProgress();
       // PlayerPrefs.SetInt("Money", currentMoney);
        UpdateText();
    }
    
}