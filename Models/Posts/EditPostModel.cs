using System;

namespace CodeBrosForum.Models.Posts
{
    public class EditPostModel
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime LUD { get; set; }
        public int LUN { get; set; }
    }
}
