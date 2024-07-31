using System.Net;
using API._Services.Interfaces.Systems;
using API.Dtos.Systems;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Systems
{
    [IsMenu]
    public class ConditionController : ApiController
    {
        private readonly IConditionService _service;

        public ConditionController(IConditionService service)
        {
            _service = service;
        }

        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(2)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ConditionDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.Create(dto));
        }

        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(3)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ConditionDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Update(dto));
        }

        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(4)]
        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromBody] ConditionDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Delete(dto));
        }

        [ProducesResponseType(typeof(PaginationUtility<ConditionDto>), (int)HttpStatusCode.OK)]
        [MenuMember(4)]
        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] string keyword)
        {
            return Ok(await _service.GetDataPagination(pagination, keyword));
        }

        [ProducesResponseType(typeof(List<KeyValuePair<long, string>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetListCondition")]
        public async Task<IActionResult> GetListCondition()
        {
            return Ok(await _service.GetListCondition());
        }

        [ProducesResponseType(typeof(ConditionDto), (int)HttpStatusCode.OK)]
        [MenuMember(5)]
        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] long id)
        {
            return Ok(await _service.GetDetail(id));
        }
    }
}