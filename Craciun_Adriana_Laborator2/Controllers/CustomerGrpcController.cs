using Craciun_Adriana_Laborator2.Models;
using Grpc.Net.Client;
using GrpcCustomerService;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Craciun_Adriana_Laborator2.Controllers
{
    public class CustomerGrpcController : Controller
    {
        private readonly GrpcChannel channel;
        public CustomerGrpcController()
        {
            //a se modifica portul corespunzator (vezi in proiectul GrpcCustomerService->Properties->launchSettings.json)
            channel = GrpcChannel.ForAddress("https://localhost:7210");
        }
        [HttpGet]
        public IActionResult Index()
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            CustomerList cust = client.GetAll(new Empty());
            return View(cust);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(GrpcCustomerService.Customer customer)
        {
            if (ModelState.IsValid)
            {
                var client = new CustomerService.CustomerServiceClient(channel);
                var createdCustomer = client.Insert(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var client = new CustomerService.CustomerServiceClient(channel);
            GrpcCustomerService.Customer customer= client.Get(new CustomerId() { Id = (int)id });
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            Empty response = client.Delete(new CustomerId()
            {
                Id = id
            });
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int? id)
        {
            var client = new CustomerService.CustomerServiceClient(channel);
            if (id == null)
            {
                return NotFound();
            }


            var customer = client.Get(new CustomerId()
            {
                Id = id.Value
            });

            return View(customer);
        }

        [HttpPost, ActionName("Edit")]
        public IActionResult EditCustomer(GrpcCustomerService.Customer customer)
        {
            if (ModelState.IsValid == true)
            { 
                var client = new CustomerService.CustomerServiceClient(channel);
                var updatedClient = client.Update(customer);
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }



    }
}
