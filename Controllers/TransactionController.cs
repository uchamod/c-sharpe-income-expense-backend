using income_expense_tracker.DTO;
using income_expense_tracker.Services;
using Microsoft.AspNetCore.Mvc;

namespace income_expense_tracker.Controllers
{
    [ApiController]
    [Route("v1/api/transcations")]
    public class TransactionController : ControllerBase
    {
        private readonly ITranscationService _transcationService;

        public TransactionController(ITranscationService transcationService) => _transcationService = transcationService;

        [HttpGet]
        public async Task<ActionResult<List<TransactionResponse>>> GetAll() =>
            Ok(await _transcationService.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<TransactionResponse>> GetById(long id)
        {
            var result = await _transcationService.GetByIdAsync(id);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TransactionResponse>> Create([FromBody] TranscationRequest request)
        {
            var result = await _transcationService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById),new {id = result.Id},result);
        }

        [HttpPut("{id:long}")]
        public async Task<ActionResult<TransactionResponse>> Update(long id, [FromBody] TranscationRequest request)
        {
            var result = await _transcationService.UpdateAsync(id, request);
            return result is null ? NotFound() : Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id) =>
            await _transcationService.DeleteAsync(id) ? Ok() : NotFound();
        
          

    }
}
