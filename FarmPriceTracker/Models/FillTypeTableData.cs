using System;
using System.Collections.Generic;
using System.Linq;
using DataTypes.Interfaces;
using FarmSimulator22Integrations.Models;

namespace FarmPriceTracker.Models;

internal record FillTypeTableData(string FillTypeName, decimal PricePerLiter) {
  private decimal BestFactor { get; set; }

  public decimal BestEasyPricePer1000 { get; set; }
  public decimal BestMediumPricePer1000 { get; set; }
  public decimal BestHardPricePer1000 { get; set; }
  public decimal SellAboveHard { get; private set; }
  public decimal SellAboveMedium { get; private set; }
  public decimal SellAboveEasy { get; private set; }
  public decimal BestPossibleHard { get; private set; }
  public decimal BestPossibleMedium { get; private set; }
  public decimal BestPossibleEasy { get; private set; }

  public decimal JanuaryFactor { get; private set; }
  public decimal FebruaryFactor { get; private set; }
  public decimal MarchFactor { get; private set; }
  public decimal AprilFactor { get; private set; }
  public decimal MayFactor { get; private set; }
  public decimal JuneFactor { get; private set; }
  public decimal JulyFactor { get; private set; }
  public decimal AugustFactor { get; private set; }
  public decimal SeptemberFactor { get; private set; }
  public decimal OctoberFactor { get; private set; }
  public decimal NovemberFactor { get; private set; }
  public decimal DecemberFactor { get; private set; }

  public static List<FillTypeTableData> CreateTableFillTypes(List<Fs22FillTypePriceData>? priceData) {
    return priceData
             ?.Where(fillType => fillType.Economy.PricePerLiter > 0)
             .Select(
               fillType =>
                 new FillTypeTableData(fillType.Name, fillType.Economy.PricePerLiter).AddMonthFactors(fillType)
             )
             .ToList() ??
           new List<FillTypeTableData>();
  }

  private FillTypeTableData AddMonthFactors(IFillTypePriceData? fillType) {
    if ( fillType?.Economy?.Factors is not null ) {
      foreach ( (string period, decimal value) in fillType.Economy?.Factors.Select(
                 factor => (factor.Period, factor.Value)
               ) ) {
        if ( value > BestFactor ) {
          BestFactor = value;

          BestHardPricePer1000 = 1000 * value * PricePerLiter;
          BestMediumPricePer1000 = BestHardPricePer1000 * 1.8m;
          BestEasyPricePer1000 = BestHardPricePer1000 * 3;

          BestPossibleHard = BestHardPricePer1000 * 1.25m;
          BestPossibleMedium = BestMediumPricePer1000 * 1.25m;
          BestPossibleEasy = BestEasyPricePer1000 * 1.25m;

          SellAboveHard = BestHardPricePer1000 * 0.9m;
          SellAboveMedium = BestMediumPricePer1000 * 0.9m;
          SellAboveEasy = BestEasyPricePer1000 * 0.9m;
        }

        switch (period) {
          case "1":
            JanuaryFactor = value;
            break;
          case "2":
            FebruaryFactor = value;
            break;
          case "3":
            MarchFactor = value;
            break;
          case "4":
            AprilFactor = value;
            break;
          case "5":
            MayFactor = value;
            break;
          case "6":
            JuneFactor = value;
            break;
          case "7":
            JulyFactor = value;
            break;
          case "8":
            AugustFactor = value;
            break;
          case "9":
            SeptemberFactor = value;
            break;
          case "10":
            OctoberFactor = value;
            break;
          case "11":
            NovemberFactor = value;
            break;
          case "12":
            DecemberFactor = value;
            break;
        }
      }
    }

    return this;
  }
}
