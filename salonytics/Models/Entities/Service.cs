using NuGet.Versioning;
using salonytics.Models;
using salonytics.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Salonytics.Models.Entities
{
  


    public class Service
    {
        public Guid ServiceId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int TotalServices { get;  set; }
        public int TimeSlot { get; set; }
        public float ReservationFee { get; set;  }
        public ICollection<StylistService> StylistServices { get; set; }
        public int NumberOfServicesAvailed { get; set; }
    }



}
