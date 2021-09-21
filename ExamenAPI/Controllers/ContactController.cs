using ExamenAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.DTO;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
/*
    Version: 1.0
    Author: Pedro Pablo Soto
    Description: Administración de "Contacts", intento validar lo mas que sea posible para que la información llegue lo mas limpia posible al Repository
*/
namespace ExamenAPI.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IContactService contactService;
        private readonly ILogService logService;
        public ContactController(IContactService _contactService, ILogService _logService)
        {
            contactService = _contactService;
            logService = _logService;
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            try
            {
                var contacts = await contactService.GetAll();
                if (contacts == null)
                {
                    return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "There's not contacts." });
                }

                return Ok(new ResponseDTO() { Status = ResponseStatus.Success, Message = "Success", Data = contacts });
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "APIWEB",
                    FileName = "ContactController.cs",
                    MethodName = "GetAll",
                    Message = error.Message.ToString()
                });
                return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Something goes wrong, please notify to the administrator." });
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<ActionResult> GetById([FromRoute] int id)
        {
            try
            {
                List<string> errors = new List<string>();

                if (id <= 0)
                {
                    errors.Add("Id is required.");
                }

                if (errors.Count > 0)
                {
                    return BadRequest(new { Errors = errors });
                }
                else
                {
                    var contacts = await contactService.GetById(id);
                    if (contacts == null)
                    {
                        return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "The contact not exists." });
                    }
                    return Ok(new ResponseDTO() { Status = ResponseStatus.Success, Message = "Success.", Data = contacts });
                }
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "APIWEB",
                    FileName = "ContactController.cs",
                    MethodName = "GetById",
                    Message = error.Message.ToString()
                });
                return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Something goes wrong, please notify to the administrator." });
            }
        }

        [HttpPost]
        [Route("Save")]
        public async Task<ActionResult> Save([FromBody] ContactDTO model)
        {
            try
            {
                List<string> errors = new List<string>();
                model.RegisterDate = DateTime.Now;

                //Name
                if (model.Name.Equals("") || model.Name == null)
                {
                    errors.Add("Name is required.");
                }
                if (model.Name.Length > 50)
                {
                    errors.Add("Name cannot be longer than 50 characters.");
                }
                if (!Regex.IsMatch(model.Name, @"^[a-z A-Z]+$"))
                {
                    errors.Add("Name only accept characters a-z.");
                }

                //Address
                if (model.Address.Equals("") || model.Address == null)
                {
                    errors.Add("Address is required.");
                }
                if (model.Address.Length > 150)
                {
                    errors.Add("Address cannot be longer than 150 characters.");
                }
                if (!Regex.IsMatch(model.Address, @"^[a-z A-Z 0-9]+$"))
                {
                    errors.Add("Address only accept characters A-Z and numbers 0-9.");
                }

                //Phone
                if (model.Phone.Equals("") || model.Phone == null)
                {
                    errors.Add("Phone is required.");
                }
                if (model.Phone.Length > 150)
                {
                    errors.Add("Phone cannot be longer than 15 characters.");
                }
                if (!Regex.IsMatch(model.Phone, @"^[0-9]+$"))
                {
                    errors.Add("Phone only accept numbers 0-9.");
                }

                //CURP
                if (model.CURP.Equals("") || model.CURP == null)
                {
                    errors.Add("CURP is required.");
                }
                if (model.CURP.Length > 20)
                {
                    errors.Add("CURP cannot be longer than 20 characters.");
                }
                if (!Regex.IsMatch(model.CURP, @"^[a-zA-Z0-9]+$"))
                {
                    errors.Add("CURP only accept characters A-Z and numbers 0-9.");
                }


                if (errors.Count == 0)
                {
                    model.Name.Trim();
                    model.Address.Trim();
                    model.Phone.Trim();
                    model.CURP.Trim();
                    var contactSaved = await contactService.Save(model);
                    if (contactSaved == null)
                    {
                        return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Contact was not saved." });
                    }

                    return Ok(new ResponseDTO() { Status = ResponseStatus.Success, Message = "Contact saved.", Data = contactSaved });
                }
                else
                {
                    return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Contact was not saved.", Data = errors });
                }
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "APIWEB",
                    FileName = "ContactController.cs",
                    MethodName = "Save",
                    Message = error.Message.ToString()
                });
                return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Something goes wrong, please notify to the administrator." });
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<ActionResult> Update([FromBody] ContactDTO model)
        {
            try
            {
                List<string> errors = new List<string>();

                if (model.Id <= 0)
                {
                    errors.Add("Id is required. ");
                }

                //Name
                if (model.Name.Equals("") || model.Name == null)
                {
                    errors.Add("Name is required. ");
                }
                if (model.Name.Length > 50)
                {
                    errors.Add("Name cannot be longer than 50 characters. ");
                }
                if (!Regex.IsMatch(model.Name, @"^[a-z A-Z]+$"))
                {
                    errors.Add("Name only accept characters a-z. ");
                }

                //Address
                if (model.Address.Equals("") || model.Address == null)
                {
                    errors.Add("Address is required. ");
                }
                if (model.Address.Length > 150)
                {
                    errors.Add("Address cannot be longer than 150 characters. ");
                }
                if (!Regex.IsMatch(model.Address, @"^[a-z A-Z 0-9]+$"))
                {
                    errors.Add("Address only accept characters A-Z and numbers 0-9. ");
                }

                //Phone
                if (model.Phone.Equals("") || model.Phone == null)
                {
                    errors.Add("Phone is required. ");
                }
                if (model.Phone.Length > 150)
                {
                    errors.Add("Phone cannot be longer than 15 characters. ");
                }
                if (!Regex.IsMatch(model.Phone, @"^[0-9]+$"))
                {
                    errors.Add("Phone only accept numbers 0-9. ");
                }

                //CURP
                if (model.CURP.Equals("") || model.CURP == null)
                {
                    errors.Add("CURP is required. ");
                }
                if (model.CURP.Length > 20)
                {
                    errors.Add("CURP cannot be longer than 20 characters. ");
                }
                if (!Regex.IsMatch(model.CURP, @"^[a-zA-Z0-9]+$"))
                {
                    errors.Add("CURP only accept characters A-Z and numbers 0-9. ");
                }

                if (errors.Count == 0)
                {
                    model.Name.Trim();
                    model.Address.Trim();
                    model.Phone.Trim();
                    model.CURP.Trim();
                    model.CURP.ToUpper();
                    var contactUpdated = await contactService.Update(model, model.Id);
                    if (contactUpdated == null)
                    {
                        return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Contact was not updated."});
                    }

                    return Ok(new ResponseDTO() { Status = ResponseStatus.Success, Message = "Success", Data = contactUpdated });
                }
                else
                {
                    return Ok(new ResponseDTO() { Status = ResponseStatus.ValidationError, Message = "Validation Error", Data = errors });
                }
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "APIWEB",
                    FileName = "ContactController.cs",
                    MethodName = "Update",
                    Message = error.Message.ToString()
                });
                return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Something goes wrong, please notify to the administrator." });
            }
        }

        [HttpPost]
        [Route("Delete")]
        public async Task<ActionResult> Delete([FromBody] int id)
        {
            try
            {
                List<string> errors = new List<string>();
                if (id <= 0)
                {
                    errors.Add("Id is required. ");
                }

                if (errors.Count > 0)
                {
                    return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Contact was not deleted.", Data = errors });
                }
                else
                {
                    var contactDeleted = await contactService.Delete(id);
                    if (contactDeleted == null)
                    {
                        return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "The contact to delete doesn't exists." });
                    }
                    else
                    {
                        return Ok(new ResponseDTO() { Status = ResponseStatus.Success, Message = "Contact has been deleted.", Data = "Contact has been deleted." });
                    }
                }
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "APIWEB",
                    FileName = "ContactController.cs",
                    MethodName = "Delete",
                    Message = error.Message.ToString()
                });
                return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Something goes wrong, please notify to the administrator." });
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("UploadCSV")]
        public async Task<ActionResult> UploadCSV()
        {
            try
            {
                List<string> errors = new List<string>();
                var csvFile = Request.Form.Files[0];
                if (csvFile == null)
                {
                    errors.Add("File CSV is required.");
                }

                if (errors.Count == 0)
                {
                    using (var reader = new StreamReader(csvFile.OpenReadStream()))
                    {
                        int row = 0;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();

                            if (row > 0)
                            {
     
                                ContactDTO model = new ContactDTO();

                                #region validation of the model

                                model.RegisterDate = DateTime.Now;
                                model.Name = line.Split(",")[0].ToString().Trim();
                                model.Address = line.Split(",")[1].ToString().Trim();
                                model.Phone = line.Split(",")[2].ToString().Trim();
                                model.CURP = line.Split(",")[3].ToString().Trim();

                                //Name
                                if (model.Name.Equals("") || model.Name == null)
                                {
                                    errors.Add("Name is required. ");
                                }
                                if (model.Name.Length > 50)
                                {
                                    errors.Add("Name cannot be longer than 50 characters. ");
                                }
                                if (!Regex.IsMatch(model.Name, @"^[a-z A-Z]+$"))
                                {
                                    errors.Add("Name only accept characters a-z.");
                                }

                                //Address
                                if (model.Address.Equals("") || model.Address == null)
                                {
                                    errors.Add("Address is required. ");
                                }
                                if (model.Address.Length > 150)
                                {
                                    errors.Add("Address cannot be longer than 150 characters. ");
                                }
                                if (!Regex.IsMatch(model.Address, @"^[a-z A-Z 0-9]+$"))
                                {
                                    errors.Add("Address only accept characters A-Z and numbers 0-9. ");
                                }

                                //Phone
                                if (model.Phone.Equals("") || model.Phone == null)
                                {
                                    errors.Add("Phone is required. ");
                                }
                                if (model.Phone.Length > 150)
                                {
                                    errors.Add("Phone cannot be longer than 15 characters. ");
                                }
                                if (!Regex.IsMatch(model.Phone, @"^[0-9]+$"))
                                {
                                    errors.Add("Phone only accept numbers 0-9. ");
                                }

                                //CURP
                                if (model.CURP.Equals("") || model.CURP == null)
                                {
                                    errors.Add("CURP is required.");
                                }
                                if (model.CURP.Length > 20)
                                {
                                    errors.Add("CURP cannot be longer than 20 characters. ");
                                }
                                if (!Regex.IsMatch(model.CURP, @"^[a-zA-Z0-9]+$"))
                                {
                                    errors.Add("CURP only accept characters A-Z and numbers 0-9. ");
                                }

                                #endregion

                                if (errors.Count == 0)
                                {
                                    //Save
                                    await contactService.Save(model);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            row++;
                        }

                        if (row == 1 || row == 0)
                        {
                            errors.Add("The CSV file its empty, please verify the information. ");
                        }

                        if (errors.Count == 0)
                        {
                            return Ok(new ResponseDTO() { Status = ResponseStatus.Success, Message = "Success" });
                        }
                        else 
                        {
                            return Ok(new ResponseDTO() { Status = ResponseStatus.ValidationError, Message = "Validation Error", Data = errors });
                        }
                    }
                }
                else
                {
                    return Ok(new ResponseDTO() { Status = ResponseStatus.ValidationError, Message = "Validation Error", Data = errors });
                }
            }
            catch (Exception error)
            {
                await logService.Save(new LogDTO()
                {
                    Layer = "APIWEB",
                    FileName = "ContactController.cs",
                    MethodName = "UploadCSV",
                    Message = error.Message.ToString()
                });
                return Ok(new ResponseDTO() { Status = ResponseStatus.Error, Message = "Something goes wrong, please notify to the administrator." });
            }
        }
    }
}
