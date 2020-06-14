using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nadlan.Repositories.Renovation;

namespace Nadlan.Repositories
{
    public class RenovationRepositoryWrapper_old
    {
        NadlanConext _context;
        RenovationItemRepository _itemRepository;
        RenovationLineRepository_old _lineRepository_old;
        RenovationLineRepository _lineRepository;
        public RenovationRepositoryWrapper_old(NadlanConext conext)
        {
            _context = conext;
        }

        public RenovationLineRepository RenovationLineRepository
        {
            get
            {
                if (_lineRepository == null)
                {
                    _lineRepository = new RenovationLineRepository(_context);
                }
                return _lineRepository;
            }
        }
        [Obsolete]
        public RenovationItemRepository RenovationItemRepository
        {
            get
            {
                if (_itemRepository == null)
                {
                    _itemRepository = new RenovationItemRepository(_context);
                }
                return _itemRepository;
            }
        }
        public RenovationLineRepository_old RenovationLineRepository_old
        {
            get
            {
                if (_lineRepository_old == null)
                {
                    _lineRepository_old = new RenovationLineRepository_old(_context);
                }
                return _lineRepository_old;
            }
        }
    }
}
