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
    Descripcion: Llamadas finales a la Base de datos a la tabla "Contacts"
*/

namespace Repository.Services
{
    public class ContactService : IContactService
    {
        private readonly dbContextApp db;
        private readonly IMapper mapper;
        private readonly ILogService logService;

        public ContactService(dbContextApp _db, IMapper _mapper, ILogService _logService)
        {
            db = _db;
            db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            mapper = _mapper;
            logService = _logService;
        }

        public async Task<List<ContactDTO>> GetAll()
        {
            try
            {
                var contacts = await db.Contacts.ToListAsync();
                if (contacts == null)
                {
                    return null;
                }

                return mapper.Map<List<Contact>, List<ContactDTO>>(contacts);
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "Repository",
                    FileName = "ContactService.cs",
                    MethodName = "GetAll",
                    Message = error.Message.ToString()
                });
                return null;                
            }
        }

        public async Task<ContactDTO> GetById(int id)
        {
            try
            {
                var contact = await db.Contacts.Where(x => x.Id == id).FirstOrDefaultAsync();
                if (contact == null)
                {
                    return null;
                }

                return mapper.Map<Contact, ContactDTO>(contact);
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "Repository",
                    FileName = "ContactService.cs",
                    MethodName = "GetById",
                    Message = error.Message.ToString()
                });
                return null;
            }
        }

        public async Task<ContactDTO> Save(ContactDTO model)
        {
            try
            {
                var contact = mapper.Map<ContactDTO, Contact>(model);
                db.Contacts.Add(contact);
                await db.SaveChangesAsync();

                return await GetById(contact.Id);
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "Repository",
                    FileName = "ContactService.cs",
                    MethodName = "Save",
                    Message = error.Message.ToString()
                });
                return null;
            }
        }

        public async Task<ContactDTO> Update(ContactDTO model, int id)
        {
            try
            {
                var contact = await GetById(id);
                if (contact == null)
                {
                    return null;
                }

                model.RegisterDate = DateTime.Now;

                var contactToUpdate = mapper.Map<ContactDTO, Contact>(model);
                db.Contacts.Update(contactToUpdate);
                await db.SaveChangesAsync();

                return mapper.Map<Contact, ContactDTO>(contactToUpdate);
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "Repository",
                    FileName = "ContactService.cs",
                    MethodName = "Update",
                    Message = error.Message.ToString()
                });
                return null;
            }
        }

        public async Task<ContactDTO> Delete(int id)
        {
            try
            {
                var contact = await GetById(id);
                if (contact == null)
                {
                    return null;
                }

                db.Contacts.Remove(contact);
                await db.SaveChangesAsync();

                return mapper.Map<Contact, ContactDTO>(contact);
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "Repository",
                    FileName = "ContactService.cs",
                    MethodName = "Delete",
                    Message = error.Message.ToString()
                });
                return null;
            }
        }
    }
}
