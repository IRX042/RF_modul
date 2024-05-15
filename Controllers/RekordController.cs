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
            //var items = ItemManager.Instance.GetItems(ModuleContext.ModuleId);
            return View(merged);
        }
    }
}
