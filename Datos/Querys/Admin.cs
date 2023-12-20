﻿using Datos.Interfaces.IQuerys;

namespace Datos.Querys
{
    public class AdminQuerys : IAdminQuerys
    {
        public string obtenerUsuariosQuery { get; set; } = "SELECT * FROM usuarios";
        public string obtenerCarritosQuery { get; set; } = "SELECT c.Carrito_UsuarioID, c.Carrito_Estado, p.Public_ID as Carrito_PID, p.Public_UsuarioID, p.Public_Nombre, p.Public_Descripcion, p.Public_Precio, p.Public_Imagen, p.Public_Stock FROM carrito c INNER JOIN publicaciones p ON c.Carrito_PID = p.Public_ID";
        public string verificarUsuarioDeshabilitadoQuery { get; set; } = "SELECT 1 FROM usuarios WHERE Usuario_ID = @UsuarioId AND Usuario_Estado = 2";
        public string verificarUsuarioHabilitadoQuery { get; set; } = "SELECT 1 FROM usuarios WHERE Usuario_ID = @UsuarioId AND Usuario_Estado = 1";
        public string habilitarUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Estado = 1 WHERE Usuario_ID = @Usuario_ID";
        public string desactivarUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Estado = 2 WHERE Usuario_ID = @Usuario_ID";
        public string asignarRolAAdminQuery { get; set; } = "UPDATE usuarios SET Usuario_Role = 2 WHERE Usuario_ID = @Usuario_ID";
        public string asignarRolAUsuarioQuery { get; set; } = "UPDATE usuarios SET Usuario_Role = 1 WHERE Usuario_ID = @Usuario_ID";
    }
}
