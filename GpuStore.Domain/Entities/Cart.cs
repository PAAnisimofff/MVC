using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GpuStore.Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();
        public void AddItem(Card card, int quantity)
        {
            CartLine line = lineCollection.Where(g => g.Card.CardId == card.CardId).FirstOrDefault();
            if (line == null)
            {
                lineCollection.Add(new CartLine
                {
                    Card = card,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Card card)
        {
            lineCollection.RemoveAll(l => l.Card.CardId == card.CardId);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Card.Price * e.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }
        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }
    public class CartLine
    {
        public Card Card { get; set; }
        public int Quantity { get; set; }
    }
}
