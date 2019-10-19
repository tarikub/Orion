using Microsoft.WindowsAzure.Storage.Table;

namespace OrionEntities
{
    public class SlideShow : TableEntity, ISlideShow
    {

        public UserStatus Status { set; get; }

        public string PIN { get; set; }

        public string Phone { get; set; }

        public string Medias { get; set; }

        public SlideShow()
        {
            PartitionKey = nameof(SlideShow);
        }
    }
}
