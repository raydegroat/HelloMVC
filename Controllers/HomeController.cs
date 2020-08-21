using HelloMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Runtime.Caching;

namespace HelloMVC.Controllers
{
    public class HomeController : Controller
    {
        ObjectCache cache = MemoryCache.Default;
        List<Customer> customers;

        public HomeController()
        {
            customers = cache["customers"] as List<Customer>;
            if (customers == null)
            {
                customers = new List<Customer>();
            }
        }
        public void SaveCache()
        {
            cache["customers"] = customers;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            ViewBag.MySupperProperty = "This is my first app!";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult ViewCustomer(String id)
        {
            Customer customer = customers.FirstOrDefault(c => c.Id == id);
            if(customer == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(customer);
            }
        }
        public ActionResult AddCustomer()
        {
            //System.Diagnostics.Debug.WriteLine("Now im in the Addcustomer method");
            
            return View();
        }
        //Decorator
        [HttpPost]
        public ActionResult AddCustomer(Customer customer)
        {
            if(!ModelState.IsValid)
            {
                return View(customer);
            }
            customer.Id = Guid.NewGuid().ToString();
            customers.Add(customer);
            SaveCache();
            

            return RedirectToAction("customerList");

        }
        public ActionResult EditCustomer(string id)
        {
            Customer customer = customers.FirstOrDefault(c => c.Id == id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(customer);
            }
        }
        [HttpPost]
        public ActionResult EditCustomer(Customer customer, string Id)
        {
            Customer customerToEdit = customers.FirstOrDefault(c => c.Id == Id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            else
            {
                customerToEdit.Name = customer.Name;
                customerToEdit.Telephone = customer.Telephone;
                SaveCache();

                return RedirectToAction("customerList");
            }
        }
        public ActionResult DeleteCustomer(string Id)
        {
            Customer customer = customers.FirstOrDefault(c => c.Id == Id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(customer);
            }
        }
        [HttpPost]
        [ActionName("DeleteCustomer")]
        public ActionResult ConfirmDeleteCustomer(string Id)
        {
            Customer customer = customers.FirstOrDefault(c => c.Id == Id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            else
            {
                customers.Remove(customer);
                return RedirectToAction("customerList");
            
            }
        }
        public ActionResult CustomerList()
        {
            return View(customers);
        }
    }
}

