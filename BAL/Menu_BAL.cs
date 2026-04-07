using RAFE.DAL;
using RAFE.Models;
using RAFE.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RAFE.BAL
{
    public class Menu_BAL
    {
        public MenuModel GetMenu(string UserID)
        {
            MenuModel ObjMenuModel = new MenuModel();
            try
            {               
                SqlCommand command = new SqlCommand();
                command.Parameters.Add("@userName", SqlDbType.VarChar, 150).Value = UserID;
                command.Parameters.Add("@action", SqlDbType.VarChar, 150).Value = "GETMENUFORUSER";
                using (DataTable dt = SQLHelper.GetDataTable(Constants.sp_user, command))
                {
                    if (dt.Rows.Count > 0)
                    {
                        var ParentList = (from v in dt.AsEnumerable()
                                          select new ParentMenu
                                          {
                                              intMenuID = v.Field<int>("intMenuID"),
                                              vMenuName = v.Field<string>("vMenuName"),
                                              vControllerName = v.Field<string>("vControllerName"),
                                              vActionName = v.Field<string>("vActionName"),
                                              vURL = v.Field<string>("vURL"),
                                              BtIsParent = v.Field<bool>("BtIsParent"),
                                              btActive = v.Field<bool>("btActive")
                                          }).Distinct().Where(i => i.BtIsParent == true).Where(i => i.btActive == true).ToList<ParentMenu>();

                        foreach (var Pmenu in ParentList)
                        {
                            var childMenu = (from v in dt.AsEnumerable()
                                             select new ChildMenu
                                             {
                                                 intMenuID = v.Field<int>("intMenuID"),
                                                 vMenuName = v.Field<string>("vMenuName"),
                                                 vControllerName = v.Field<string>("vControllerName"),
                                                 vActionName = v.Field<string>("vActionName"),
                                                 vURL = v.Field<string>("vURL"),
                                                 intParentID = v.Field<int>("intParentID"),
                                                 btActive = v.Field<bool>("btActive"),
                                                 URLParam = v.Field<string>("URLParam")
                                             }).Distinct().Where(i => i.intParentID == Pmenu.intMenuID).Where(i => i.btActive == true).ToList<ChildMenu>();
                            Pmenu.ChildList = childMenu;
                            ObjMenuModel.MenuList.Add(Pmenu);
                        }
                    }
                }
            }
            catch (Exception)
            {
 
            }
            return ObjMenuModel;
        }
    }
}