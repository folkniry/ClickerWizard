using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GifAnimate : MonoBehaviour
{

    // Start is called before the first frame update
    public Texture2D[] frame;
    private float framePerSecond = 0f;

    private RawImage image = null;
    private Renderer render = null;

    void Awake()
    {
        image = GetComponent<RawImage>();
        render = GetComponent<Renderer>();
    }

    public void startGif()
    {
        framePerSecond = 15f;
    }

    public void startGif2()
    {
        framePerSecond = 10f;
    }

    public void stopGif()
    {
        framePerSecond = 0f;
    }

    void Update()
    {
        float index = Time.time * framePerSecond;
        index = index % frame.Length;

        if (render != null)
        {
            render.material.mainTexture = frame[(int)index];
        }
        else
        {
            image.texture = frame[(int)index];
        }
    }
   

     
}
