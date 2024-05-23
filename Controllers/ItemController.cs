/*
' Copyright (c) 2024 Nokedlik
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using Nokedlik.Dnn.suti.Components;
using Nokedlik.Dnn.suti.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1.Orders;
using Hotcakes.CommerceDTO.v1.Contacts;

namespace Nokedlik.Dnn.suti.Controllers
{
    [DnnHandleError]
    public class ItemController : DnnController
    {

        public ActionResult Delete(int itemId)
        {
            ItemManager.Instance.DeleteItem(itemId, ModuleContext.ModuleId);
            return RedirectToDefaultRoute();
        }

        public ActionResult Edit(int itemId = -1)
        {
            DotNetNuke.Framework.JavaScriptLibraries.JavaScript.RequestRegistration(CommonJs.DnnPlugins);

            var userlist = UserController.GetUsers(PortalSettings.PortalId);
            var users = from user in userlist.Cast<UserInfo>().ToList()
                        select new SelectListItem { Text = user.DisplayName, Value = user.UserID.ToString() };

            ViewBag.Users = users;

            var item = (itemId == -1)
                 ? new Item { ModuleId = ModuleContext.ModuleId }
                 : ItemManager.Instance.GetItem(itemId, ModuleContext.ModuleId);

            return View(item);
        }

        [HttpPost]
        [DotNetNuke.Web.Mvc.Framework.ActionFilters.ValidateAntiForgeryToken]
        public ActionResult Edit(Item item)
        {
            if (item.ItemId == -1)
            {
                item.CreatedByUserId = User.UserID;
                item.CreatedOnDate = DateTime.UtcNow;
                item.LastModifiedByUserId = User.UserID;
                item.LastModifiedOnDate = DateTime.UtcNow;

                ItemManager.Instance.CreateItem(item);
            }
            else
            {
                var existingItem = ItemManager.Instance.GetItem(item.ItemId, item.ModuleId);
                existingItem.LastModifiedByUserId = User.UserID;
                existingItem.LastModifiedOnDate = DateTime.UtcNow;
                existingItem.ItemName = item.ItemName;
                existingItem.ItemDescription = item.ItemDescription;
                existingItem.AssignedUserId = item.AssignedUserId;

                ItemManager.Instance.UpdateItem(existingItem);
            }

            return RedirectToDefaultRoute();
        }

        //[ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            var currentUser = UserController.Instance.GetCurrentUserInfo();
            int a = currentUser.UserID;
            string bvin = "";
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<Order>();
            string url = "http://20.234.113.211:8107/DesktopModules/Hotcakes/API/rest/v1/orders";
            url = "http://www.dnndev.me/";

            string key = "1-79771cd1-cb22-4710-a786-b360d8a92c2f";
            key = "1-f7b5b986-6a80-44a9-9141-003b8559ca85";


            // known issue:
            // guestek kosarát nem kezeli megfelelően, mert a süti törlése mindíg végbemegy, nem csak akkor amikor kijelentkezünk/bejelentkezünk
            if (Request.Cookies["hotcakes-cartid-1"] != null)
            {
                // Delete the existing cookie by setting its expiration date to a past date
                Response.Cookies["hotcakes-cartid-1"].Expires = DateTime.Now.AddDays(-1);
            }

            Api proxy = new Api(url, key);
            // call the API to find all orders in the store
            ApiResponse<List<OrderSnapshotDTO>> response = proxy.OrdersFindAll();


            List<OrderSnapshotDTO> orderSnapshotList = new List<OrderSnapshotDTO>();
            if (response != null)
            {
                orderSnapshotList = response.Content;
                if (orderSnapshotList != null)
                {
                    foreach (var orderSnapshot in orderSnapshotList)
                    {
                        if (orderSnapshot.UserID == a.ToString())
                        {
                            bvin = orderSnapshot.bvin; break;
                        }
                    }
                }
            }
            if (bvin != "")
            {
                HttpCookie cookie = new HttpCookie("hotcakes-cartid-1", bvin);

                // Set the cookie expiration (optional)
                cookie.Expires = DateTime.Now.AddDays(1); // Expires in one day

                // Add the cookie to the response
                Response.Cookies.Add(cookie);
            }
            

            return View();
        }
    }
}
