# Productos

Aplicación full stack de login y gestión de productos. Backend en .NET 10 con autenticación JWT y autorización por roles, frontend en React consumiendo la API.

## Tecnologías

- .NET 10
- React 19
- TypeScript
- Vite
- SQL Server
- Entity Framework Core
- JWT
- BCrypt
- xUnit

## Requisitos

- .NET 10 SDK
- Node.js/npm
- SQL Server

## Base de datos

El script `database/CreateDatabase.sql` crea la base `ProductosDb`, las tablas y los usuarios de prueba. Ejecutarlo contra tu instancia de SQL Server (por ejemplo desde SSMS o `sqlcmd`) antes de levantar el backend.

Si prefieres usar migraciones de EF Core en vez del script, desde `Productos/` corre `dotnet ef database update` (requiere `dotnet tool install --global dotnet-ef`).

La cadena de conexión está en `Productos/appsettings.json` (`ConnectionStrings:DefaultConnection`), apuntando a `localhost` con autenticación de Windows. Ajústala si tu SQL Server usa otra configuración.

## Ejecutar el proyecto

La forma más simple, desde la raíz:

```
.\run-dev.ps1
```

Levanta el backend y el frontend cada uno en su propia ventana de PowerShell.

También se pueden levantar por separado:

Backend:
```
cd Productos
dotnet restore
dotnet run
```

Frontend:
```
cd Productos.Web
npm install
npm run dev
```

`npm install` solo hace falta la primera vez, o cuando cambian las dependencias del `package.json`.

Frontend: http://localhost:5173
Backend: http://localhost:5080

## Usuarios de prueba

Administrador (CRUD completo de productos):
```
admin@serfinsa.com
Admin123!
```

Usuario (solo lectura):
```
user@serfinsa.com
User123!
```

## Pruebas

Desde la raíz:

```
dotnet test
```

Cubren login (credenciales válidas e inválidas), acceso sin token, restricción por rol y el CRUD de productos.
