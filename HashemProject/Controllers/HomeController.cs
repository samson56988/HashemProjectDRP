using HashemProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace HashemProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnviroment;
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
     
        public string ConnectionString { get; set; }
        public string ProviderName { get; set; }

        public HomeController(ILogger<HomeController> logger, IConfiguration config, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _config = config;
            ConnectionString = _config.GetConnectionString("DefaultConnectionString");
            ProviderName = "System.Data.SqlClient";
            _webHostEnviroment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult Vehicleowner()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Vehicleowner(Vehicleowners vehicle)
        {
            long size = vehicle.Vehicledocument.Length;
            long sizee = vehicle.purchaseticket.Length;
            var roothPath = Path.Combine(_webHostEnviroment.ContentRootPath, "Resources", "Documents");
            if (!Directory.Exists(roothPath))
                Directory.CreateDirectory(roothPath);
            var filepath = Path.Combine(roothPath, vehicle.Vehicledocument.FileName);
            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                var document = new Vehicleowners
                {
                    Vehiclefile = vehicle.Vehicledocument.FileName,
                    Vehiclefilecontent = vehicle.Vehicledocument.ContentType,
                    Vehiclefilesize = vehicle.Vehicledocument.Length
                };
                await vehicle.Vehicledocument.CopyToAsync(stream);
            }

            var filepath2 = Path.Combine(roothPath, vehicle.purchaseticket.FileName);
            using (var stream = new FileStream(filepath2, FileMode.Create))
            {
                var document = new Vehicleowners
                {
                    Purchaserecieptfile = vehicle.purchaseticket.FileName,
                    Purchaserecieptfilecontent = vehicle.purchaseticket.ContentType,
                    Purchaserecieptfilesize = vehicle.purchaseticket.Length
                };
                await vehicle.purchaseticket.CopyToAsync(stream);
            }

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("customerform", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Vehilefile", vehicle.Vehicledocument.FileName);
                    cmd.Parameters.AddWithValue("@Vehiclefilecontent",vehicle.Vehicledocument.ContentType );
                    cmd.Parameters.AddWithValue("@Vehiclefilesize", vehicle.Vehicledocument.Length);
                    cmd.Parameters.AddWithValue("@Purchaserecieptfile", vehicle.purchaseticket.FileName);
                    cmd.Parameters.AddWithValue("@Purchaserecieptfilecontent", vehicle.purchaseticket.ContentType);
                    cmd.Parameters.AddWithValue("@Purchaserecieptfilesize", vehicle.purchaseticket.Length);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                ViewBag.status = "Submitted Successfully";
            }


            return View();
        }

        public IActionResult Fuelstationowner()
        {
            return View ();
        }

        [HttpPost]
        public IActionResult Fuelstationowner(Fuelstation station)
        {

            try 
            {
                if(ModelState.IsValid)
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        SqlCommand cmd2 = new SqlCommand("InsertFuelstationcomplains", con);
                        cmd2.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@businessname", station.businessname);
                        cmd2.Parameters.AddWithValue("@suppliername", station.suppliername);
                        cmd2.Parameters.AddWithValue("@supplierAddress", station.supplierAddress);
                        cmd2.Parameters.AddWithValue("@AmountSupplied", station.AmountSupplied);
                        cmd2.Parameters.AddWithValue("@CostofSupply", station.costofsupply);
                        cmd2.Parameters.AddWithValue("@datesupplied", station.datesupplied);
                        cmd2.ExecuteNonQuery();
                        ViewBag.status = "Submitted Successfully";

                    }
                }
               
            }
            catch(Exception ex)
            {
                ViewBag.ex = ex;
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
