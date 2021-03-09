using System.Collections.Generic;
using System.Web.Mvc;
using RepairShopProject12.Models;
using RepairShopProject12.Scripts;

namespace RepairShopProject12.Controllers
{
    public class OrderController : AddonController
    {
        public ActionResult Index()
        {
            LoadStaticStatusData();
            return View();
        }
        //---------------------------------------------------------------------------------------------
        //=============================================================================================
        //---------------------------------------------------------------------------------------------
        public ActionResult AddImage()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddImage(ImageModel imageModel)
        {
            DBProcessor.Create<ImageModel>(imageModel, "Images");
            return View();
        }
        //---------------------------------------------------------------------------------------------
        //=============================================================================================
        //---------------------------------------------------------------------------------------------
        public ActionResult OrderList()
        {
            LoadStaticStatusData();
            List<OrderModel> results = DBProcessor.ListEverything<OrderModel>("Orders");
            foreach (var item in results)
            {
                item.OrderStatusLabel = (StatusType)item.OrderStatus;
            }
            return View(results);
        }
        public ActionResult CreateOrder()
        {
            LoadStaticStatusData();
            return View();
        }
        [HttpPost]
        public ActionResult CreateOrder(OrderModel orderModel)
        {
            LoadStaticStatusData();
            orderModel.EmployeeId = 0;
            orderModel.OrderId = DBProcessor.Count<OrderModel>("Orders") +1;

            DBProcessor.Create<ImageLinkModel>(new ImageLinkModel(), "ImageLinks");


            orderModel.ImageID = DBProcessor.Count<ImageLinkModel>("ImageLinks");
            DBProcessor.Create(orderModel, "Orders",true, new List<string> { "OrderStatusLabel" });

            return View();
        }
        public ActionResult EditOrder(int OrderId)
        {
            LoadStaticStatusData();
            OrderModel results = DBProcessor.OneById<OrderModel>(OrderId, "Orders");
            results.OrderStatusLabel = (StatusType)results.OrderStatus; //Small patch
            return View(results);
        }
        [HttpPost]
        public ActionResult EditOrder(OrderModel orderModel)
        {
            LoadStaticStatusData();
            orderModel.OrderStatus = (int)orderModel.OrderStatusLabel;
            DBProcessor.Update(orderModel, "Orders", new List<string> { "OrderStatusLabel" });

            return RedirectToAction("OrderDetails", orderModel);
        }
        public ActionResult OrderDetails(OrderModel orderModel)
        {
            LoadStaticStatusData();
            var results = DBProcessor.Details(orderModel, "Orders");
            results.OrderStatusLabel = (StatusType)results.OrderStatus;
            return View(results);
        }
        public ActionResult DeleteOrder(int OrderId)
        {
            LoadStaticStatusData();
            var results = DBProcessor.OneById<OrderModel>(OrderId, "Orders", wherePropertie: "OrderId");
            results.OrderStatusLabel = (StatusType)results.OrderStatus;
            return View(results);
        }
        [HttpPost]
        public ActionResult DeleteOrder(OrderModel orderModel)
        {
            LoadStaticStatusData();
            DBProcessor.Delete("Orders", orderModel.Id.ToString());
            return View();
        }
        //---------------------------------------------------------------------------------------------
        //=============================================================================================
        //---------------------------------------------------------------------------------------------
        public ActionResult PartsList()
        {
            LoadStaticStatusData();
            return View(DBProcessor.ListEverything<PartModel>("ComputerParts"));
        }
        public ActionResult CreatePart()
        {
            LoadStaticStatusData();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePart(PartModel partModel)
        {
            LoadStaticStatusData();
            DBProcessor.Create<PartModel>(partModel, "ComputerParts");
            return View();
        }

        public ActionResult EditPart(int PartId)
        {
            LoadStaticStatusData();
            return View(DBProcessor.OneById<PartModel>(PartId, "ComputerParts"));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPart(PartModel partModel)
        {
            LoadStaticStatusData();
            DBProcessor.Update(partModel, "ComputerParts");
            return View();
        }
        //---------------------------------------------------------------------------------------------
        //=============================================================================================
        //---------------------------------------------------------------------------------------------
        public ActionResult RegisterUser()
        {
            LoadStaticStatusData();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(UserModel userModel)
        {
            LoadStaticStatusData();
            DBProcessor.Create<UserModel>(userModel, "Users");
            return View();
        }
        public ActionResult CreateUser()
        {
            LoadStaticStatusData();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserModel userModel)
        {
            LoadStaticStatusData();
            DBProcessor.Create<UserModel>(userModel, "Users");
            return View();
        }
        public ActionResult EditUser(int UserId)
        {
            LoadStaticStatusData();
            var x = DBProcessor.OneById<UserModel>(UserId, "Users");
            return View(x);
        }
        [HttpPost]
        public ActionResult EditUser(UserModel userModel)
        {
            LoadStaticStatusData();
            DBProcessor.Update(userModel, "Users");
            return View();
        }

        [HttpPost]
        public ActionResult DeleteUser(UserModel userModel)
        {
            LoadStaticStatusData();
            DBProcessor.Delete("Users", userModel.Id.ToString());
            return View();
        }
        //---------------------------------------------------------------------------------------------
        //=============================================================================================
        //---------------------------------------------------------------------------------------------
        public ActionResult CreateEmployee()
        {
            LoadStaticStatusData();
            return View();
        }
        [HttpPost]
        public ActionResult CreateEmployee(EmployeeModel employeeModel)
        {
            LoadStaticStatusData();
            DBProcessor.Create<EmployeeModel>(employeeModel, "Employees");
            return View();
        }
        public ActionResult EditEmployee(int EmployeeId)
        {
            LoadStaticStatusData();
            return View(DBProcessor.OneById<EmployeeModel>(EmployeeId, "Employees"));
        }
        [HttpPost]
        public ActionResult EditEmployee(EmployeeModel employeeModel)
        {
            LoadStaticStatusData();
            DBProcessor.Update(employeeModel, "Employees");
            return View();
        }
        public ActionResult EmployeesList()
        {
            LoadStaticStatusData();
            return View(DBProcessor.ListEverything<EmployeeModel>("Employees"));
        }
    }
}