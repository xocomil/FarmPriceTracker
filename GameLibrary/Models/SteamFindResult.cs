namespace GameLibrary.Models;

public record SteamFindResult {
  public string? GameLocation { get; init; }
  public string? ErrorMessage { get; init; }
  public bool Found { get; init; }
}
