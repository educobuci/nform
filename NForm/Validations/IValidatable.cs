namespace NForm.Validations
{
  public interface IValidatable<T>
  {
    ValidationSet<T> Validations { get; }
  }
}