using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NForm.Validations
{
  public class PresenceValidator : Validator
  {
    public PresenceValidator(object target, string propertyName, string message) : base(target, propertyName, message) { }

    public override bool IsValid(object value)
    {
      return value != null && value.ToString().Trim() != String.Empty;
    }
  }
}