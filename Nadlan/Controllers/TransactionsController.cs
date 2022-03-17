using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nadlan.Models;
using Nadlan.Models.Enums;
using Nadlan.Repositories;
using Nadlan.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly NadlanConext _context;
        private readonly IRepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;

        public TransactionsController(IRepositoryWrapper repositoryWrapper, NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = repositoryWrapper;
            _context = context;
            _mapper = mapper;
        }


        // GET: api/Transactions
        [HttpGet]
        public async Task<IEnumerable<TransactionDto>> GetTransactions([FromQuery] Filter filter)
        {
            var transactions = await _repositoryWraper.Transaction.GetFilteredTransactions(filter);

            var transactionsDto = _mapper.Map<List<Transaction>, IEnumerable<TransactionDto>>(transactions);
            return transactionsDto;
        }

        //[Authorize(Policy = "CanViewTransactions")]
        // GET: api/Transactions
        [HttpGet("list/{monthsBack}")]
        public async Task<IEnumerable<TransactionDto>> GetTransactions(int monthsBack)
        {
            var transactions = await _repositoryWraper.Transaction.GetAllAsync(monthsBack, CreatedByEnum.Any);

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

        [HttpGet("getByPersonalTransactionId/{id}")]
        public async Task<IActionResult> GetByPersonalTransactionId([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaction = await _context.Transactions
                .Where(a => a.IsDeleted == false)
                .Where(a => a.PersonalTransactionId == id)
                .OrderByDescending(a => a.Date)
                .ToListAsync();

            if (transaction == null) return NotFound();

            return Ok(transaction);
        }

        [HttpGet("getPendingExpensesForInvestor/{stakeholderId}")]
        public async Task<IActionResult> GetPendingExpensesForInvestor([FromRoute] int stakeholderId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaction = await _repositoryWraper.Transaction
                .GetPendingExpensesForInvestor(stakeholderId);

            if (transaction == null) return NotFound();

            return Ok(transaction);
        }

        [HttpGet("getPendingExpensesForApartment/{apartmentId}/{year}")]
        public async Task<IActionResult> getPendingExpensesForApartment([FromRoute] int apartmentId, int year)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var transaction = await _repositoryWraper.Transaction
                .GetPendingExpensesForApartment(apartmentId, year);

            if (transaction == null) return NotFound();

            return Ok(transaction);
        }


        [HttpGet("GetFiltered")]
        public async Task<IEnumerable<TransactionDto>> GetFiltered([FromQuery] Filter filter)
        {
            var transactions = await _repositoryWraper.Transaction.GetFilteredTransactions(filter);

            var transactionsDto = _mapper.Map<List<Transaction>, IEnumerable<TransactionDto>>(transactions);
            return transactionsDto;
        }



        // GET: api/Transactions/5
        [HttpGet("{apartmentId}/{accountId}/{isPurchaseCost}/{year=0}")]
        public async Task<IActionResult> GetTransaction([FromRoute] int apartmentId, int accountId, bool isPurchaseCost, int year)
        {
            //Purchase costs are not year dependant
            if (isPurchaseCost)
            {
                year = 0;
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _repositoryWraper.Transaction.GetByAcountAsync(apartmentId, accountId, isPurchaseCost, year);
            //transaction.ForEach(a => a.Amount = Math.Abs(a.Amount));
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
            //Classic transactions (not expenses) will get userAccount 1 
            transaction.CreatedBy = (int)CreatedByEnum.Yuval;

            //Not distribution transaction
            if (transaction.AccountId != (int)Accounts.Distribution)
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
        [HttpPost("PostRent")]
        public async Task<IActionResult> PostRent([FromBody] TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (transactionDto.AccountId != (int)Accounts.Rent)
            {
                return BadRequest("Only rent account (1) is valid");
            }
            var rentTrans = _mapper.Map<TransactionDto, Transaction>(transactionDto);

            var result = await _repositoryWraper.Transaction.CreateRentTransactionsAsync(rentTrans);

            return Ok(result);
        }

        // PUT: api/Transactions/5
        [HttpPut("UpdateRent/{id}")]
        public async Task<IActionResult> UpdateRent([FromRoute] int id, [FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TransactionExists(id))
            {
                return BadRequest();
            }


            var results = await _repositoryWraper.Transaction.UpdateRentTransactionsAsync(transaction);

            return NoContent();
            //return Ok(results);
        }

        [HttpDelete("SoftDeleteRent/{id}")]
        public async Task<IActionResult> SoftDeleteRent([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repositoryWraper.Transaction.SoftDeleteRentTransactionsAsync(id);
            return Ok();

        }


        [HttpPut("confirm")]
        public async Task<IActionResult> Confirm([FromBody] int transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _repositoryWraper.Transaction.Confirm(transactionId);
            return CreatedAtAction("Confirm", new { id = transactionId });
        }

        [HttpPut("payUnpay")]
        public async Task<IActionResult> PayUnpay([FromBody] int transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool isPendingNew = await _repositoryWraper.Transaction.PayUnpay(transactionId);
            return CreatedAtAction("Confirm", new { id = transactionId, isPending = isPendingNew });
        }







        // PUT: api/Transactions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransaction([FromRoute] int id, [FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!TransactionExists(id))
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
            await _repositoryWraper.Transaction.SoftDeleteTransactionAsync(id);
            return Ok();

        }

        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }
    }
}
