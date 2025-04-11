using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase, iController<Orders>
{
    private WebRestOracleContext _context;
    // Create a field to store the mapper object
    private readonly IMapper _mapper;

    public OrdersController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {

        List<Orders>? lst = null;
        lst = await _context.Orders.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.Orders.Where(x => x.OrdersId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.Orders.Where(x => x.OrdersId == ID).FirstOrDefaultAsync();
        _ = _context.Orders.Remove(itm);
        _ = await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Orders _Orders)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.Orders.AsNoTracking()
            .Where(x => x.OrdersId == _Orders.OrdersId)
            .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<Orders>(_Orders);

                /*
                       itm.OrdersFirstName = _Orders.OrdersFirstName;
                       itm.OrdersMiddleName = _Orders.OrdersMiddleName;
                       itm.OrdersLastName = _Orders.OrdersLastName;
                       itm.OrdersDateOfBirth = _Orders.OrdersDateOfBirth;
                       itm.OrdersGenderId = _Orders.OrdersGenderId;
                  */
                _ = _context.Orders.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] Orders _Orders)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _Orders.OrdersId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _ = _context.Orders.Add(_Orders);
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
