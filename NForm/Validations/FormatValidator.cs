using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace NForm.Validations
{
  public class FormatValidator : Validator
  {
    public FormatValidator(object target, string propertyName, string with, string message)
      : base(target, propertyName, message)
    {
      this.With = with;
    }

    public override bool IsValid(object value)
    {
      return Regex.IsMatch(value.ToString(), this.With);
    }

    public string With { get; private set; }
  }
}