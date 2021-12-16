using DataTypes.Interfaces;
using FarmSimulator22Integrations.Parsers;

namespace FarmSimulator22Integrations.Models;

public record Fs22Pallet(string Filename) : IPallet {
  public static IPallet CreatePalletFromMapPallet(MapFillTypesFillTypePallet pallet) => new Fs22Pallet(
    pallet.Filename
  );
}
