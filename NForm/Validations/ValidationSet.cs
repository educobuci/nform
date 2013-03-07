using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Linq.Expressions;
using System.Text;
using System.Collections.Specialized;

namespace NForm.Validations
{
    public class ValidationSet<T>
    {
        #region Fields
        T _model;
        Dictionary<string, string> _errors;
        private List<Validator> validators;
        #endregion

        public ValidationSet(T model)
        {
            this._model = model;
            this.validators = new List<Validator>();
        }

        public Dictionary<string, string> Errors
        {
            get
            {
                this.RunValidations();
                return this._errors;
            }
        }

        public bool IsValid
        {
            get
            {
                return this.Errors.Count == 0;
            }
        }

        public bool IsInvalid
        {
            get
            {
                return !this.IsValid;
            }
        }

        public void Add(Validator validator)
        {
            this.validators.Add(validator);
        }

        public void Presence(Expression<Func<T, object>> expression, string message = "can't be blank")
        {
            this.validators.Add(new PresenceValidator(this._model, GetPropertyName(expression), message));
        }

        public void Format(Expression<Func<T, object>> expression, string with, string message = "is invalid")
        {
            this.validators.Add(new FormatValidator(this._model, GetPropertyName(expression), with, message));
        }

        public void Uniqueness(Expression<Func<T, object>> expression, Func<object, bool> checker)
        {
            this.validators.Add(new UniquenessValidator(this._model, GetPropertyName(expression), checker));
        }

        public void Confirmation(Expression<Func<T, object>> expression)
        {
            this.validators.Add(new ConfirmationValidator(this._model, GetPropertyName(expression)));
        }

        protected void RunValidations()
        {
            this._errors = new Dictionary<string, string>();
            foreach (var validator in this.validators)
            {
                if (!validator.IsValid(typeof(T).GetProperty(validator.PropertyName).GetValue(this._model, null)))
                {
                    this._errors.Add(validator.PropertyName, validator.ErrorMessage);
                }
            }
        }

        public string ToJson()
        {
            var properties = this.validators.Select(v => v.PropertyName).Distinct();
            var buffer = new StringBuilder();

            for (var i = 0; i < properties.Count(); i++)
            {
                var property = properties.ElementAt(i);

                buffer.AppendFormat("\"{0}\":{{", property);

                var validators = this.validators.Where(v => v.PropertyName == property);

                for (int j = 0; j < validators.Count(); j++)
                {
                    var validator = validators.ElementAt(j);
                    buffer.AppendFormat("\"{0}\":[{{\"message\":\"{1}\"", validator.GetType().Name.Replace("Validator", "").ToLower(), validator.ErrorMessage);

                    var validatorProps = validator.GetType().GetProperties();
                    for (int k = 0; k < validatorProps.Length - 3; k++)
                    {
                        var validatorProp = validatorProps[k];
                        // Regex condition
                        if (validatorProp.Name == "With")
                        {
                            buffer.AppendFormat(",\"{0}\":/{1}/", validatorProp.Name, validatorProp.GetValue(validator, null));
                        }
                        else
                        {
                            buffer.AppendFormat(",\"{0}\":\"{1}\"", validatorProp.Name, validatorProp.GetValue(validator, null));
                        }
                    }
                    buffer.Append("}]");

                    if (j + 1 < validators.Count())
                        buffer.Append(",");
                }

                buffer.Append("}");

                if (i + 1 < properties.Count())
                {
                    buffer.Append(",");
                }
            }

            return buffer.ToString();
        }

        private string GetPropertyName(LambdaExpression expression)
        {
            if (expression.Body is System.Linq.Expressions.MemberExpression)
            {
                return (expression.Body as System.Linq.Expressions.MemberExpression).Member.Name;
            }
            else
            {
                var methodCall = expression.Body as System.Linq.Expressions.UnaryExpression;
                return (methodCall.Operand as System.Linq.Expressions.MemberExpression).Member.Name;
            }
        }
    }
}