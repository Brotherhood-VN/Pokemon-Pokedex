using API._Services.Interfaces.Systems;
using Microsoft.AspNetCore.Mvc;
using API.Dtos.Systems;

namespace API.Controllers.Systems
{
    public class MenuController : ApiController
    {
        private readonly IMenuService _service;
        public MenuController(IMenuService service)
        {
            _service = service;
        }

        [HttpGet("LoadMenus")]
        public async Task<IActionResult> LoadMenus()
        {
            return Ok(await _service.LoadMenus());
        }

        [HttpPost("ConfigurationMenus")]
        public async Task<IActionResult> ConfigurationMenus([FromBody] List<TreeNode<MenuDto>> nodes)
        {
            return Ok(await _service.ConfigurationMenus(nodes));
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] MenuDto dto)
        {
            return Ok(await _service.Create(dto));
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] MenuDto dto)
        {
            return Ok(await _service.Update(dto));
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromQuery] long id)
        {
            return Ok(await _service.Delete(id));
        }

        [HttpGet("GetListController")]
        public async Task<IActionResult> GetListController()
        {
            return Ok(await _service.GetListController());
        }
    }
}