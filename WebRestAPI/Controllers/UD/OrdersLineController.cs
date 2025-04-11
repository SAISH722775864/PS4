using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class OrdersLineController : ControllerBase, iController<OrdersLine>
{
    private WebRestOracleContext _context;
    // Create a field to store the mapper object
    private readonly IMapper _mapper;

    public OrdersLineController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {

        List<OrdersLine>? lst = null;
        lst = await _context.OrdersLine.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.OrdersLine.Where(x => x.OrdersLineId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.OrdersLine.Where(x => x.OrdersLineId == ID).FirstOrDefaultAsync();
        _ = _context.OrdersLine.Remove(itm);
        _ = await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] OrdersLine _OrdersLine)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.OrdersLine.AsNoTracking()
            .Where(x => x.OrdersLineId == _OrdersLine.OrdersLineId)
            .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<OrdersLine>(_OrdersLine);

                /*
                       itm.OrdersLineFirstName = _OrdersLine.OrdersLineFirstName;
                       itm.OrdersLineMiddleName = _OrdersLine.OrdersLineMiddleName;
                       itm.OrdersLineLastName = _OrdersLine.OrdersLineLastName;
                       itm.OrdersLineDateOfBirth = _OrdersLine.OrdersLineDateOfBirth;
                       itm.OrdersLineGenderId = _OrdersLine.OrdersLineGenderId;
                  */
                _ = _context.OrdersLine.Update(itm);
                _ = await _context.SaveChangesAsync();
                trans.Commit();

            }
        }
        catch (Exception ex)
        {
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return Ok();

    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OrdersLine _OrdersLine)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _OrdersLine.OrdersLineId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _ = _context.OrdersLine.Add(_OrdersLine);
            _ = await _context.SaveChangesAsync();
            trans.Commit();
        }
        catch (Exception ex)
        {
            trans.Rollback();
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }

        return Ok();
    }

}
