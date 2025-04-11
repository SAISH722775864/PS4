using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD;

[ApiController]
[Route("api/[controller]")]
public class AddressController : ControllerBase, iController<Address>
{
    private WebRestOracleContext _context;
    // Create a field to store the mapper object
    private readonly IMapper _mapper;

    public AddressController(WebRestOracleContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("Get")]
    public async Task<IActionResult> Get()
    {

        List<Address>? lst = null;
        lst = await _context.Address.ToListAsync();
        return Ok(lst);
    }

    [HttpGet]
    [Route("Get/{ID}")]
    public async Task<IActionResult> Get(string ID)
    {
        var itm = await _context.Address.Where(x => x.AddressId == ID).FirstOrDefaultAsync();
        return Ok(itm);
    }

    [HttpDelete]
    [Route("Delete/{ID}")]
    public async Task<IActionResult> Delete(string ID)
    {
        var itm = await _context.Address.Where(x => x.AddressId == ID).FirstOrDefaultAsync();
        _ = _context.Address.Remove(itm);
        _ = await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> Put([FromBody] Address _Address)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            var itm = await _context.Address.AsNoTracking()
            .Where(x => x.AddressId == _Address.AddressId)
            .FirstOrDefaultAsync();

            if (itm != null)
            {
                itm = _mapper.Map<Address>(_Address);

                /*
                       itm.AddressFirstName = _Address.AddressFirstName;
                       itm.AddressMiddleName = _Address.AddressMiddleName;
                       itm.AddressLastName = _Address.AddressLastName;
                       itm.AddressDateOfBirth = _Address.AddressDateOfBirth;
                       itm.AddressGenderId = _Address.AddressGenderId;
                  */
                _ = _context.Address.Update(itm);
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
    public async Task<IActionResult> Post([FromBody] Address _Address)
    {
        var trans = _context.Database.BeginTransaction();

        try
        {
            _Address.AddressId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            _ = _context.Address.Add(_Address);
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
