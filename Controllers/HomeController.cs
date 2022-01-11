using CodeBrosForum.Data;
using CodeBrosForum.Data.Models;
using CodeBrosForum.Models;
using CodeBrosForum.Models.Forum;
using CodeBrosForum.Models.Home;
using CodeBrosForum.Models.Posts;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Linq;

namespace CodeBrosForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPost _postService;


        public HomeController(IPost postService)
        {
            _postService = postService;
        }
        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestPosts = _postService.GetLatestPosts(10);

            var posts = latestPosts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created,
                RepliesCount = post.Replies.Count(),
                Forum = GetForumListingForPost(post)
            });

            return new HomeIndexModel
            {
                LatestPosts = posts
            };
        }

        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;
            return new ForumListingModel
            {
                Name = forum.Title,
                ImageUrl = forum.ImageURL,
                Id = forum.Id
            };
        }

    }
}
