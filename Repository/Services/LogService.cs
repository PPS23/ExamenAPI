using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Repository.DTO;
using Repository.Interfaces;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
    Version: 1.0
    Author: Pedro Pablo Soto
    Descripcion: Llamadas finales a la Base de datos a la tabla "Logs" en esta tabla solo se registraran errores que se generen en el catch
*/
namespace Repository.Services
{
    public class LogService : ILogService
    {
        private readonly dbContextApp db;
        private readonly IMapper mapper;

        public LogService(dbContextApp _db, IMapper _mapper)
        {
            db = _db;
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            mapper = _mapper;
        }

        public Task<LogDTO> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<LogDTO>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<LogDTO> GetById(int id)
        {
            try
            {
                var log = await db.Logs.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (log == null)
                {
                    return null;
                }

                return mapper.Map<Log, LogDTO>(log);
            }
            catch
            {
                return null;
            }
        }

        public async Task<LogDTO> Save(LogDTO model)
        {
            try
            {
                model.RegisterDate = DateTime.Now;
                var log = mapper.Map<LogDTO, Log>(model);
                db.Logs.Add(log);
                await db.SaveChangesAsync();

                return await GetById(log.Id);
            }
            catch
            {
                return null;
            }
        }

        public Task<LogDTO> Update(LogDTO model, int id)
        {
            throw new NotImplementedException();
        }
    }
}
