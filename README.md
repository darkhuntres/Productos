# Productos

Aplicación full stack para login y gestión de productos. El backend está desarrollado en .NET 10 y el frontend en React.

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

Ejecutar `database/CreateDatabase.sql` en SQL Server antes de iniciar el backend.

El script crea la base de datos `ProductosDb`, las tablas necesarias y los usuarios de prueba.

La cadena de conexión se encuentra en `Productos/appsettings.json`. Si es necesario, se puede modificar de acuerdo con la configuración local de SQL Server.

## Ejecutar el proyecto

Desde la raíz del proyecto se puede ejecutar:

```powershell
.\run-dev.ps1
```

Esto inicia el backend y el frontend.

También se pueden ejecutar por separado.

Backend:

```bash
cd Productos
dotnet restore
dotnet run
```

Frontend:

```bash
cd Productos.Web
npm install
npm run dev
```

`npm install` solo es necesario la primera vez o cuando cambian las dependencias.

Frontend: `http://localhost:5173`

Backend: `http://localhost:5080`

## Usuarios de prueba

Administrador:

```text
admin@serfinsa.com
Admin123!
```

Usuario:

```text
user@serfinsa.com
User123!
```

El administrador puede realizar el CRUD de productos. El usuario tiene acceso de solo lectura.

## Pruebas

Desde la raíz del proyecto:

```bash
dotnet test
```

Las pruebas incluyen autenticación, autorización por roles y operaciones CRUD de productos.
