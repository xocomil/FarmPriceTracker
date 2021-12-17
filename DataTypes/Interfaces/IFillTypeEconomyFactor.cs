namespace DataTypes.Interfaces;

public interface IFillTypeEconomyFactor {
  string Period { get; }
  decimal Value { get; }
  bool ValueSpecified { get; }
}
