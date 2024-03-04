using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class InteractiveButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Vector2 initialScale, finalScale;
    public float moveTime;

    public AudioClip[] selectSounds, interactSounds;

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(finalScale, moveTime);
        if (selectSounds.Length > 0) AudioManager.instance.PlayClipAt(0, Vector2.zero, selectSounds[Random.Range(0, selectSounds.Length)]);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(initialScale, moveTime);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactSounds.Length > 0) AudioManager.instance.PlayClipAt(0, Vector2.zero, interactSounds[Random.Range(0, interactSounds.Length)]);
    }
}