using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NForm.Validations
{
  public abstract class Validator
  {
    public Validator(object target, string propertyName, string message)
    {
      this.PropertyName = propertyName;
      this.ErrorMessage = message;
      this.Target = target;
    }

    public string PropertyName { get; private set; }
    public string ErrorMessage { get; set; }

    public abstract bool IsValid(object value);
    public object Target { get; private set; }
  }
}