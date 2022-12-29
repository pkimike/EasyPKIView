// See https://aka.ms/new-console-template for more information

using EasyPKIView;
using Newtonsoft.Json;

try {
    Console.WriteLine("Reading from Active Directory...");
    var pkServices = PublicKeyServices.GetFromActiveDirectory();
    Console.WriteLine("Serializing result...");
    String json = JsonConvert.SerializeObject(pkServices, Formatting.Indented);
    File.WriteAllText("pks.json",json);
    Console.WriteLine("Attempting to de-serialize...");
    var deserialized = JsonConvert.DeserializeObject<PublicKeyServices>(json);
    Console.WriteLine("Success");
} catch (Exception ex) {
    Console.WriteLine(ex.ToString());
}
