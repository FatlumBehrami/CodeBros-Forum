using CodeBrosForum.Data;
using CodeBrosForum.Data.Models;
using CodeBrosForum.Models.Forum;
using CodeBrosForum.Models.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeBros_Forum.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;

        public ForumController(IForum forumService, IPost postService)
        {
            _forumService = forumService;
            _postService = postService;
        }

        public IActionResult Index()
        {
            IEnumerable<ForumListingModel> forums = _forumService.GetAll().Select(forum => new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageURL
            });

            ForumIndexModel model = new ForumIndexModel
            {
                ForumList = forums.OrderBy(forum => forum.Name)
            };

            return View(model);
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);
            var posts = new List<Post>();

            if (!String.IsNullOrEmpty(searchQuery))
            {
                posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();
            }
            else
            {
                posts = forum.Posts.ToList();
            }

            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                DatePosted = post.Created,
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post)
            });

            ForumTopicModel model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new AddForumModel();
            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var forum = _forumService.GetById(id);
            if (forum == null)
            {
                return View("Error");
            }

            var model = new EditForumModel()
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                LUD = forum.LUD,
                LUN = forum.LUN
            };

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var forum = _forumService.GetById(id);
            if(forum == null)
            {
                return View("Error");
            }

            var model = new DeleteForumModel()
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description
            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteForum(int id)
        {
            await _forumService.Delete(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditForum(EditForumModel model)
        {
            var forum = _forumService.GetById(model.Id);
            var lud = DateTime.Now;
            int lun = ++model.LUN;
            if(!(forum.Title == model.Title))
            {
                await _forumService.UpdateForumTitle(model.Id, model.Title, lud,lun);
            }
            if(!(forum.Description == model.Description))
            {
                await _forumService.UpdateForumDescription(model.Id, model.Description, lud, lun);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUrl = "/images/default.png";
            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageURL = imageUrl
            };
            await _forumService.Create(forum);
            return RedirectToAction("Index", "Forum");
        }



        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;
            return BuildForumListing(forum);

        }

        private ForumListingModel BuildForumListing(Forum forum)
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageURL
            };

        }


    }
}