using RepairShopProject12.Models;
using RepairShopProject12.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepairShopProject12.Controllers
{
    public class AddonController : Controller
    {
        public void LoadStaticStatusData()
        {
            ViewBag.StatusType0Count = DBProcessor.ListAll<OrderModel>(new OrderModel(), "Orders", new List<string> { "OrderStatusLabel" }).Where(d => d.OrderStatus == 0).Count();
            ViewBag.StatusType1Count = DBProcessor.ListAll<OrderModel>(new OrderModel(), "Orders", new List<string> { "OrderStatusLabel" }).Where(d => d.OrderStatus == 1).Count();
            ViewBag.StatusType2Count = DBProcessor.ListAll<OrderModel>(new OrderModel(), "Orders", new List<string> { "OrderStatusLabel" }).Where(d => d.OrderStatus == 2).Count();
            ViewBag.StatusType3Count = DBProcessor.ListAll<OrderModel>(new OrderModel(), "Orders", new List<string> { "OrderStatusLabel" }).Where(d => d.OrderStatus == 3).Count();
        }
    }
}