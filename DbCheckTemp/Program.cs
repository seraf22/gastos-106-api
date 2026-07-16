using Npgsql;

var cs = "Host=casa106-db-casa106.i.aivencloud.com;Port=16228;Database=defaultdb;Username=avnadmin;Password=TU_PASSWORD;SSL Mode=Require;Trust Server Certificate=true";
using var conn = new NpgsqlConnection(cs);
await conn.OpenAsync();

using var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM \"Propiedades\";", conn);
var count = await cmd.ExecuteScalarAsync();
Console.WriteLine($"Propiedades: {count}");
