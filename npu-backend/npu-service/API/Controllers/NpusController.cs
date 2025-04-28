using Application;
using Application.Services;
using Asp.Versioning;
using Domain.Common;
using Domain.Npu;
using Infrastructure.SQL;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[ApiVersion(1)]
[Route("api/v{version:apiVersion}/[controller]")]
public class NpusController(INpuRepository npuRepository, INpuService npuService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ListResult<Npu>>> GetNpus()
    {
        var npus = await npuRepository.GetAllNpus();
        return new ListResult<Npu>() { Items = npus };
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Npu>> GetNpuById(Guid id)
    {
        var npu = await npuRepository.GetNpuById(id);

        if (npu == null)
        {
            return NotFound();
        }

        return npu;
    }

    [HttpPost]
    public async Task<ActionResult<Npu>> CreateNpu([FromBody] CreateNpuInputDto body)
    {
        var input = NpuMapper.MapToInternal(body, Guid.NewGuid()); // In a real application, this should come from the authenticated user
        var createdNpu = await npuService.CreateNpu(input);

        return Ok(createdNpu);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<Npu>> UpdateNpu(Guid id, [FromBody] UpdateNpuInputDto body)
    {
        var input = NpuMapper.MapToInternal(body);
        var result = await npuService.UpdateNpu(id, input);

        if (result == null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteNpu(Guid id)
    {
        var result = await npuRepository.DeleteNpu(id);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }
}
