using CodeBrosForum.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBrosForum.Models.Home
{
    public class HomeIndexModel
    {
        public IEnumerable<PostListingModel> LatestPosts { get; set; }
       
    }
}
