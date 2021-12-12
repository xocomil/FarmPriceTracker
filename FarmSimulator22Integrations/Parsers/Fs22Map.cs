using System.Xml.Serialization;

namespace FarmSimulator22Integrations.Parsers;

public class Fs22Map {
  private Fs22Map(Map map) {
    Map = map;
  }

  public Map Map { get; }

  public static Fs22Map CreateInstance() {
    var serializer = new XmlSerializer(typeof(Map));

    using Stream reader = new FileStream(
      "C:\\SteamLibrary\\steamapps\\common\\Farming Simulator 22\\data\\maps\\maps_fillTypes.xml",
      FileMode.Open
    );

    Map map = serializer.Deserialize(reader) as Map ?? throw new InvalidOperationException();

    return new Fs22Map(map);
  }
}
