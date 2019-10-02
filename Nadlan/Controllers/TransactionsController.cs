using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.ViewModels;
using Nadlan.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly NadlanConext _context;
        //private readonly IRepository<Account> _repository;
        private readonly IRepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;

        //public TransactionsController(IRepository<Account> repository, NadlanConext context, IMapper mapper)
        public TransactionsController(IRepositoryWrapper repositoryWrapper, NadlanConext context, IMapper mapper)
        {
            //_repository = repository;
            _repositoryWraper = repositoryWrapper;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Transactions
        [HttpGet]
        public async Task<IEnumerable<TransactionDto>> GetTransactions()
        {
            var transactions = await _repositoryWraper.Transaction.GetAllAsync();
            var transactionsDto = _mapper.Map<List<Transaction>, IEnumerable<TransactionDto>>(transactions);
            return transactionsDto;
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // GET: api/Transactions/5
        [HttpGet("{apartmentId}/{accountId}/{isPurchaseCost}/{year=0}")]
        public async Task<IActionResult> GetTransaction([FromRoute] int apartmentId, int accountId, bool isPurchaseCost, int year)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _repositoryWraper.Transaction.GetByAcountAsync(apartmentId, accountId, isPurchaseCost, year);
            transaction.ForEach(a => a.Amount = Math.Abs(a.Amount));
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // POST: api/Transactions
        [HttpPost]
        public async Task<IActionResult> PostTransaction([FromBody] TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = _mapper.Map<TransactionDto, Transaction>(transactionDto);
            //Not distribution transaction
            if (transaction.AccountId != 100)
            {
                await _repositoryWraper.Transaction.CreateTransactionAsync(transaction);
            }
            //Distribution transaction
            else
            {
                await _repositoryWraper.Transaction.DistributeBalanceAsync(transaction);

            }
            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }


        // POST: api/Transactions
        [HttpPost("PostExpenses/{isHours=false}")]
        public async Task<IActionResult> PostAssistantExpenses([FromBody] TransactionDto transactionDto, bool isHours)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = _mapper.Map<TransactionDto, Transaction>(transactionDto);

            await _repositoryWraper.Transaction.CreateDoubleTransactionAsync(transaction, isHours);


            ////Not distribution transaction
            //if (transaction.AccountId != 100)
            //{
            //    await _repositoryWraper.Transaction.CreateTransactionAsync(transaction);
            //}
            ////Distribution transaction
            //else
            //{
            //    await _repositoryWraper.Transaction.DistributeBalanceAsync(transaction);

            //}
            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }


        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction([FromRoute] int id, [FromBody] Apartment transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.Id)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Transactions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok(transaction);
        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}