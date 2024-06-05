using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour
{
    public ButtonDefinition buttonDefinition;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        showSelectionImage();
    }

    private void clearImage()
    {
        image.sprite = null;
        image.color = Color.black;
    }

    private void showSelectionImage()
    {
        image.color = Color.white;
        var tex = Resources.Load<Texture2D>(buttonDefinition.imagePath);
        var sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        image.sprite = sprite;

    }

    public void ClickHandler()
    {
        buttonDefinition.action();
    }
}
