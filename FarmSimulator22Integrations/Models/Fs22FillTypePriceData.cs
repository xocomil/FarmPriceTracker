using DataTypes.Interfaces;
using FarmSimulator22Integrations.Parsers;

namespace FarmSimulator22Integrations.Models;

public record Fs22FillTypePriceData(string Image, IPallet Pallet, IFillTypeEconomy Economy, string Name, string Title, string AchievementName, string ShowOnPriceTable, string UnitShort) : IFillTypePriceData {
  public static Fs22FillTypePriceData CreateFs22FillTypePriceDataFromMap(MapFillTypesFillType fillType) => new(fillType.Image.Hud, Fs22Pallet.CreatePalletFromMapPallet(fillType.Pallet), Fs22FillTypeEconomy.CreateEconomyMapEconomy(fillType.Economy), fillType.Name, fillType.Title, fillType.AchievementName, fillType.ShowOnPriceTable, fillType.UnitShort );
}
