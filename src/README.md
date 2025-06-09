# Estructura Técnica del Proyecto `BackEndAje.Api`

Este documento describe la estructura técnica del proyecto .NET `BackEndAje.Api`, organizando los distintos componentes por capas y responsabilidades.

---

## 📁 Solución General: `BackEndAje.Api`

Contiene **11 proyectos** distribuidos en las siguientes carpetas lógicas:

- `Core`: contiene la lógica de dominio y aplicación.
- `External`: implementaciones y componentes de infraestructura.
- `Test`: pruebas unitarias y de integración.
- `BackEndAje.Api.Web`: punto de entrada principal (ASP.NET Core).

---

## 📊 Capa Core

### ✅ `BackEndAje.Api.Application`

Contiene la lógica de negocio y de aplicación, dividida en módulos como:

- `Dtos`: objetos de transferencia de datos.
- `Services`: servicios de dominio o aplicación.
- `Interfaces`: contratos para servicios externos o repositorios.
- `Behaviors`: validaciones, logging o métodos transversales (como pipelines de MediatR).
- Carpetas por contexto de negocio: `OrderRequests`, `Assets`, `Clients`, `Roles`, etc.
- `Mappers`: configuraciones de mapeo (ej. AutoMapper).
- `Hubs`: señal de uso de SignalR.
- `ApplicationServiceCollectionExtensions.cs`: registro de dependencias.

---

### ✅ `BackEndAje.Api.Domain`

Contiene:

- `Entities`: entidades del dominio (clases del negocio).
- `Repositories`: interfaces que definen el acceso a datos.

---

## 🛠️ Capa External

### ✅ `BackEndAje.Api.Infrastructure`

Implementación de contratos definidos en `Domain`:

- `Data/ApplicationDbContext.cs`: configuración del contexto de datos (Entity Framework Core).
- `Middlewares`: manejo de errores, logging, etc.
- `Repositories`: implementación concreta de cada interfaz definida en `Domain`.
- `Services`: servicios auxiliares como:
    - `JwtTokenGenerator`
    - `HashingService`
    - `S3Service`

---

### ✅ `BackEndAje.Api.Presentation`

Contiene los controladores HTTP (`Controllers`) que exponen las funcionalidades REST:

- `AssetController`, `CensusController`, `OrderRequestController`, `UsersController`, etc.
- Manejan las solicitudes HTTP y delegan la lógica a la capa de `Application`.

---

## 📢 Web (Punto de Entrada)

### ✅ `BackEndAje.Api.Web`

Contiene:

- `Program.cs`: arranque del proyecto ASP.NET Core.
- `Jwt`: configuración del token JWT.
- `FileUpload`: filtros para Swagger o carga de archivos.
- `appsettings.json` y `appsettings.Development.json`: configuración por entorno.

---

## 🔧 Proyecto de Pruebas: `Test`

### ✅ `BackEndAje.Api.Application.Tests`

Contiene pruebas unitarias organizadas según los módulos de `Application`.

### ✅ `BackEndAje.Api.Domain.Tests`

Pruebas de lógica de negocio y entidades.

### ✅ `BackEndAje.Api.Architecture.Tests` y `BackEndAje.Api.Web.Tests`

Pruebas de arquitectura y validaciones de integraciones.

---

## 📖 Conclusión

El proyecto está construido sobre los principios de **Clean Architecture**, separando las responsabilidades en capas:

- **Application**: lógica de negocio y casos de uso.
- **Domain**: entidades y contratos.
- **Infrastructure**: detalles de implementación de almacenamiento, servicios, etc.
- **Presentation/Web**: capa expuesta al exterior vía HTTP.
- **Tests**: pruebas unitarias e integraciones bien organizadas.

Esto promueve un código **modular, testeable, mantenible y escalable**.

---

> Autor: Vortech  