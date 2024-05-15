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
using form0.Dnn.form0.Components;
using form0.Dnn.form0.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace form0.Dnn.form0.Controllers
{
    [DnnHandleError]
    public class RekordController : DnnController
    {
        UserController userController;
        //[ModuleAction(ControlKey = "Edit", TitleKey = "AddItem")]
        public ActionResult Index()
        {
            var currentUser = UserController.Instance.GetCurrentUserInfo();
            int a = currentUser.UserID;
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<felir>();
            var felirs = rep
                .Find("")
                .ToArray();
            foreach ( var felir in felirs )
            {
                if(felir.UserID == a )
                    return View(felir);
            }
            return View(new felir
            {
                UserID = 0,
                FeliratkozasID = 0,
            });
        }

        [HttpPost]
        public ActionResult CreateFelir(int csomag_tipus=0)
        {
            IDataContext ctx = DataContext.Instance();
            var currentUser = UserController.Instance.GetCurrentUserInfo();
            var rep = ctx.GetRepository<felir>();
            felir felirInstance;
            if (currentUser.UserID == -1)
            {
                return View(new felir
                {
                    UserID = -1,
                    FeliratkozasID = 0,
                });
            }
            felirInstance = new felir
            {
                UserID = currentUser.UserID,
                FeliratkozasID = csomag_tipus,
            };
            rep.Insert(felirInstance);
            ctx.Commit();
            return View(felirInstance);
            
        }
        [HttpPost]
        public ActionResult Leir()
        {
            var currentUser = UserController.Instance.GetCurrentUserInfo();
            int a = currentUser.UserID;
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<felir>();
            var felirs = rep
                .Find("")
                .ToArray();
            int felirID = 0;
            int id = 0;
            foreach (var felir in felirs)
            {
                if (felir.UserID == a)
                {
                    felirID = felir.FeliratkozasID;
                    id = felir.id;
                }
                    
            }
            felir felirInstance = new felir
            {
                UserID = 1,
                FeliratkozasID = 1,
            };
            rep.Delete(rep.GetById(id));
            ctx.Commit();
            return View(felirInstance);

        }

        [HttpPost]
        public ActionResult Valt(int csomag_tipus = 0)
        {
            if(csomag_tipus == 0)
                return View();
            var currentUser = UserController.Instance.GetCurrentUserInfo();
            int a = currentUser.UserID;
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<felir>();
            var felirs = rep
                .Find("")
                .ToArray();
            int felirID = 0;
            int id = 0;
            foreach (var felir in felirs)
            {
                if (felir.UserID == a)
                {
                    felirID = felir.FeliratkozasID;
                    id = felir.id;
                }

            }
            felir felirInstance = rep.GetById(id);
            rep.Delete(felirInstance);
            felirInstance.FeliratkozasID = csomag_tipus;
            rep.Insert(felirInstance);
            ctx.Commit();
            return View();

        }
    }
}
