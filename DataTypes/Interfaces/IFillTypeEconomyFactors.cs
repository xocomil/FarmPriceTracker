namespace DataTypes.Interfaces;

public interface IFillTypeEconomyFactors {
  string Period { get; }
  decimal Value { get; }
  bool ValueSpecified { get; }
}
