namespace Framework.DomainModels.Base
{
    public class CategoryDomainModel : DomainModelBase
    {
        public Guid? UserId { get; set; }

        public Guid? ListId { get; set; }

        public string Bezeichnung { get; set; }

        public string RGB_A { get; set; }

        public string Icon { get; set; }


        public CategoryDomainModel()
        {
            Bezeichnung = string.Empty;
            RGB_A = "#ffffffff";
            Icon = string.Empty;
        }
    }
}
