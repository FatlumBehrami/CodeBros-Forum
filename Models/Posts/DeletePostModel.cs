namespace CodeBrosForum.Models.Posts
{
    public class DeletePostModel
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
