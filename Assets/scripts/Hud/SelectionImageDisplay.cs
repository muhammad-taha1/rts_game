using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionImageDisplay : MonoBehaviour
{
    private UnityAction<string, int> selectEventListener;
    private UnityAction<int> deSelectEventListener;

    private Image image;


    private void Awake()
    {
        selectEventListener = new UnityAction<string, int>(showSelectionImage);
        deSelectEventListener = new UnityAction<int>(clearImage);
        image = GetComponent<Image>();
    }

    private void clearImage(int arg0)
    {
        image.sprite = null;
        image.color = Color.black;
    }

    private void showSelectionImage(string imagePath, int id)
    {
        image.color = Color.white;
        var tex = Resources.Load<Texture2D>(imagePath);
        var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        image.sprite = sprite;

    }

    void Start()
    {
        EventManager.StartListening<SelectEvent, string, int>(selectEventListener);
        EventManager.StartListening<DeSelectEvent, int>(deSelectEventListener);
    }

    private void OnDestroy()
    {
        EventManager.StopListening<SelectEvent, string, int>(selectEventListener);
        EventManager.StopListening<DeSelectEvent, int>(deSelectEventListener);
    }
}
