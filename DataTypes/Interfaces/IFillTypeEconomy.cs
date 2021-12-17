namespace DataTypes.Interfaces;

public interface IFillTypeEconomy {
  IEnumerable<IFillTypeEconomyFactor> Factors { get; }

  bool FactorsSpecified { get; }

  decimal PricePerLiter { get; }
  bool PricePerLiterSpecified { get; }
}
