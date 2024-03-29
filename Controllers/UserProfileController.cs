using OrderTracker.Models;
using OrderTracker.CreateViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace UserProfile.Controllers
{
    public class UserProfileController : Controller
    {
        public static Employee CurrentEmployee = null;
        private readonly IHostingEnvironment hostingEnvironment;

        public UserProfileController(
                              IHostingEnvironment hostingEnvironment)
        {

            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ViewResult Create()
        {
            Console.WriteLine("User Create");
            return View();
        }

        [HttpGet]
        public ViewResult Details()
        {

            return View(UserProfileController.CurrentEmployee);

        }

        [HttpPost]
        public IActionResult Create(CreateViewModel model)
        {
            Console.WriteLine(model.Email + " is incoming model email");
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.Photo != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    // Department = model.Department,
                    // Store the file name in PhotoPath property of the employee object
                    // which gets saved to the Employees database table

                    PhotoPath = uniqueFileName
                };
                Console.WriteLine(newEmployee.PhotoPath + "THIS IS THE PHOTO PATH");
                Console.WriteLine(newEmployee.Email + "email");
                UserProfileController.CurrentEmployee = newEmployee;
                return RedirectToAction("details");
            }

            return View();
        }
    }
}