namespace JobPortal.Models.ViewModels
{
    public class CategoriesVM
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string? CategoryHOD { get; set; }


        public List<CategoriesVM> CategoriesList { get; set; }

        public CategoriesVM()
        {
            CategoriesList = new List <CategoriesVM>();
        }
    }
}
