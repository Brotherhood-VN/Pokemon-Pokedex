using API.Models;
namespace API.Dtos.Systems
{
    public class MenuDto : Menu
    {
        public List<MenuDto> Items { get; set; }
    }
}