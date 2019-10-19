using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace OrionEntities
{
    public class User : TableEntity
    {
        public string Id { set; get; }
        public UserStatus Status { set; get; }

        public string PhoneNumber { get; set; }

        public string PIN { get; set; }

        public string RokuUID { get; set; }
        public User()
        {
            RowKey = Guid.NewGuid().ToString();
            PartitionKey = nameof(User);
        }
    }
}
