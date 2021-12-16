using System.Collections.ObjectModel;

namespace DataTypes.Interfaces;

public interface IFillTypeEconomy {
  Collection<IFillTypeEconomyFactors> Factors { get; }

  bool FactorsSpecified { get; }

  float PricePerLiter { get; }
  bool PricePerLiterSpecified { get; }
}
