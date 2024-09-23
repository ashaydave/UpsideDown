using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Button button;
    public Sprite hoverSprite;
    private Sprite defaultSprite;


    private void Start()
    {
        button = GetComponent<Button>();
        defaultSprite = button.image.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // AudioManager.PlaySound2D(AudioManager.SoundClips.ButtonHover, 1f, 1f, 0f);
        button.image.sprite = hoverSprite;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.sprite = defaultSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // AudioManager.PlaySound2D(AudioManager.SoundClips.ButtonClick, 1f, 1f, 0f);
    }
}
