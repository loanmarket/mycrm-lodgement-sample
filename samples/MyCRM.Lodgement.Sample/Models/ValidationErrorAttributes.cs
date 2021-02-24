using System.Collections.Generic;

namespace MyCRM.Lodgement.Server.Models
{
    public class ValidationErrorAttributes 
    {
        public List<object> Ids { get; set; } = new List<object>();
        public ErrorMessage ErrorMessage { get; set; } = new ErrorMessage();
    }
}