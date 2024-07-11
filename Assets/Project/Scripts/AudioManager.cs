using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using YG;


public class AudioManager : MonoBehaviour
{
    public AudioSource tapSfx;

    public AudioSource MusicSfx;

    public static AudioManager instance;

  

    // Start is called before the first frame update
    private void Awake()
    {        
        if (YandexGame.savesData.music == true)
        {
            ToogleMusic(true);          
        }
        else
        {
            ToogleMusic(false);          
        }       

        if (FindObjectsOfType(typeof(AudioManager)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;


        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToogleMusic(bool toogle)
    {
        if (toogle)
        {           
            MusicSfx.volume = 1.0f;
            tapSfx.volume = 0.6f;
        }
        else {
           
            MusicSfx.volume = 0f;
            tapSfx.volume = 0f;
        }
        
    }
   
}
