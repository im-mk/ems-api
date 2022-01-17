using System;

namespace EMS.Domain.Db
{
    public class Document
    {
        public Guid DocumentId { get; set; }
        public string Title { get; set; }
        public string Path { get; set; }
        public string Comments { get; set; }
        public DateTimeOffset DateUploaded { get; set; }
    }
}