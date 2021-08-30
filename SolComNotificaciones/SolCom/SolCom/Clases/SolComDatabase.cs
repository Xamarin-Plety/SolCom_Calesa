using App1.Clases;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolCom.Clases
{
    public class SolComDatabase
    {

        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public SolComDatabase()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            try
            {
                if (!initialized)
                {
                    if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(Usuario).Name))
                    {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(Usuario)).ConfigureAwait(false);
                    }
                    if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(UsuarioJerarquia).Name))
                    {
                        await Database.CreateTablesAsync(CreateFlags.None, typeof(UsuarioJerarquia)).ConfigureAwait(false);
                    }
                    if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(AccionSolicitud).Name))
                    {
                        await Database.CreateTablesAsync(CreateFlags.None, typeof(AccionSolicitud)).ConfigureAwait(false);
                    }

                    initialized = true;
                }
            }
            catch (Exception ex)
            {
                string serror = ex.Message;
            }
        }

        public Task<Usuario> GetUsuarioIDAsync(int id)
        {
            return Database.Table<Usuario>().Where(i => i.iIdUsuario == id).FirstOrDefaultAsync();
        }

        public Task<List<Usuario>> GetUserAsync()
        {
            return Database.Table<Usuario>().ToListAsync();
        }

        public Task<List<AccionSolicitud>> GetSolicitudesAsync()
        {
            return Database.Table<AccionSolicitud>().ToListAsync();
        }

        public Task<List<UsuarioJerarquia>> GetUserJerarquiaAsync()
        {
            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(UsuarioJerarquia).Name))
            {
                 Database.CreateTablesAsync(CreateFlags.None, typeof(UsuarioJerarquia)).ConfigureAwait(false);
            }
            return Database.Table<UsuarioJerarquia>().ToListAsync();
        }

        public Task<int> SaveUserAsync(Usuario usuario)
        {
            var query = Database.Table<Usuario>().Where(s => s.iIdUsuario.Equals(usuario.iIdUsuario));

            var result = query.ToListAsync();

            if(result.Result.Count > 0)
            {
                return Database.UpdateAsync(usuario);
            }
            else
            {
            return Database.InsertAsync(usuario);
            }
        }

        public Task<int> SaveJerarquiaAsync(UsuarioJerarquia uJerarquia)
        {

            if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(UsuarioJerarquia).Name))
            {
                Database.CreateTablesAsync(CreateFlags.None, typeof(UsuarioJerarquia)).ConfigureAwait(false);
            }

            var query = Database.Table<UsuarioJerarquia>().Where(s => s.iIdUsuario.Equals(uJerarquia.iIdUsuario));

            var result = query.ToListAsync();

            if (result.Result.Count > 0)
            {
                return Database.UpdateAsync(uJerarquia);
            }
            else
            {
                return Database.InsertAsync(uJerarquia);
            }
        }

        public int GetIDAccionSolicitud(AccionSolicitud vAccion)
        {
            int iSolicitud = 0;
            var query = Database.Table<AccionSolicitud>().Where(s => s.iIdSolicitud.Equals(vAccion.iIdSolicitud) && s.iIdCentro.Equals(vAccion.iIdCentro));

            var result = query.ToListAsync();
            if (result.Result.Count > 0)
            {
                iSolicitud = result.Result[0].iId;
            }
            else
            {
               var nID = Database.QueryAsync<int>("SELECT MAX(iID) + 1 FROM [AccionSolicitud] ");
                iSolicitud = nID.Result[0];
            }

            return iSolicitud;
        }

        public Task<int> SaveAccionSolicitud(AccionSolicitud vAccion)
        {
            try
            {
                var query = Database.Table<AccionSolicitud>().Where(s => s.iIdSolicitud.Equals(vAccion.iIdSolicitud) && s.iIdCentro.Equals(vAccion.iIdCentro));

                var result = query.ToListAsync();
                if (result.Result.Count > 0)
                {
                    foreach (AccionSolicitud aSolicitud in result.Result)
                    {
                        Database.DeleteAsync(aSolicitud);
                    }
                }
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
            }

            return Database.InsertAsync(vAccion);
            
        }

        public Task<int> DeleteAccionAsync(AccionSolicitud vAccion)
        {
            return Database.DeleteAsync(vAccion);
        }

        public async void DeleteRow(AccionSolicitud vAccion)
        {
            await Database.DeleteAsync(vAccion);
        }

        public void DeleteAccionSolicitud()
        {
            Database.QueryAsync<AccionSolicitud>("Delete * FROM [AccionSolicitud]");
        }
    }

    public static class TaskExtensions
    {
        // NOTE: Async void is intentional here. This provides a way
        // to call an async method from the constructor while
        // communicating intent to fire and forget, and allow
        // handling of exceptions
        public static async void SafeFireAndForget(this Task task,
            bool returnToCallingContext,
            Action<Exception> onException = null)
        {
            try
            {
                await task.ConfigureAwait(returnToCallingContext);
            }

            // if the provided action is not null, catch and
            // pass the thrown exception
            catch (Exception ex) when (onException != null)
            {
                onException(ex);
            }
        }
    }
}
