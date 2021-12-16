using System.Collections.ObjectModel;
using System.Xml.Serialization;
using DataTypes.Interfaces;
using FarmSimulator22Integrations.Models;

namespace FarmSimulator22Integrations.Parsers;

public class Fs22Map {
  private const string MapsFillTypesXmlLocation = @"maps\maps_fillTypes.xml";

  private Fs22Map(Map? map) {
    Map = map;
  }

  private Map? Map { get; }

  public Collection<IFillTypePriceData>? FillTypes =>
    (Map?.FillTypes.Select(Fs22FillTypePriceData.CreateFs22FillTypePriceDataFromMap) ??
     Enumerable.Empty<IFillTypePriceData>()) as Collection<IFillTypePriceData>;

  public static Fs22Map CreateInstance(string dataDirectoryPath) {
    string fileLocation = Path.Combine(dataDirectoryPath, MapsFillTypesXmlLocation);

    if ( !File.Exists(fileLocation) ) {
      return new Fs22Map(null);
    }

    var serializer = new XmlSerializer(typeof(Map));

    using Stream reader = new FileStream(fileLocation, FileMode.Open);

    Map? map = serializer.Deserialize(reader) as Map ?? throw new InvalidOperationException();

    return new Fs22Map(map);
  }
}
