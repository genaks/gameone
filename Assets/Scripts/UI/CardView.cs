using System;
using Core.Config;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public class CardView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image image;

        private bool _revealed = false;
        private string _spriteID;
        private int _index;
        
        public delegate void CardSelectedDelegate(string cardID, int index);
        public CardSelectedDelegate OnCardSelected;
        
        public void SetData(string sprite, int index)
        {
            image.sprite = Resources.Load<Sprite>(Constants.Filenames.Textures + sprite);
            _spriteID = sprite;
            _index = index;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            image.gameObject.SetActive(true);
            OnCardSelected.Invoke(_spriteID, _index);
        }

        public void SetRevealed()
        {
            _revealed = true;
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            image.gameObject.SetActive(false);
        }
    }
}
