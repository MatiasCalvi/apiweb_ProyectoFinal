﻿using Datos.Modelos;
using Datos.Modelos.DTO;

namespace Datos.Interfaces.IDaos
{
    public interface IDaoBDUsuarios
    {
        Task<UsuarioSalida?> ObtenerUsuarioPorID(int pId);
        Task<UsuarioModif?> ObtenerUsuarioPorIDU(int pId);
        Task<UsuarioSalida> ObtenerUsuarioPorEmail(string pEmail);
        Task<UsuarioModif> ObtenerUsuarioPorEmailU(string pEmail);
        Task<UsuarioSalidaC> CrearNuevoUsuario(UsuarioCreacion pUserInput);
        Task<bool> ActualizarUsuario(int pId, UsuarioModif pUsuarioModif);
    }
}
