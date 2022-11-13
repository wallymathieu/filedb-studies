namespace SomeBasicFileStoreApp
open System
open Newtonsoft.Json
open Newtonsoft.Json.Serialization

module Json=
    type ShortNameSerializationBinder(type':Type)=
        let types = type'.Assembly.GetTypes() 
                    |> Array.filter(type'.IsAssignableFrom)
                    |> Array.map(fun t->(t.Name,t))
                    |> Map.ofArray

        interface ISerializationBinder with
            member this.BindToName(serializedType, assemblyName, typeName)=
                if type'.IsAssignableFrom(serializedType) then
                    assemblyName <- null
                    typeName <- serializedType.Name
                    ()
                else
                    assemblyName <- serializedType.Assembly.FullName
                    typeName <- serializedType.FullName
                    ()
    
            member this.BindToType(assemblyName, typeName)=
                if (String.IsNullOrEmpty(assemblyName) && types.ContainsKey(typeName)) then
                    types.[typeName]
                else
                    Type.GetType(String.Format("{0}, {1}", typeName, assemblyName), true)
    
    module Commands=
        let private settings = JsonSerializerSettings(
                                  TypeNameHandling = TypeNameHandling.Auto,
                                  SerializationBinder = ShortNameSerializationBinder(typeof<Command>))

        let deserialize v= JsonConvert.DeserializeObject<Command array>(v, settings);
    
        let serialize (obj:Command array)= JsonConvert.SerializeObject(obj, settings);
