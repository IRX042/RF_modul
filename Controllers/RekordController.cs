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
using Listazas.Dnn.listazas.Components;
using Listazas.Dnn.listazas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using Hotcakes.CommerceDTO.v1;
using Hotcakes.CommerceDTO.v1.Client;
using Hotcakes.CommerceDTO.v1.Orders;
using Hotcakes.CommerceDTO.v1.Contacts;

using Hotcakes.Commerce;
using Hotcakes.Commerce.Catalog;
using Hotcakes.Commerce.Extensions;
using Hotcakes.Commerce.Orders;
using Hotcakes.Commerce.Urls;


namespace Listazas.Dnn.listazas.Controllers
{
    [DnnHandleError]
    public class RekordController : DnnController
    {

        [ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<Rekord>();
            var rekords = rep
                .Find("")
                .ToArray();
            IDataContext ctx2 = DataContext.Instance();
            var rep2 = ctx2.GetRepository<felir>();
            var felirs = rep2
                .Find("")
                .ToArray();
            List<Merged> merged = new List<Merged>();
            string FindEmail(int id)
            {
                foreach (var item in rekords)
                {
                    if (item.UserID == id)
                        return item.Email;
                }
                return "no email";
            }
            foreach (var item in felirs)
            {
                if (item.FeliratkozasID == 0)
                    continue;
                Merged mergedInstance = new Merged
                {
                    UserID = item.UserID,
                    FeliratkozasID = item.FeliratkozasID,
                    Email = FindEmail(item.UserID),
                };
                merged.Add(mergedInstance);
            }

            return View(merged);
        }

        [HttpPost]
        public ActionResult OrderList()
        {
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<Rekord>();
            var rekords = rep
                .Find("")
                .ToArray();
            IDataContext ctx2 = DataContext.Instance();
            var rep2 = ctx2.GetRepository<felir>();
            var felirs = rep2
                .Find("")
                .ToArray();
            List<Merged> merged = new List<Merged>();
            string FindEmail(int id)
            {
                foreach (var item in rekords)
                {
                    if (item.UserID == id)
                        return item.Email;
                }
                return "no email";
            }
            foreach (var item in felirs)
            {
                if (item.FeliratkozasID == 0)
                    continue;
                Merged mergedInstance = new Merged
                {
                    UserID = item.UserID,
                    FeliratkozasID = item.FeliratkozasID,
                    Email = FindEmail(item.UserID),
                };
                merged.Add(mergedInstance);
            }

            string url = "http://20.234.113.211:8107";
            url = "http://www.dnndev.me/";
            string key = "1-79771cd1-cb22-4710-a786-b360d8a92c2f";
            key = "1-f7b5b986-6a80-44a9-9141-003b8559ca85";


            Api proxy = new Api(url, key);

            foreach (var rendeles in merged)
            {
                int a = rendeles.UserID;
                string bvin = "";

                // call the API to find all orders in the store
                ApiResponse<List<OrderSnapshotDTO>> response1 = proxy.OrdersFindAll();

                List<OrderSnapshotDTO> orderSnapshotList = response1.Content;

                foreach (var orderSnapshot in orderSnapshotList)
                {
                    if (orderSnapshot.UserID == a.ToString())
                    {
                        bvin = orderSnapshot.bvin; break;
                    }
                }
                var order = new OrderDTO();
                if (bvin == "")
                {
                    order.Items = new List<LineItemDTO>();
                }
                else
                {
                    order = proxy.OrdersFind(bvin).Content;
                }
                // create a new order object
                

                
                switch (rendeles.FeliratkozasID)
                {
                    case 1:
                        order.Items.Add(new LineItemDTO
                        {
                            ProductId = "49538DB5-354C-466E-8DE2-A94974368C9D",
                            IsBundle = true,
                            BasePricePerItem = 6000,
                            AdjustedPricePerItem = 6000,
                            LineTotal = 6000,
                            ProductName = "Bundle1",
                            ProductSku = "6",
                            Quantity = 1
                        });
                        break;
                    default:
                        break;
                }

                if (bvin == "")
                {
                    order.IsPlaced = false;
                    order.FraudScore = -1;

                    order.UserEmail = rendeles.Email;
                    order.UserID = rendeles.UserID.ToString();

                    ApiResponse<OrderDTO> response2 = proxy.OrdersCreate(order);
                }
                else
                {
                    ApiResponse<OrderDTO> response3 = proxy.OrdersUpdate(order);
                }
            }
            return View();
        }
    }
}
