using System.Net;
using API._Services.Interfaces.Systems;
using API.Dtos.Systems;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Systems
{
    [IsMenu]
    public class ItemController : ApiController
    {
        private readonly IItemService _service;

        public ItemController(IItemService service)
        {
            _service = service;
        }

        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(2)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] ItemDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.Create(dto));
        }

        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(3)]
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ItemDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Update(dto));
        }

        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(4)]
        [HttpPut("Delete")]
        public async Task<IActionResult> Delete([FromBody] ItemDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Delete(dto));
        }

        [ProducesResponseType(typeof(PaginationUtility<ItemDto>), (int)HttpStatusCode.OK)]
        [MenuMember(1)]
        [HttpGet("GetDataPagination")]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] string keyword)
        {
            return Ok(await _service.GetDataPagination(pagination, keyword));
        }

        [ProducesResponseType(typeof(List<KeyValuePair<long, string>>), (int)HttpStatusCode.OK)]
        [HttpGet("GetListItem")]
        public async Task<IActionResult> GetListItem()
        {
            return Ok(await _service.GetListItem());
        }

        [ProducesResponseType(typeof(ItemDto), (int)HttpStatusCode.OK)]
        [MenuMember(5)]
        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] long id)
        {
            return Ok(await _service.GetDetail(id));
        }
    }
}