using System;

namespace CodeBrosForum.Models.Forum
{
    public class EditForumModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime LUD { get; set; }
        public int LUN { get; set; }
    }
}
