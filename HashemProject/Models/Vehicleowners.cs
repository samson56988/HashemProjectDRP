using Microsoft.AspNetCore.Http;

namespace HashemProject.Models
{
    public class Vehicleowners
    {
        public string Vehiclefile { get; set; }

        public string Vehiclefilecontent { get; set; }

        public long? Vehiclefilesize { get; set; }

        public string Purchaserecieptfile { get; set; }

        public string Purchaserecieptfilecontent { get; set; }

        public long? Purchaserecieptfilesize { get; set; }

        public IFormFile Vehicledocument { get; set; }

        public IFormFile purchaseticket { get; set; }


    }
}
