using System.ComponentModel.DataAnnotations;

namespace JobPortal.Models.EntityModels
{
    public class Categories
    {
        [Key]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryHOD { get; set; }
   
    }
}
