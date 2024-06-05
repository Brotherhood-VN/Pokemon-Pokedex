using API._Services.Interfaces.Systems;
using API.Dtos.Systems;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Systems
{
    public class FunctionController : ApiController
    {
        private readonly IFunctionService _service;

        public FunctionController(IFunctionService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] FunctionDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.Create(dto));
        }

        [HttpPost("CreateRange")]
        public async Task<IActionResult> CreateRange([FromBody] FunctionViewDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.CreateRange(dto));
        }

        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromBody] FunctionDto dto)
        {
            return Ok(await _service.Delete(dto));
        }

        [HttpPut("DeleteRange")]
        public async Task<IActionResult> DeleteRange([FromBody] FunctionViewDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.DeleteRange(dto));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] FunctionDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Update(dto));
        }

        [HttpPut("UpdateRange")]
        public async Task<IActionResult> UpdateRange([FromBody] FunctionViewDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.UpdateRange(dto));
        }

        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] FunctionSearchParam param)
        {
            return Ok(await _service.GetDataPagination(pagination, param));
        }

        [HttpPost("CreateAllFunction")]
        public async Task<IActionResult> CreateAllFunction(string controller)
        {
            return Ok(await _service.CreateAllFunction(UserId, controller));
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] FunctionParam param)
        {
            return Ok(await _service.GetDetail(param));
        }

        [HttpGet("GetDetailGroup")]
        public async Task<IActionResult> GetDetailGroup([FromQuery] FunctionParam param)
        {
            return Ok(await _service.GetDetailGroup(param));
        }

        [HttpGet("GetListArea")]
        public async Task<IActionResult> GetListArea()
        {
            return Ok(await _service.GetListArea());
        }

        [HttpGet("GetListController")]
        public async Task<IActionResult> GetListController([FromQuery] string area)
        {
            return Ok(await _service.GetListController(area));
        }

        [HttpGet("GetListAction")]
        public async Task<IActionResult> GetListAction([FromQuery] string area, [FromQuery] string controller)
        {
            return Ok(await _service.GetListAction(area, controller));
        }
    }
}