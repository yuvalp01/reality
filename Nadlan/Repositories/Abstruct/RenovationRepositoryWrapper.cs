using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Repositories
{
    public class RenovationRepositoryWrapper
    {
        NadlanConext _context;
        RenovationItemRepository _itemRepository;
        RenovationLineRepository _lineRepository;
        public RenovationRepositoryWrapper(NadlanConext conext)
        {
            _context = conext;
        }
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
        public RenovationLineRepository RenovationLineRepository
        {
            get
            {
                if (_lineRepository == null)
                {
                    _lineRepository = new  RenovationLineRepository(_context);
                }
                return _lineRepository;
            }
        }
    }
}
