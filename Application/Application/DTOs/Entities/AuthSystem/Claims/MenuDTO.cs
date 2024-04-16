using Microsoft.AspNetCore.Mvc;

namespace Application.DTOs.Claims
{
    public class MenuDTO
    {
        #region Constructors
        public MenuDTO(string title, string area, string controller, string action, Dictionary<string,string> route = null, string icon = null, List<MenuDTO> subMenus = null)
        {
            Title = title;
            Area = area;
            Controller = controller;
            Action = action;
            Route = route;
            Icon = icon;
            SubMenus = subMenus ?? new List<MenuDTO>();
        }
        #endregion


        #region Properties
        public string Title { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public Dictionary<string,string> Route { get; set; }
        public string Icon { get; set; }
        public List<MenuDTO> SubMenus { get; set; } = new List<MenuDTO>();

        public string Claim
        {
            get
            {
                if (string.IsNullOrEmpty(Area))
                    return $"{Controller}.{Action}";
                else
                    return $"{Area}.{Controller}.{Action}";
            }
        }
        #endregion
    }
}
