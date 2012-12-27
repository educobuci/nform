using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NForm.Validations
{
  public class UniquenessValidator : Validator
  {
    private Func<object, bool> _checker;
    public UniquenessValidator(object target, string propertyName, Func<object, bool> checker)
      : base(target, propertyName, "is already taken")
    {
      this._checker = checker;
    }

    public override bool IsValid(object value)
    {
      return this._checker(this.Target);
    }
  }
}