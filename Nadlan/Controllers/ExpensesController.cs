using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models;
using Nadlan.Models.Enums;
using Nadlan.Repositories;
using Nadlan.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nadlan.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExpensesController : ControllerBase
    {
        private readonly NadlanConext _context;
        private readonly IRepositoryWrapper _repositoryWraper;
        private readonly IMapper _mapper;

        public ExpensesController(IRepositoryWrapper repositoryWrapper, NadlanConext context, IMapper mapper)
        {
            _repositoryWraper = repositoryWrapper;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("list/{monthsBack}")]
        public async Task<IActionResult> GetExpenses(int monthsBack)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var transactions = await _repositoryWraper.Transaction.GetAllTransactionDtoAsync(monthsBack, CreatedByEnum.Stella);

            if (transactions == null)
            {
                return NotFound();
            }

            return Ok(transactions);
        }
        // GET: api/Transactions/5
        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetExpense([FromRoute] int transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var transaction = await _context.Transactions.FindAsync(transactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }


        private decimal AlignSignWithAccount(decimal amount, int accountId)
        {
            List<Accounts> positiveAccounts = new List<Accounts> { Accounts.Rent, Accounts.SecurityDeposit, Accounts.CashWithdrawal };

            if (positiveAccounts.Contains((Accounts)accountId))
            {
                amount = Math.Abs(amount);
            }
            else
            {
                amount = Math.Abs(amount) * -1;

            }
            return amount;
        }


        [HttpPost()]
        public async Task<IActionResult> PostExpense([FromBody] TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            transactionDto.Amount = AlignSignWithAccount(transactionDto.Amount, transactionDto.AccountId);

            var transaction = _mapper.Map<TransactionDto, Transaction>(transactionDto);
            //Expenses will be just a transaction with userAccount 2 (Stella) 
            transaction.CreatedBy = (int)CreatedByEnum.Stella;

            await _repositoryWraper.Transaction.CreateTransactionAndExpenseAsync(transaction);
            return CreatedAtAction("PostExpense", new { id = transaction.Id }, transaction);
        }

        [HttpPost("ReceiveCash")]
        public async Task<IActionResult> ReceiveCash([FromBody] TransactionDto transactionDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            transactionDto.Amount = AlignSignWithAccount(transactionDto.Amount, transactionDto.AccountId);

            var transaction = _mapper.Map<TransactionDto, Transaction>(transactionDto);
            //Expenses will be just a transaction with userAccount 2 (Stella) 
            transaction.CreatedBy = (int) CreatedByEnum.Stella;

            await _repositoryWraper.Transaction.CreateTransactionAndPersonalTransAsync(transaction);
            return CreatedAtAction("PostExpense", new { id = transaction.Id }, transaction);
        }




        [HttpPut()]
        public async Task<IActionResult> PutExpense([FromBody] Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            transaction.Amount = AlignSignWithAccount(transaction.Amount, transaction.AccountId);

            await _repositoryWraper.Transaction.UpdateExpenseAndTransactionAsync(transaction);
            return NoContent();

        }


        [Authorize]
        [HttpPut("confirm")]
        public async Task<IActionResult> Confirm([FromBody] int transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repositoryWraper.Transaction.Confirm(transactionId);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteExpense([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repositoryWraper.Transaction.SoftDeleteTransactionAsync(id);
            return CreatedAtAction("SoftDeleteExpense", id);
        }

        [HttpGet("GetExpensesBalance")]
        public async Task<IActionResult> GetExpensesBalance()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            decimal balance = await _repositoryWraper.Transaction.GetExpensesBalance();

            return Ok(balance);
        }


    }
}