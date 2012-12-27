using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NForm.Validations;

namespace NFormSample.Models
{
  public class Car : IValidatable<Car>
  {
    public int Id { get; set; }
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public int Year { get; set; }

    public ValidationSet<Car> Validations
    {
      get
      {
        var validate = new ValidationSet<Car>(this);
        validate.Presence(c => c.Model);
        validate.Presence(c => c.Manufacturer);
        validate.Format(c => c.Year, @"\d{4}");
        return validate;
      }
    }
  }
}
