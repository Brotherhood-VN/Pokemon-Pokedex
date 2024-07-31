using API._Services.Interfaces.Systems;
using Microsoft.AspNetCore.Mvc;
using API.Dtos.Systems;
using System.Net;

namespace API.Controllers.Systems
{
    [IsMenu]
    public class MenuController : ApiController
    {
        private readonly IMenuService _service;
        public MenuController(IMenuService service)
        {
            _service = service;
        }

        [HttpGet("LoadMenus")]
        [ProducesResponseType(typeof(List<TreeNode<MenuDto>>), (int)HttpStatusCode.OK)]
        [MenuMember(1)]
        public async Task<IActionResult> LoadMenus()
        {
            return Ok(await _service.LoadMenus());
        }

        [HttpPost("ConfigurationMenus")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(2)]
        public async Task<IActionResult> ConfigurationMenus([FromBody] List<TreeNode<MenuDto>> nodes)
        {
            return Ok(await _service.ConfigurationMenus(nodes));
        }

        [HttpPost("Create")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(3)]
        public async Task<IActionResult> Create([FromBody] MenuDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpPut("Update")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(4)]
        public async Task<IActionResult> Update([FromBody] MenuDto dto)
        {
            return Ok(await _service.Update(dto));
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(OperationResult), (int)HttpStatusCode.OK)]
        [MenuMember(5)]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            return Ok(await _service.Delete(id));
        }

        [HttpGet("GetListController")]
        [ProducesResponseType(typeof(List<KeyValuePair<string, string>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetListController()
        {
            return Ok(await _service.GetListController());
        }
    }
}