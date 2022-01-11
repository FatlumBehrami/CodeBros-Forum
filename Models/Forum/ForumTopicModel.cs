using CodeBrosForum.Models.Posts;
using System.Collections.Generic;

namespace CodeBrosForum.Models.Forum
{
    public class ForumTopicModel
    {
        public ForumListingModel Forum { get; set; }
        public IEnumerable<PostListingModel> Posts { get; set; }
        public string SearchQuery { get; set; }
    }
}
