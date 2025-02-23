using System.Text.Json.Serialization;
using Flash.Configuration.WebApi_AOT.Models;

namespace Flash.Configuration.WebApi_AOT;

[JsonSerializable(typeof(Todo[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}