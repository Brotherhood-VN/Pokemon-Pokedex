using API._Services.Interfaces.Systems;
using API.Dtos.Systems;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Systems
{
    public class StoneController : ApiController
    {
        private readonly IStoneService _service;

        public StoneController(IStoneService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] StoneDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.Create(dto));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] StoneDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Update(dto));
        }

        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromBody] StoneDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Delete(dto));
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] string keyword)
        {
            return Ok(await _service.GetDataPagination(pagination, keyword));
        }

        [HttpGet("GetListStone")]
        public async Task<IActionResult> GetListStone()
        {
            return Ok(await _service.GetListStone());
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] long id)
        {
            return Ok(await _service.GetDetail(id));
        }
    }
}