﻿using Datos.Modelos.DTO;

namespace Datos.Interfaces.IServicios
{
    public interface IAdminServicios
    {
        Task<List<UsuarioSalida>> ObtenerTodosLosUsuarios();
        Task<List<CarritoSalida>> ObtenerCarritos();
        Task<bool> VerificarUsuarioHabilitado(int pId);
        Task<bool> VerificarUsuarioDeshabilitado(int pId);
        Task<bool> HabilitarUsuario(int pId);
        Task<bool> DeshabilitarUsuario(int pId);
        Task<bool> AsignarRolAAdmin(int pId);
        Task<bool> AsignarRolAUsuario(int pId);
    }
}