using System.Collections.Generic;

namespace RAFE.Models
{

    public class MenuModel
    {
        public List<ParentMenu> MenuList { get; set; }
        public MenuModel()
        {
            MenuList = new List<ParentMenu>();
        }
    }

    public class ParentMenu
    {
        public int intMenuID { get; set; }
        public string vMenuName { get; set; }
        public string vURL { get; set; }
        public string vControllerName { get; set; }
        public string vActionName { get; set; }
        public bool btActive { get; set; }
        public bool BtIsParent { get; set; }
        public List<ChildMenu> ChildList { get; set; }
        public ParentMenu()
        {
            ChildList = new List<ChildMenu>();
        }
    }

    public class ChildMenu
    {
        public int intMenuID { get; set; }
        public int intParentID { get; set; }
        public string vMenuName { get; set; }
        public string vControllerName { get; set; }
        public string vActionName { get; set; }
        public string vURL { get; set; }
        public bool btActive { get; set; }
        public string URLParam { get; set; }
    }
}