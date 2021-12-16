using System.Collections.ObjectModel;
using DataTypes.Interfaces;
using FarmSimulator22Integrations.Parsers;

namespace FarmSimulator22Integrations.Models;

public record Fs22EconomyFactor(string Period, decimal Value, bool ValueSpecified) : IFillTypeEconomyFactor {
  public static Fs22EconomyFactor CreateFactorFromMapFactor(MapFillTypesFillTypeEconomyFactorsFactor factor) =>
    new Fs22EconomyFactor(factor.Period, new decimal(factor.Value), factor.ValueSpecified);
}
