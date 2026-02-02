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
        private string _cardID;
        private int _index;

        public bool Revealed => _revealed;
        public string CardID => _cardID;
        
        public delegate void CardSelectedDelegate(string cardID, int index);
        public CardSelectedDelegate OnCardSelected;
        
        public void SetData(string sprite, int index, bool revealed)
        {
            image.sprite = Resources.Load<Sprite>(Constants.Filenames.Textures + sprite);
            _cardID = sprite;
            _index = index;
            _revealed = revealed;
            if (_revealed)
            {
                gameObject.SetActive(false);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            image.gameObject.SetActive(true);
            OnCardSelected.Invoke(_cardID, _index);
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
