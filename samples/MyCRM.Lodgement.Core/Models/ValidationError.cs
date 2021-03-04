namespace MyCRM.Lodgement.Common.Models
{
    public class ValidationError
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsValid { get; set; }
        public string ErrorType { get; set; }
        public ValidationErrorAttributes Attributes { get; set; }
    }
}