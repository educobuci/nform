using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NForm.Validations
{
  public class ConfirmationValidator : Validator
  {
    public ConfirmationValidator(object target, string propertyName, string message = "confirmation doesn't match") : base(target, propertyName, message) { }

    public override bool IsValid(object value)
    {
      return true;
    }
  }
}