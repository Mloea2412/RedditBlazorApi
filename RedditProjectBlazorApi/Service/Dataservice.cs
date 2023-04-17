using System;
using RedditProjectBlazorApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace RedditProjectBlazorApi
{
    public class DataService
    {
        private RedditContext db { get; }

        public DataService(RedditContext db)
        {
            this.db = db;
        }

        public List<Post> GetAllPosts()
        {
            {
                return db.Posts.Include(p => p.User).Include(p => p.Comments).ToList();
            }
        }

        public List<Comment> GetAllComments()
        {
            return db.Comments.ToList();
        }

        // Henter post på dets id
        public Post GetPostById(int postid)
        {
            return db.Posts.Where(p => p.PostId == postid)
            .Include(p => p.Comments).FirstOrDefault()!;
        }

        // Henter kommentaren på dets id
        public Comment GetCommentById(int commentid)
        {
            return db.Comments.Where(p => p.CommentId == commentid).FirstOrDefault()!;
        }

        // Henter bruger på dets id
        public User GetUserById(int userid)
        {
            return db.Users.Where(p => p.UserId == userid).FirstOrDefault()!;
        }

        // Henter alle brugere
        public List<User> GetAllUsers()
        {
            return db.Users.ToList();
        }

        // Create post
        public string CreatePost(string title, User user, string text, int upvote, int downvote, int numberOfVotes, DateTime postTime)
        {

            User tempuser = db.Users.FirstOrDefault(a => a.UserId == user.UserId)!;
            if (tempuser == null)
            {
                db.Posts.Add(new Post(title, user, text, upvote, downvote, numberOfVotes, DateTime.Now));
            }
            else
            {
                db.Posts.Add(new Post(title, tempuser, text, upvote, downvote, numberOfVotes, DateTime.Now));
            }
            db.SaveChanges();
            return "Post created";
        }

        // Create comment
        public string CreateComment(string text, int upvote, int downvote, int numberOfVotes, int postid, User user, DateTime CommentTime)
        {
            var post = db.Posts.Where(p => p.PostId == postid).FirstOrDefault();
            if (post == null)
            {
                return "Post ikke fundet";
            }

            post.Comments.Add(new Comment(text, downvote, upvote, numberOfVotes, user, DateTime.Now));
            db.SaveChanges();
            return "Comment created";
        }

        public bool PostVoting(int postId, User user, bool UpvoteOrDownvote)
        {
            {
                // Find indlægget med det givne postId
                var post = db.Posts.FirstOrDefault(p => p.PostId == postId);
                if (post == null)
                {

                    return false;
                }

                // Hvis UpvoteOrDownvote er sat som true upvote

                if (UpvoteOrDownvote == true)
                {
                    post.Upvote++;
                    post.NumberOfVotes++;
                    db.SaveChanges();

                    return true;

                    // Hvis UpvoteOrDownvote er sat som false downvote
                }
                else if (UpvoteOrDownvote == false)
                {
                    post.Downvote--;
                    post.NumberOfVotes++;

                    //post.UserVotes.Remove(tempUser);
                    db.SaveChanges();
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool CommentVoting(int commentId, User user, bool UpvoteOrDownvote)
        {
            {
                // Find indlægget med det givne postId
                var comment = db.Comments.FirstOrDefault(p => p.CommentId == commentId);
                if (comment == null)
                {
                    return false;
                }

                // Hvis UpvoteOrDownvote er sat som true upvote

                if (UpvoteOrDownvote == true)
                {
                    comment.Upvote++;
                    comment.NumberOfVotes++;
                    db.SaveChanges();

                    return true;

                    // Hvis UpvoteOrDownvote er sat som false downvote
                }
                else if (UpvoteOrDownvote == false)
                {
                    comment.Downvote--;
                    comment.NumberOfVotes++;
                    db.SaveChanges();
                    return false;
                }
                else
                {
                    return false;
                }
            }
        }

        public void SeedData()

        {
            Post post = db.Posts.FirstOrDefault()!;
            if (post == null)
            {
                User user1 = new User("Boes");
                post = new Post { PostId = 1, Title = "Basement åbningstider?", User = user1, Text = "Hvornår har basement åben?", Downvote = 0, Upvote = 10, NumberOfVotes = 10 };
                db.Add(post);
                db.SaveChanges();
            }
            Comment comment = db.Comments.FirstOrDefault()!;
            if (comment == null)
            {
                comment = new Comment { CommentId = 1, Text = "Den har åben Torsdag og fredag fra kl 12", Downvote = 4, Upvote = 5, NumberOfVotes = 9 };
                db.Add(comment);
                db.SaveChanges();
            }
            User user = db.Users.FirstOrDefault()!;
            if (user == null)
            {
                User user2 = new User("Mads");
                User user3 = new User("ML");
                User user4 = new User("Rasmus");
                db.Add(user2);
                db.Add(user3);
                db.Add(user4);
                db.SaveChanges();
            }
        }
    }
}

