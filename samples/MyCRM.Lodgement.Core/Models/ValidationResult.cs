using System.Collections.Generic;

namespace MyCRM.Lodgement.Common.Models
{
    public class ValidationResult
    {
        public string ReferenceId { get; set; }
        public List<ValidationError> ValidationErrors { get; set; } = new List<ValidationError>();
    }
}
