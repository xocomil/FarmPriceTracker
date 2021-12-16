namespace DataTypes.Interfaces;

public interface IFillTypePriceData {
  string Image { get; }
  IPallet Pallet { get; }
  IFillTypeEconomy Economy { get; }
  string Name { get; }
  string Title { get; }
  string AchievementName { get; }
  string ShowOnPriceTable { get; }
  string UnitShort { get; }
}
