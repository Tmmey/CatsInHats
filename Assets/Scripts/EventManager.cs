using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Backgroud;

    [SerializeField]
    private List<Sprite> Images;
    // Start is called before the first frame update
    void Start()
    {
        //SetBackground();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetBackground()
    {
        var imageIndex = UnityEngine.Random.Range(0, Images.Count);
        var img = Backgroud.GetComponent<UnityEngine.UI.Image>();
        img.sprite = Images[imageIndex];

        var cam = Camera.main;


        //img.sprite.texture.Reinitialize(cam.pixelWidth, cam.pixelHeight);
        //img.sprite.texture.Reinitialize(128, 64);
        Debug.Log(Images[imageIndex].name);
    }
}
