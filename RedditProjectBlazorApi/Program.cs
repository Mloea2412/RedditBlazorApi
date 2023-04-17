using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using RedditProjectBlazorApi;
using RedditProjectBlazorApi.Model;

var builder = WebApplication.CreateBuilder(args);

// Sætter CORS så API'en kan bruges fra andre domæner
var AllowSomeStuff = "_AllowSomeStuff";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowSomeStuff, builder => {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

// Tilføj DbContext factory som service.
builder.Services.AddDbContext<RedditContext>(options =>
  options.UseSqlite(builder.Configuration.GetConnectionString("ContextSQLite")));


// Tilføj DataService så den kan bruges i endpoints
builder.Services.AddScoped<DataService>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(AllowSomeStuff);

using (var scope = app.Services.CreateScope())
{
    var dataService = scope.ServiceProvider.GetRequiredService<DataService>();
    // dataService.SeedData(); Fylder data på, hvis databasen er tom. Ellers ikke.
}

using (var db = new RedditContext())
{

}

// Henter alle post

app.Use(async (context, next) =>
{
    context.Response.ContentType = "application/json; charset=utf-8";
    await next(context);
});

app.MapGet("/all/posts", (DataService service) =>
{
    return service.GetAllPosts();
});

// Henter post på dets id
app.MapGet("/post/{postid}", (DataService service, int postid) =>
{
    return service.GetPostById(postid);
});

// Henter alle kommentarer
app.MapGet("/all/comments", (DataService service) =>
{
    return service.GetAllComments();
});

// Henter en kommmentar på dets id
app.MapGet("/comment/{commentid}", (DataService service, int commentid) =>
{
    return service.GetCommentById(commentid);
});

// Henter user på bruger id
app.MapGet("/user/{userid}", (DataService service, int userid) =>
{
    return service.GetUserById(userid);
});

// Henter alle brugere
app.MapGet("/all/users", (DataService service) =>
{
    return service.GetAllUsers();
});

// Laver et post
app.MapPost("/createpost", (DataService service, PostDTO data) =>
{
    return service.CreatePost(data.title, data.user, data.text, data.upvote, data.downvote, data.numberOfVotes, data.postTime);
});

// Laver en kommentar
app.MapPost("/createcomment", (DataService service, CommentDTO data) =>
{
    return service.CreateComment(data.text, data.downvote, data.upvote, data.numberOfVotes, data.postid, data.user, data.commentTime);

});

// Laver en vote på post
app.MapPut("/PostVote", (DataService service, PostVoteDTO data) =>

{
    return service.PostVoting(data.postId, data.user, data.UpvoteOrDownvote);

});

// Laver en vote på comment
app.MapPut("/CommentVote", (DataService service, CommentVoteDTO data) =>

{
    return service.CommentVoting(data.commentId, data.user, data.UpvoteOrDownvote);

});

app.Run();

record PostDTO(string title, User user, string text, int upvote, int downvote, int numberOfVotes, DateTime postTime);
record CommentDTO(string text, int downvote, int upvote, int numberOfVotes, int postid, User user, DateTime commentTime);
record PostVoteDTO(int postId, User user, bool UpvoteOrDownvote);
record CommentVoteDTO(int commentId, User user, bool UpvoteOrDownvote);


