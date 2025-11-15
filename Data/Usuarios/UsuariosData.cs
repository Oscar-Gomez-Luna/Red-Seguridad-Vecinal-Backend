using Backend_RSV.Models.Request;
using FirebaseAdmin.Auth;
using MiApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend_RSV.Data.Usuarios
{
    public class UsuariosData
    {
        private readonly AppDbContext _context;

        public UsuariosData(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> LoginAsync(string firebaseUID)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Persona)
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.FirebaseUID == firebaseUID && u.Activo);

            if (usuario != null)
            {
                usuario.UltimoAcceso = DateTime.Now;
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
            }

            return usuario;
        }
        public async Task<Usuario> RegistrarUsuarioAsync(Persona persona, Usuario usuario, CuentaUsuario cuentaUsuario)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Personas.Add(persona);
                await _context.SaveChangesAsync();

                usuario.PersonaID = persona.PersonaID;

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                cuentaUsuario.UsuarioID = usuario.UsuarioID;
                cuentaUsuario.UltimaActualizacion = DateTime.Now;

                cuentaUsuario.SaldoTotal = cuentaUsuario.SaldoMantenimiento + cuentaUsuario.SaldoServicios;

                _context.CuentaUsuario.Add(cuentaUsuario);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                await _context.Entry(usuario)
                    .Reference(u => u.Persona)
                    .LoadAsync();

                await _context.Entry(usuario)
                    .Reference(u => u.TipoUsuario)
                    .LoadAsync();

                await _context.Entry(usuario)
                    .Reference(u => u.CuentaUsuario)
                    .LoadAsync();

                return usuario;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
        public async Task<bool> UpdateUsuarioAsync(UsuarioUpdateRequest request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Persona)
                .Include(u => u.CuentaUsuario)
                .FirstOrDefaultAsync(u => u.UsuarioID == request.UsuarioID);

            if (usuario == null)
                return false;

            // --- Actualización en Firebase ---
            var updateArgs = new UserRecordArgs
            {
                Uid = usuario.FirebaseUID
            };

            bool actualizarFirebase = false;

            if (!string.IsNullOrEmpty(request.Email) && request.Email != usuario.Persona.Email)
            {
                updateArgs.Email = request.Email;
                actualizarFirebase = true;
            }

            if (!string.IsNullOrEmpty(request.Password))
            {
                updateArgs.Password = request.Password;
                actualizarFirebase = true;
            }

            if (actualizarFirebase)
            {
                await FirebaseAuth.DefaultInstance.UpdateUserAsync(updateArgs);
            }

            usuario.NumeroCasa = request.NumeroCasa;
            usuario.Calle = request.Calle;

            usuario.Persona.Nombre = request.Nombre;
            usuario.Persona.ApellidoPaterno = request.ApellidoPaterno;
            usuario.Persona.ApellidoMaterno = request.ApellidoMaterno;
            usuario.Persona.Telefono = request.Telefono;
            usuario.Persona.FechaNacimiento = request.FechaNacimiento;
            usuario.Persona.Email = request.Email;

            if (usuario.CuentaUsuario != null)
            {
                if (!string.IsNullOrEmpty(request.NumeroTarjeta))
                {
                    usuario.CuentaUsuario.NumeroTarjeta = System.Text.Encoding.UTF8.GetBytes(request.NumeroTarjeta);

                    string ultimos4 = request.NumeroTarjeta.Length >= 4
                        ? request.NumeroTarjeta[^4..]
                        : request.NumeroTarjeta;

                    usuario.CuentaUsuario.UltimosDigitos = ultimos4;
                }
                usuario.CuentaUsuario.FechaVencimiento = request.FechaVencimiento;
            }

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Usuario>> GetAllUsuariosAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Persona)
                .Include(u => u.TipoUsuario)
                .Include(u => u.CuentaUsuario)
                .ToListAsync();
        }

        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Persona)
                .Include(u => u.TipoUsuario)
                .Include(u => u.CuentaUsuario)
                .FirstOrDefaultAsync(u => u.UsuarioID == id);
        }
        public async Task<List<TipoUsuario>> GetTiposUsuarioAsync()
        {
            return await _context.TiposUsuario.ToListAsync();
        }
        public async Task<List<CuentaUsuario>> GetAllAsync()
        {
            return await _context.CuentaUsuario
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<CuentaUsuario?> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.CuentaUsuario
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.UsuarioID == usuarioId);
        }

    }
}