using GameStore.Api.Entities;

const string GetGameEndpointName = "GetGame";

List <Game> games = new ()
{
  new Game() 
  {
    id = 1,
    Name = "StreetFighter II",
    Genre = "Fighting",
    Price = 19.99M,
    ReleaseDate = new DateTime(1991, 2, 1),
    ImageUri = "https://placehold.co/100"
  },
  new Game() 
  {
    id = 2,
    Name = "Final Fantasy XIV",
    Genre = "RPG",
    Price = 59.99M,
    ReleaseDate = new DateTime(2010, 9, 3),
    ImageUri = "https://placehold.co/100"
  },
  new Game() 
  {
    id = 3,
    Name = "FIFA 23",
    Genre = "Sports",
    Price = 69.99M,
    ReleaseDate = new DateTime(2022, 9, 27),
    ImageUri = "https://placehold.co/100"
  },
};

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var group = app.MapGroup("/games").WithParameterValidation();

group.MapGet("/", () => games);

group.MapGet("/{id}", (int id) => 
{
  Game? game = games.FirstOrDefault(g => g.id == id);

  if (game is null)
  {
    return Results.NotFound();
  }

  return Results.Ok(game);
})
.WithName(GetGameEndpointName);

group.MapPost("/", (Game game) => 
{
  game.id = games.Max(game => game.id + 1);
  games.Add(game);

  return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.id}, game);
});

group.MapPut("/{id}", (int id, Game updatedGame) => 
{
  Game? existingGame = games.FirstOrDefault(g => g.id == id);

  if (existingGame is null)
  {
    return Results.NotFound();
  }

  existingGame.Name = updatedGame.Name;
  existingGame.Genre = updatedGame.Genre;
  existingGame.Price = updatedGame.Price;
  existingGame.ReleaseDate = updatedGame.ReleaseDate;
  existingGame.ImageUri = updatedGame.ImageUri;

  return Results.NoContent();

});

group.MapDelete("/{id}", (int id) => {
  Game? existingGame = games.FirstOrDefault(g => g.id == id);

  if (existingGame is not null)
  {
    games.Remove(existingGame);
  }

  return Results.NoContent();

});

app.Run();
