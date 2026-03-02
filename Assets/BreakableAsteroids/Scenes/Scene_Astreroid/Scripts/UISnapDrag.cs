using UnityEngine;
using UnityEngine.EventSystems;


public class UISnapDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
   public Transform snapTarget;



   private _canvas       _canvas;
   private Vector2       _startPosition;
   private RectTransform _rectTransform;



   public void OnBeginDrag(PointerEventData eventData)
   {
       _startPosition = _rectTransform.anchoredPosition;
   }


   public void OnDrag(PointerEventData eventData)
   {
       _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
   }


   public void OnEndDrag(PointerEventData eventData)
   {
       if (Vector2.Distance(_rectTransform.position, snapTarget.position) < 50f)
	   {
		   _rectTransform.position = snapTarget.position; // Snap
	   }
       else
	   {
		   _rectTransform.anchoredPosition = _startPosition; // Reset
	   }
   }



   private void Awake()
   {
       _canvas        = GetComponentInParent<Canvas>();
       _rectTransform = GetComponent<RectTransform>();
   }


}