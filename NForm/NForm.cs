using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Mvc;
using NForm.Validations;
using System.Web.Routing;

namespace NForm
{
  public class NForm<T> : IDisposable where T:IValidatable<T>
  {
    private TextWriter _writer;
    private bool _disposed;
    private T _model;
    private bool _validate;
    private string _id;
    private bool _renderDL;

    public NForm(TextWriter writer, T model, string id, bool validate, bool renderDL)
    {
      this._writer = writer;
      this._model = model;
      this._validate = validate;
      this._id = id;
      this._renderDL = renderDL;
    }

    public void Dispose()
    {
      Dispose(true /* disposing */);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!_disposed)
      {
        _disposed = true;
        if (this._validate)
        {
          var temp = "window['" + this._id + "'] = {\"type\": \"ActionView::Helpers::FormBuilder\", \"input_tag\": \"<div class=\\\"field_with_errors\\\"><span id=\\\"input_tag\\\" /><label for=\\\"\\\" class=\\\"message\\\"></label></div>\",\"label_tag\": \"<div class=\\\"field_with_errors\\\"><label id=\\\"label_tag\\\" /></div>\", \"validators\": {";
          _writer.Write("<script>{0}{1}}} }};</script>", temp, this._model.Validations.ToJson());
        }
        if (this._renderDL)
        {
          _writer.Write("</dl>");
        }
        _writer.Write("</form>");
      }
    }

    public MvcHtmlString Input(string name, string label = null, bool? validate = null, object html = null)
    {
      var buffer = new StringBuilder();

      // Label
      if (this._renderDL) buffer.Append("<dt>");
      buffer.AppendFormat("<label for=\"{0}\">{1}</label>", name, label != null ? label : name.InflectTo().Titleized);
      if (this._renderDL) buffer.Append("</dt>");

      // Acutal input
      var value = ""; //_model == null ? "" : this._model.GetType().GetProperty(name).GetValue(this._model, null);

      var attributes = new Dictionary<string, string>();
      if ((!validate.HasValue && _validate) || (validate.HasValue && validate.Value))
      {
        attributes.Add("data-validate", "true");
      }

      attributes.Add("id", name);
      attributes.Add("name", name);
      attributes.Add("value", value.ToString());

      if (name.ToLower().Contains("password"))
      {
        attributes.Add("type", "password");
      }

      new RouteValueDictionary(html);

      var attrHtml = attributes.Aggregate(new StringBuilder(), (htmlBuffer, at) => htmlBuffer.AppendFormat("{0}=\"{1}\"", at.Key, at.Value));

      if (this._renderDL) buffer.Append("<dd>");
      buffer.AppendFormat("<input {0}/>", attrHtml);
      if (this._renderDL) buffer.Append("</dd>");

      return new MvcHtmlString(buffer.ToString());
    }
  }
}
