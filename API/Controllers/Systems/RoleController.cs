using System.Net;
using API._Services.Interfaces.Auth;
using API._Services.Interfaces.Systems;
using API.Dtos.Systems;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Systems
{
    [IsMenu]
    public class RoleController : ApiController
    {
        private readonly IRoleService _service;
        private readonly IAuthService _authService;

        public RoleController(
            IRoleService service,
            IAuthService authService)
        {
            _service = service;
            _authService = authService;
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(2)]
        public async Task<IActionResult> Create([FromBody] RoleDto dto)
        {
            dto.CreateBy = UserId;
            dto.CreateTime = Now;
            return Ok(await _service.Create(dto));
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(3)]
        public async Task<IActionResult> Update([FromBody] RoleDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Update(dto));
        }

        [HttpPut("Delete")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(4)]
        public async Task<IActionResult> Delete([FromBody] RoleDto dto)
        {
            dto.UpdateBy = UserId;
            dto.UpdateTime = Now;
            return Ok(await _service.Delete(dto));
        }

        [HttpGet("GetDataPagination")]
        [ProducesResponseType(typeof(PaginationUtility<RoleDto>), (int)HttpStatusCode.OK)]
        [MenuMember(1)]
        public async Task<IActionResult> GetDataPagination([FromQuery] PaginationParam pagination, [FromQuery] string keyword)
        {
            return Ok(await _service.GetDataPagination(pagination, keyword));
        }

        [HttpGet("GetListRole")]
        [ProducesResponseType(typeof(List<KeyValuePair<Guid, string>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListRole()
        {
            return Ok(await _service.GetListRole());
        }

        [HttpGet("GetDetail")]
        public async Task<IActionResult> GetDetail([FromQuery] long id)
        {
            return Ok(await _service.GetDetail(id));
        }

        [HttpGet("GetRoles")]
        [ProducesResponseType(typeof(List<RoleUser>), (int)HttpStatusCode.OK)]
        [MenuMember(6)]
        public async Task<IActionResult> GetRoles([FromQuery] RoleUserParams param)
        {
            return Ok(await _authService.GetRoles(param));
        }
    }
}