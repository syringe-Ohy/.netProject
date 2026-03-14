using System;
using System.Web;
using System.Web.Http;
using CarSalesAndInventoryManagementSystem.Context;
using CarSalesAndInventoryManagementSystem.Models;

namespace CarSalesAndInventoryManagementSystem.Controllers.API
{
    public class CarRequestDto
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string EngineType { get; set; }
        public string Notes { get; set; }
    }

    public class CarRequestApiController : ApiController
    {
        private readonly CarSales_Inventory _dbContext;

        public CarRequestApiController()
        {
            _dbContext = new CarSales_Inventory();
        }

        // POST: api/CarRequestApi
        // This action now accepts the DTO instead of the full database model.
        [HttpPost]
        public IHttpActionResult PostCarRequest(CarRequestDto requestDto)
        {
            var session = HttpContext.Current.Session;
            if (session == null || session["UserID"] == null)
            {
                return Unauthorized();
            }

            // Basic check to ensure we have the most important data.
            if (requestDto == null || string.IsNullOrWhiteSpace(requestDto.Brand) || string.IsNullOrWhiteSpace(requestDto.Model))
            {
                return BadRequest("Brand and Model are required.");
            }

            try
            {
                // Create a NEW CarRequest database model object from the DTO.
                var newCarRequest = new CarRequest
                {
                    // Map data from the DTO
                    Brand = requestDto.Brand,
                    Model = requestDto.Model,
                    EngineType = requestDto.EngineType,
                    Notes = requestDto.Notes,

                    // Set server-side properties
                    UserID = (int)session["UserID"],
                    RequestDate = DateTime.Now,
                    Status = "New"
                };

                _dbContext.CarRequests.Add(newCarRequest);
                _dbContext.SaveChanges();

                return Ok(new { message = "Your request has been submitted successfully!" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}