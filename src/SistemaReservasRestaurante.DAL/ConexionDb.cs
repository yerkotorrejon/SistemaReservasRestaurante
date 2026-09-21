using Microsoft.Data.SqlClient;

namespace SistemaReservasRestaurante.DAL
{
    // Punto único de acceso a la cadena de conexión.
    // Ninguna otra clase del sistema abre una conexión directa fuera de aquí.
    public static class ConexionDb
    {
        public static string ConnectionString { get; set; } =
            @"Server=.\SQLEXPRESS;Initial Catalog=ReservasRestauranteDB;Integrated Security=True;TrustServerCertificate=True;";

        public static SqlConnection ObtenerConexion() => new SqlConnection(ConnectionString);
    }
}
