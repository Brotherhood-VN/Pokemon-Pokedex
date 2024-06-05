using API._Services.Interfaces.Systems;
using API.Dtos.Systems;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Systems
{
    public class GenderController : ApiController
    {
        private readonly IGenderService _service;

        public GenderController(IGenderService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] GenderDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.Create(dto));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] GenderDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Update(dto));
        }

        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromBody] GenderDto dto)
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

        [HttpGet("GetListGender")]
        public async Task<IActionResult> GetListGender()
        {
            return Ok(await _service.GetListGender());
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] long id)
        {
            return Ok(await _service.GetDetail(id));
        }
    }
}