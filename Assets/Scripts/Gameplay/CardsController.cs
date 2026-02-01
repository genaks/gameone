using System.Collections;
using System.Collections.Generic;
using Core.Config;
using Core.Services;
using Scriptables;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class CardsController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FlexibleGridLayout gridLayout;
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip cardSelectAudioClip;
        [SerializeField] private AudioClip failAudioClip;
        [SerializeField] private AudioClip successAudioClip;
        [SerializeField] private float cardPreviewDelay = 0.5f;
    
        private MessageBroker _messageBroker;
        private FileService _fileService;
        private LevelData _levelData;
        private CardShuffler _cardShuffler;
    
        private List<CardView> _cards = new List<CardView>();
        private int _firstCardIndex = -1;
        private string _firstCardID = "";
    
        private void Start()
        {
            _cardShuffler = new CardShuffler();
            
            if (ServiceLocator.Instance.TryGet(out MessageBroker messageBroker))
            {
                _messageBroker = messageBroker;
            }
            
            if (ServiceLocator.Instance.TryGet(out FileService fileService))
            {
                _fileService = fileService;
                _levelData = _fileService.ReadFromFile<LevelData>(Constants.Filenames.CurrentLevel);
                PopulateCards(_levelData);
            }
        }
    
        private void PopulateCards(LevelData levelData)
        {
            // Clear existing children
            ClearGrid();
            int numberOfElements = levelData.numberOfRows * levelData.numberOfColumns;
            levelData.sprites.AddRange(levelData.sprites);
            string[] shuffledCards = _cardShuffler.ShuffleCards(levelData.sprites.ToArray());
            
            // Create new elements
            for (int i = 0; i < numberOfElements; i++)
            {
                CardView card = Instantiate(cardPrefab, gridLayout.transform);
                card.SetData(shuffledCards[i], i);
                card.gameObject.name = Constants.ObjectNames.CardPrefix +  $"_{i}";
                card.OnCardSelected += RegisterCardSelection;
                _cards.Add(card);
            }
        
            // Update the grid layout
            gridLayout.SetColumns(levelData.numberOfColumns);
            gridLayout.SetRows(levelData.numberOfRows);
            gridLayout.UpdateGrid();
        }
    
    
        private void RegisterCardSelection(string cardID, int index)
        {
            audioSource.PlayOneShot(cardSelectAudioClip);
            if (_firstCardIndex == -1)
            {
                _firstCardIndex = index;
                _firstCardID = cardID;
            }
            else if (index != _firstCardIndex)
            {
                if (_firstCardID == cardID)
                {
                    audioSource.PlayOneShot(successAudioClip);
                    StartCoroutine(HideCardsWithDelay(_firstCardIndex, index));
                    ResetStoredCards();
                }
                else
                {
                    audioSource.PlayOneShot(failAudioClip);
                    StartCoroutine(ResetCardsWithDelay(_firstCardIndex, index));
                    ResetStoredCards();
                }
            }
        }

        private void ResetStoredCards()
        {
            _firstCardIndex = -1;
            _firstCardID = "";
        }
    
        private IEnumerator HideCardsWithDelay(int firstCardIndex, int secondCardIndex)
        {
            yield return new WaitForSeconds(cardPreviewDelay);
            HideCards(firstCardIndex, secondCardIndex);
        }

        private void HideCards(int firstCardIndex, int secondCardIndex)
        {
            _cards[firstCardIndex].SetRevealed();
            _cards[secondCardIndex].SetRevealed();
        }
        
        private IEnumerator ResetCardsWithDelay(int firstCardIndex, int secondCardIndex)
        {
            yield return new WaitForSeconds(cardPreviewDelay);
            ResetCards(firstCardIndex, secondCardIndex);
        }

        private void ResetCards(int firstCardIndex, int secondCardIndex)
        {
            _cards[firstCardIndex].Reset();
            _cards[secondCardIndex].Reset();
        }
    
        private void ClearGrid()
        {
            for (int i = gridLayout.transform.childCount - 1; i >= 0; i--)
            {
                if (Application.isPlaying)
                {
                    Destroy(gridLayout.transform.GetChild(i).gameObject);
                }
                else
                {
                    DestroyImmediate(gridLayout.transform.GetChild(i).gameObject);
                }
            }
        }
    }
}
