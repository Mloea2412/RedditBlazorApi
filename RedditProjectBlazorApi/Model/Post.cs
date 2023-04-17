using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace RedditProjectBlazorApi.Model
{
    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public User User { get; set; }
        public string Text { get; set; }
        public int Downvote { get; set; }
        public int Upvote { get; set; }
        public int NumberOfVotes { get; set; }

        public DateTime PostTime { get; set; }
        public List<Comment> Comments { get; set; } = new List<Comment>();


        public Post(string title, User user, string text, int upvote, int downvote, int numberOfVotes, DateTime postTime)
        {

            this.Title = title;
            this.User = user;
            this.Text = text;
            this.Upvote = upvote;
            this.Downvote = downvote;
            this.NumberOfVotes = numberOfVotes;
            this.PostTime = postTime;

        }

        // Der er lavet to constructors da der ellers kan opstå konflikter mellem datatyper
        public Post()
        {
            PostId = 0;
            Title = "";
            User = null;
            Text = "";
            Upvote = 0;
            Downvote = 0;
            PostTime = DateTime.Now;

        }

    }
}


