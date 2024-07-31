using System.Net;
using API._Services.Interfaces.Systems;
using API.Dtos.Systems;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Systems
{
    [IsMenu]
    public class FunctionController : ApiController
    {
        private readonly IFunctionService _service;

        public FunctionController(IFunctionService service)
        {
            _service = service;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(3)]
        public async Task<IActionResult> Create([FromBody] FunctionDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.Create(dto));
        }

        [HttpPost("CreateRange")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(4)]
        public async Task<IActionResult> CreateRange([FromBody] FunctionViewDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.CreateRange(dto));
        }

        [HttpPut("Delete")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(7)]
        public async Task<IActionResult> Delete([FromBody] FunctionDto dto)
        {
            return Ok(await _service.Delete(dto));
        }

        [HttpPut("DeleteRange")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(8)]
        public async Task<IActionResult> DeleteRange([FromBody] FunctionViewDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.DeleteRange(dto));
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(5)]
        public async Task<IActionResult> Update([FromBody] FunctionDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Update(dto));
        }

        [HttpPut("UpdateRange")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(6)]
        public async Task<IActionResult> UpdateRange([FromBody] FunctionViewDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.UpdateRange(dto));
        }

        [HttpGet("GetDataPagination")]
        [ProducesResponseType(typeof(PaginationUtility<FunctionViewDto>), (int)HttpStatusCode.OK)]
        [MenuMember(1)]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] FunctionSearchParam param)
        {
            return Ok(await _service.GetDataPagination(pagination, param));
        }

        [HttpPost("CreateAllFunction")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(2)]
        public async Task<IActionResult> CreateAllFunction(string controller)
        {
            return Ok(await _service.CreateAllFunction(UserId, controller));
        }

        [HttpGet("GetDetail")]
        [ProducesResponseType(typeof(FunctionDto), (int)HttpStatusCode.OK)]
        [MenuMember(9)]
        public async Task<IActionResult> GetDetail([FromQuery] FunctionParam param)
        {
            return Ok(await _service.GetDetail(param));
        }

        [HttpGet("GetDetailGroup")]
        [ProducesResponseType(typeof(FunctionViewDto), (int)HttpStatusCode.OK)]
        [MenuMember(10)]
        public async Task<IActionResult> GetDetailGroup([FromQuery] FunctionParam param)
        {
            return Ok(await _service.GetDetailGroup(param));
        }

        [HttpGet("GetListArea")]
        [ProducesResponseType(typeof(List<KeyValuePair<string, string>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListArea()
        {
            return Ok(await _service.GetListArea());
        }

        [HttpGet("GetListController")]
        [ProducesResponseType(typeof(List<KeyValuePair<string, string>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListController([FromQuery] string area)
        {
            return Ok(await _service.GetListController(area));
        }

        [HttpGet("GetListAction")]
        [ProducesResponseType(typeof(List<KeyValuePair<string, string>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListAction([FromQuery] string area, [FromQuery] string controller)
        {
            return Ok(await _service.GetListAction(area, controller));
        }
    }
}