using AssemblyScanning;
using GameFinder.StoreHandlers.Steam;
using GameLibrary.Models;
using Microsoft.Extensions.DependencyInjection;

namespace GameLibrary;

[Inject(serviceLifetime: ServiceLifetime.Transient, provideFor: typeof(SteamHelper))]
public class SteamHelper {
  private readonly Dictionary<int, string> _games = new() { { FarmingSimulator22SteamId, "Farming Simulator 22" } };
  private readonly SteamHandler _steamHandler;

  public SteamHelper(SteamHandler steamHandler) {
    _steamHandler = steamHandler;
  }

  public static int FarmingSimulator22SteamId => 1248130;

  public bool HasSteam => _steamHandler.FoundSteam;

  public SteamFindResult SteamInstallLocation(int steamId) {
    var errorResult =
      new SteamFindResult { Found = false, ErrorMessage = "An error happened communicating with Steam." };

    if ( !_steamHandler.FoundSteam ) {
      return errorResult with { ErrorMessage = "Steam is not installed." };
    }

    if ( !_steamHandler.FindAllGames() ) {
      return errorResult with { ErrorMessage = "Could not find any Steam games." };
    }

    _steamHandler.TryGetByID(steamId, out SteamGame? game);

    return game == null
      ? errorResult with { ErrorMessage = $"Could not find {_games[steamId]} in the Steam library!" }
      : new SteamFindResult { Found = true, GameLocation = game.Path };
  }
}
