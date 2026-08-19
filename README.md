# 🎮 PersonaStats API

REST API construida con .NET 10 para gestionar las estadísticas sociales inspiradas en el sistema de *Persona 5*.

[![.NET](https://img.shields.io/badge/.NET-10.0-blue.svg)]()
[![C#](https://img.shields.io/badge/C%23-14.0-purple.svg)]()
[![EF Core](https://img.shields.io/badge/EF%20Core-10.0-green.svg)]()
[![SQLite](https://img.shields.io/badge/SQLite-3-red.svg)]()
[![Swagger](https://img.shields.io/badge/Swagger-UI-orange.svg)]()

## 🎯 Sobre el proyecto

API REST construida como parte de mi prueba técnica para el rol de Analista de Operaciones de Sistemas, que gestiona las estadísticas sociales del estilo de *Persona 5* con persistencia en base de datos.

El sistema maneja las 5 estadísticas sociales clásicas del juego: **Conocimiento, Coraje, Amabilidad, Proeza y Valentía**, cada una con un nivel (1-5) y puntos de experiencia acumulados.

## 🛠️ Stack tecnológico

- [.NET 10](https://dotnet.microsoft.com/) (ASP.NET Core Web API) — última versión LTS
- [EF Core 10](https://learn.microsoft.com/ef/core/) + SQLite
- Swagger / Swashbuckle (UI interactiva de documentación)
- C# con controladores clásicos

## ✅ Requisitos previos

- [.NET SDK 10+](https://dotnet.microsoft.com/download)
- [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) (herramienta global):

```powershell
dotnet tool install --global dotnet-ef
```

## 🚀 Cómo ejecutar

```powershell
# 1. Restaurar paquetes
dotnet restore

# 2. Crear la base de datos (genera la migración y ejecuta el seed)
dotnet ef migrations add InitialCreate   # solo la primera vez
dotnet ef database update                # aplica la migración y crea personastats.db

# 3. Ejecutar la API
dotnet run
```

La API queda disponible en:

- **Swagger UI:** http://localhost:5056/swagger
- **Endpoint base:** http://localhost:5056/api/socialstats
- **Spec OpenAPI:** http://localhost:5056/openapi/v1.json

> El puerto puede variar según `Properties/launchSettings.json`.

## 📡 Endpoints

| Método | Ruta | Descripción | Códigos de respuesta |
|--------|------|-------------|----------------------|
| GET | `/api/socialstats` | Lista todas las estadísticas | 200 OK |
| GET | `/api/socialstats/{id}` | Obtiene una estadística por ID | 200 OK, 404 Not Found |
| POST | `/api/socialstats` | Crea una nueva estadística | 201 Created |
| PUT | `/api/socialstats/{id}` | Actualiza una estadística existente | 204 No Content, 400 Bad Request, 404 Not Found |
| DELETE | `/api/socialstats/{id}` | Elimina una estadística | 204 No Content, 404 Not Found |

### 📦 Ejemplo de payload

```json
{
  "name": "Encanto",
  "level": 2,
  "points": 50
}
```

## 🗂️ Estructura del proyecto

```
PersonaStatsApi/
├── Controllers/
│   └── SocialStatsController.cs   # Endpoints CRUD
├── Data/
│   └── AppDbContext.cs            # Contexto de EF Core + seed de datos
├── Models/
│   └── SocialStats.cs             # Entidad / modelo
├── Migrations/                    # Migraciones de EF Core
├── Program.cs                     # Configuración de la aplicación
└── appsettings.json               # Connection string de SQLite
```

## 💡 Decisiones técnicas

Cada decisión de este desarrollo fue tomada con un propósito. Estas son las principales y el razonamiento detrás de cada una:

### 🧩 .NET 10 (última versión LTS)

Elegí la versión más reciente del framework para trabajar sobre la tecnología vigente y demostrar que me mantengo actualizado en el ecosistema .NET.

### 🎛️ Controladores clásicos vs Minimal API

Usé la arquitectura de **controladores** porque es el estándar en entornos empresariales: la estructura (Controller + Model + Data) es más explícita, escalable y familiar para equipos de desarrollo tradicionales. Facilita la revisión del código y su mantenimiento.

### 🗄️ EF Core + SQLite

- **SQLite** permite levantar una base de datos real sin instalar ni configurar servidores, ideal para una prueba técnica reproducible en cualquier máquina.
- **EF Core** abstrae el acceso a datos: con cambiar el proveedor (SQL Server, PostgreSQL) y el connection string, la app podría correr contra otro motor sin reescribir lógica.
- El **DbContext** centraliza el mapeo entre el modelo y la tabla, y expone el `DbSet<SocialStats>` para las operaciones.

### 📜 Migraciones + Seed (HasData)

- Las **migraciones** versionan el esquema de la base de datos: cualquier cambio en el modelo se registra, se puede aplicar y revertir de forma controlada (importante en ambientes productivos, como los que se manejan en operaciones).
- El **seed con `HasData`** inserta las 5 estadísticas base automáticamente al aplicar la migración, garantizando datos iniciales consistentes sin intervención manual.

### 📝 DataAnnotations

Validación declarativa en el modelo (`[Required]`, `[Range(1,5)]`, `[MaxLength]`). Con `[ApiController]` en el controlador, ASP.NET valida automáticamente y responde **400 Bad Request** sin escribir una sola línea de validación manual. Menos código = menos errores.

### 🔄 Async/await en todos los endpoints

Las operaciones de base de datos son I/O. Usar métodos asíncronos evita bloquear hilos del servidor, permitiendo escalar mejor bajo carga — un criterio importante dado que el contexto laboral maneja procesamiento de alto volumen.

### 👁️ AsNoTracking() en lecturas

Para los GET, `AsNoTracking()` le indica a EF Core que no rastree las entidades en memoria: como solo se leen y devuelven, se evita overhead innecesario y se mejora la performance.

### 💉 Inyección de dependencias

El `AppDbContext` se inyecta en el constructor del controlador (DI nativa de .NET): el controlador no crea ni gestiona el ciclo de vida de la base de datos, separando responsabilidades y facilitando las pruebas.

### 📟 Códigos de estado HTTP correctos

Cada endpoint responde el código adecuado a su operación: **201 Created** al crear (con `CreatedAtAction` apuntando al recurso nuevo), **204 No Content** al actualizar/eliminar, **404 Not Found** cuando no existe, **400 Bad Request** ante datos inválidos. Un API bien diseñada comunica con su protocolo.

### 📖 Swagger / Swashbuckle

Documentación interactiva del API: permite probar todos los endpoints desde el navegador, útil tanto para el desarrollador como para quien consume la API.

### ⚙️ Configuración externa (appsettings.json)

La connection string vive en `appsettings.json`, no en el código: así la misma aplicación puede apuntar a diferentes bases según el ambiente (dev, staging, producción) sin recompilar.

## 🧪 Aspectos a mejorar

- Implementar **DTOs** para no exponer la entidad directamente en las respuestas.
- Agregar **autenticación (JWT)** para proteger los endpoints.

---

*Proyecto creado como parte de mi proceso de prueba técnica.*