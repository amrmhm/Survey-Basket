

using SurveyBasket.Api.Services;

namespace SurveyBasket.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollsServices pollsServices) : ControllerBase
{
    private readonly IPollsServices _pollsServices = pollsServices;
    

    [HttpGet("")]
   
    public IActionResult GetAll ()
    {
        return Ok(_pollsServices.GetAll());
    }
    [HttpGet("{id}")]

    public IActionResult Get(int id )
    {
        var boll = _pollsServices.Get(id);

        return boll is null ? NotFound() : Ok(boll);
    }

    [HttpPost("")]

    public IActionResult Create (Poll request)
    {
        var newPoll =_pollsServices.Create(request);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }
    [HttpPut("{id}")]

    public IActionResult Update (int id ,Poll request)
    {
        var updatePoll =_pollsServices.Update(id, request);
        if (!updatePoll)
            return NotFound();
        return NoContent();

    }
    [HttpDelete("{id}")]
    public IActionResult Delete (int id)
    {
        var deletePoll = _pollsServices.Delete(id);
        if (!deletePoll)
            return NotFound();
        return NoContent();
    }
}
