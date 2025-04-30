using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PrimeTween;

public class Cards : MonoBehaviour
{
    [SerializeField] private Image runeImage;
    [SerializeField] private Image backgroundImage;

    public Sprite hiddenRuneSprite;
    public Sprite RuneSprite;

    public bool isSelected;

    public CardController controller;

    private Color originalColor;

    private void Awake()
    {
        if (backgroundImage != null)
        {
            originalColor = backgroundImage.color;
        }
    }

    public void OnCardClick()
    {
        controller.SetSelected(this);
    }

    public void SetRuneSprite(Sprite sp)
    {
        RuneSprite = sp;
    }

    public void Show()
    {
        Tween.Rotation(transform, new Vector3(0f, 180f, 0f), 0.2f);

        Tween.Delay(0.1f, () => runeImage.sprite = RuneSprite);

        isSelected = true;

        backgroundImage.color = new Color32(0x6B, 0xA2, 0x9F, 0xFF);
    }

    public void Hide()
    {
        Tween.Rotation(transform, new Vector3(0f, 0f, 0f), 0.2f);

        Tween.Delay(0.1f, () =>
        {
            runeImage.sprite = hiddenRuneSprite;
            backgroundImage.color = originalColor;
            isSelected = false;
        });
    }
}
