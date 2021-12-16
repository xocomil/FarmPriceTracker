using DataTypes.Interfaces;
using FarmSimulator22Integrations.Parsers;

namespace FarmSimulator22Integrations.Models;

public record Fs22FillTypeEconomy(IEnumerable<IFillTypeEconomyFactor> Factors, bool FactorsSpecified, decimal PricePerLiter, bool PricePerLiterSpecified) : IFillTypeEconomy {
  public static IFillTypeEconomy CreateEconomyMapEconomy(MapFillTypesFillTypeEconomy economy) =>
    new Fs22FillTypeEconomy(
      (economy.Factors ?? Enumerable.Empty<MapFillTypesFillTypeEconomyFactorsFactor>()).Select(Fs22EconomyFactor.CreateFactorFromMapFactor),
      economy.FactorsSpecified,
      new decimal(economy.PricePerLiter),
      economy.PricePerLiterSpecified
    );
}
