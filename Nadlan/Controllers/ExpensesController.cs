using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nadlan.Models;
using Nadlan.Repositories;
using Nadlan.ViewModels;
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

        [HttpGet()]
        public async Task<IActionResult> GetExpenses()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _repositoryWraper.Transaction.GetAllExpensesAsync();
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }
        // GET: api/Transactions/5
        //[HttpGet("GetExpense/{transactionId}")]
        [HttpGet("{transactionId}")]
        public async Task<IActionResult> GetExpense([FromRoute] int transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = await _repositoryWraper.Transaction.GetExpenseByIdAsync(transactionId);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }



        [HttpPost()]
        public async Task<IActionResult> PostExpense([FromBody] TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = _mapper.Map<TransactionDto, Transaction>(transactionDto);

            await _repositoryWraper.Transaction.CreateExpenseAndTransactionAsync(transaction);
            return CreatedAtAction("PostExpense", new { id = transaction.Id }, transaction);
        }

        [HttpPut()]
        public async Task<IActionResult> PutExpense([FromBody] TransactionDto transactionDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var transaction = _mapper.Map<TransactionDto, Transaction>(transactionDto);
            await _repositoryWraper.Transaction.UpdateExpenseAndTransactionAsync(transaction);
            return NoContent();
            //return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }
        [Authorize]
        [HttpPut("confirm")]
        public async Task<IActionResult> Confirm([FromBody] int transactionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // await _repositoryWraper.Transaction.CreateDoubleTransactionAsync(transaction, isHours);
            await _repositoryWraper.Transaction.Confirm(transactionId);
            return NoContent();
            //return CreatedAtAction("Confirm", new { id = transactionId });
        }

        //[HttpPut("DeleteExpense")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> SoftDeleteExpense([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _repositoryWraper.Transaction.SoftDelete(id);
            return CreatedAtAction("SoftDeleteExpense", id);
        }

        [HttpGet("GetExpensesBalance")]
        public async Task<IActionResult> GetExpensesBalance()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            decimal balance = await _repositoryWraper.ApartmentReport.GetExpensesBalance();

            return Ok(balance);
        }


    }
}