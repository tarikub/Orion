namespace OrionEntities
{
    public class Media : IMedia
    {
        public string Id { get; set; }
        public string Link { get; set; }

        public string UploadedDate { get; set; }

        public string UploadedByUId { get; set; }

        public Media()
        {

        }
    }
}
