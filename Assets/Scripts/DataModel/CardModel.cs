using System;

namespace DataModel
{
    [Serializable]
    public class CardModel
    {
        public string CardID { get; set; }
        public bool Revealed { get; set; }

        public CardModel()
        {
        
        }

        public CardModel(string cardId, bool revealed)
        {
            CardID = cardId;
            Revealed = revealed;
        }
    }
}
