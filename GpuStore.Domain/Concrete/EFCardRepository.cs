using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpuStore.Domain.Entities;
using GpuStore.Domain.Abstract;

namespace GpuStore.Domain.Concrete
{
    public class EFCardRepository : ICardRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Card> Cards
        {
            get { return context.Cards; }
        }
        public void SaveCard(Card card)
        {
            if (card.CardId == 0)
                context.Cards.Add(card);
            else
            {
                Card dbEntry = context.Cards.Find(card.CardId);
                if (dbEntry != null)
                {
                    dbEntry.Name = card.Name;
                    dbEntry.Description = card.Description;
                    dbEntry.Price = card.Price;
                    dbEntry.Manufacturer = card.Manufacturer;
                    dbEntry.ImageData = card.ImageData;
                    dbEntry.ImageMimeType = card.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
        public Card DeleteCard(int cardId)
        {
            Card dbEntry = context.Cards.Find(cardId);
            if(dbEntry!=null)
            {
                context.Cards.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }
    }
}
