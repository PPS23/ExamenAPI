# ExamenAPI

Este es un proyecto de pruebas, su contenido es de dominio publico. Trata de una API que cuenta con los metodos comunes para la administración de "Contacts" 
las cuales son Alta, Baja, Cambios y Consultas.

Todo el codigo esta validado con Try y Catch, dentro del metodo Catch, existe un pequeño procedimiento que almacena en base de datos en la tabla "Logs" el error
que se generé en caso de que la aplicación no realice su tarea correctamente.

Los controladores retornan un objeto de tipo "ReponseDTO", esto para obtener una respuesta mas clara en el proyecto web. Si la petición fue satisfactoria, se debe hacer uso
del enumerador "ResponseStatus" donde sus respuestas pueden ser las siguientes:

  Error: Ha ocurrido un error en la aplicación y esta no pudo ser controlada por las validaciones.
  Success: Todo ha salido bien.
  ValidationError: Ha ocurrido un error de validación y este fue controlado.
  
Dentro de el mismo objeto, existen dos propiedades mas, "Message" de tipo "string", en caso de que se requiera enviar un mensaje a la aplicación web. La siguiente propiedad "Data" 
que es de tipo "object", esta puede ser null, pero en caso de que sea necesario retornar información se debe utilizar esta propiedad.

Para la seguridad se hace uso de JWT, de momento solo el controlador "Contact" es el que esta protegido, debido a que la información que se maneja a travez de este controlador
es considerada como privada. Si se requiere proteger algun otro controlador, se debe utilizar la siguiente linea al inicio de cada controlador:

  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  
En la capa "Repository", se encuentra todo lo relacionado a la base de datos, para cada servicio utilizo una interfaz ya que esta es quien va a interactuar con la capa "API", cada
interfaz debe heredar de una llamada "IBaseService", dentro se encuentra la definición para tareas simples de un "CRUD".

IBaseService, solicitará un objeto para que al momento de implementar los metodos, estos ya tengan el tipo especifico a retornar y asi evitar duplicados y se pueda retornar
una información mas especifica. Si requieres de algun otro metodo extra que no se encuentre en IBaseService, solo debe agregarse en cada interfaz donde esta sea heredable.

Los servicios nunca retornan el tipo "Modelo" a la capa de "API", por ello se han creado archivos de transporte en la carpeta "DTO", para recibir información, tambien deben ser
del mismo tipo, para que el contexto detecte dichos cambios, existe una configuración en el archivo Startup.cs dentro de la capa "API" que a su vez toma como referencia a otro
archivo en la capa "Repository" llamado "AutoMapperConfigurationApp". En dicho archivo, se crea un mapeo donde:

  <Modelo, Transporte> = Tipo recibido, Tipo a Retornar
  <Transporte, Modelo> = Tipo recibido, Tipo a Retornar
  
Por ultimo, hay que modificar el archivo de configuracion "appsettings.json". Iniciando por la cadena de coneccion a la base de datos ya que la actual es para ingresar a 
un servidor local sin salida a internet. En caso de ser necesario, tambien editar "SecretKey" ya que en esta propiedad se usa para encriptar el JWT. Y no menos importante, 
la propiedad llamada "AppWEB", esta es utilizada para configurar el CORS que permite la conectividad entre el proyecto "Frontend" y la API(Backend).

Versiones utilizadas:
  ASP NET Core 5.0
  Microsoft.EntityFrameworkCore.SqlServer 5.0.10
  Microsoft.EntityFrameworkCore.Tools 5.0.10
  AutoMapper 10.1.1
  Microsoft.AspNetCore.Authentication.JwtBearer 5.0.10
  System.IdentityModel.Tokens.Jwt 6.12.2
  
 Autor: Pedro Pablo Soto
 Email: pedro_pablo@live.com.mx
 


