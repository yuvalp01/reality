﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nadlan.Repositories.Renovation;

namespace Nadlan.Repositories
{
    public class RenovationRepositoryWrapper
    {
        NadlanConext _context;
        RenovationPaymentRepository _payment;
        RenovationLineRepository _line;
        RenovationProductRepository _product;
        public RenovationRepositoryWrapper(NadlanConext conext)
        {
            _context = conext;
        }

        public RenovationLineRepository RenovationLineRepository
        {
            get
            {
                if (_line == null)
                {
                    _line = new RenovationLineRepository(_context);
                }
                return _line;
            }
        }
        public RenovationPaymentRepository RenovationPaymentRepository
        {
            get
            {
                if (_payment == null)
                {
                    _payment = new RenovationPaymentRepository(_context);
                }
                return _payment;
            }
        }
        public RenovationProductRepository RenovationProductRepository
        {
            get
            {
                if (_product == null)
                {
                    _product = new RenovationProductRepository(_context);
                }
                return _product;
            }
        }

    }
}
