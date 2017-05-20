using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GpuStore.Domain.Entities;

namespace GpuStore.Domain.Abstract
{
    public interface ICardRepository
    {
        IEnumerable<Card> Cards { get; }
        void SaveCard(Card card);
        Card DeleteCard(int cardId);
    }
}
