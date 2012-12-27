using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;
using NForm.Validations;

namespace NForm
{
  public static class NExtensions
  {
    public static NForm<T> FormFor<T>(this HtmlHelper helper, T model, string id, bool validate = false, bool renderWithDataList = true, object html = null)
      where T : IValidatable<T>
    {
      var dic = new RouteValueDictionary(html);
      if (validate) dic.Add("data-validate", "true");

      dic.Add("id", id);

      var htmlString = dic.Aggregate(new StringBuilder(), (buffer, kv) => buffer.AppendFormat("{0}=\"{1}\"", kv.Key, kv.Value));

      var w = helper.ViewContext.Writer;

      w.Write("<form {0}>", htmlString);
      if (renderWithDataList)
      {
        w.Write("<dl>");
      }
      
      return new NForm<T>(w, model, id, validate, renderWithDataList);
    }
  }
}
