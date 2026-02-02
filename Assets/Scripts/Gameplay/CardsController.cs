using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core.Config;
using Core.MessageBroker;
using Core.Services;
using Core.Services.FileSystem;
using DataModel;
using UI;
using UnityEngine;

namespace Gameplay
{
    public class CardsController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private FlexibleGridLayout gridLayout;
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private ScoreController scoreController;
        [SerializeField] private AudioSource audioSource;
        
        [SerializeField] private AudioClip cardSelectAudioClip;
        [SerializeField] private AudioClip failAudioClip;
        [SerializeField] private AudioClip successAudioClip;
        [SerializeField] private float cardPreviewDelay = 0.5f;

        private MessageBroker _messageBroker;
        private FileService _fileService;
        private LevelData _levelData;
        private CardShuffler _cardShuffler;
    
        private readonly List<CardView> _cards = new();
        private int _firstCardIndex = -1;
        private string _firstCardID = "";
        private int _unrevealedCards = 0;
        
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
            int numberOfElements = levelData.NumberOfRows * levelData.NumberOfColumns;
            List<string> sprites = new List<string>();
            foreach (var card in levelData.Cards)
            {
                sprites.Add(card.CardID);
            }
            sprites.AddRange(sprites);
            string[] shuffledCards = _cardShuffler.ShuffleCards(sprites.ToArray());
            _unrevealedCards = numberOfElements;
            
            // Create new elements
            for (int i = 0; i < numberOfElements; i++)
            {
                CardView card = Instantiate(cardPrefab, gridLayout.transform);
                card.SetData(shuffledCards[i], i, false);
                card.gameObject.name = Constants.ObjectNames.CardPrefix +  $"_{i}";
                card.OnCardSelected += RegisterCardSelection;
                _cards.Add(card);
            }
        
            // Update the grid layout
            gridLayout.SetColumns(levelData.NumberOfColumns);
            gridLayout.SetRows(levelData.NumberOfRows);
            gridLayout.UpdateGrid();
        }
    
    
        private void RegisterCardSelection(string cardID, int index)
        {
            if (_firstCardIndex == -1)
            {
                scoreController.OnCardSelected();
                audioSource.PlayOneShot(cardSelectAudioClip);
                _firstCardIndex = index;
                _firstCardID = cardID;
            }
            else if (index != _firstCardIndex)
            {
                scoreController.OnCardSelected();
                if (_firstCardID == cardID) //card id is same as the previously selected card
                {
                    OnMatchingCardsSelected(index);
                }
                else
                {
                    OnDifferentCardsSelected(index);
                }
            }
        }

        private void OnMatchingCardsSelected(int index)
        {
            _unrevealedCards -= 2;
            audioSource.PlayOneShot(successAudioClip);
            scoreController.OnMatchingCardsSelected();
            StartCoroutine(HideCardsWithDelay(_firstCardIndex, index));
            ResetStoredCards();
        }
        
        private void OnDifferentCardsSelected(int index)
        {
            audioSource.PlayOneShot(failAudioClip);
            scoreController.OnDifferentCardsSelected();
            StartCoroutine(ResetCardsWithDelay(_firstCardIndex, index));
            ResetStoredCards();
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

            if (_unrevealedCards == 0)
            {
                _messageBroker.Publish(new CardGridExhaustedEvent());
            }
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

        private void OnApplicationQuit()
        {
            SaveCurrentCardData();
        }

        private void SaveCurrentCardData()
        {
            LevelData levelData = new LevelData(_levelData, GetCurrentCards(), true, scoreController.Turns, scoreController.Score);
            _fileService.WriteToFile(levelData, Constants.Filenames.CurrentLevel);
        }

        private Dictionary<string, bool> GetCurrentCards()
        {
            Dictionary<string, bool> cardsDictionary = new();
            foreach (var card in _cards)
            {
                cardsDictionary[card.CardID] = card.Revealed;
            }
            
            return cardsDictionary;
        }
    }
}
