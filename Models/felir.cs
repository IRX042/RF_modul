using DotNetNuke.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Listazas.Dnn.listazas.Models
{
    [TableName("Feliratkozasok")]
    //setup the primary key for table
    [PrimaryKey("UserID", AutoIncrement = true)]
    //configure caching using PetaPoco
    //[Cacheable("Rekords", CacheItemPriority.Default, 20)]
    //scope the objects to the ModuleId of a module on a page (or copy of a module on a page)
    [Scope("ModuleId")]
    public class felir
    {
        public int UserID { get; set; }
        public int FeliratkozasID { get; set; }
    }
}