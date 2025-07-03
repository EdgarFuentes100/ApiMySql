# ApiMySql
# 🧾 API de Gestión de Empleados

Esta es una API RESTful desarrollada con **ASP.NET Core Web API** y **MySQL** que permite la gestión de empleados. Se incluye validación, filtros, estadísticas y un middleware personalizado para registrar peticiones.

---

## 📌 Características principales

- Crear, consultar, actualizar y eliminar empleados
- Filtrar empleados por edad, puesto y departamento
- Obtener estadísticas (promedio de edad, cantidad por puesto y por departamento)
- Middleware personalizado para registrar todas las peticiones HTTP
- Documentación Swagger integrada
- Compatible con Postman y Swagger para pruebas

---

## ⚙️ Tecnologías utilizadas

- ASP.NET Core 
- Entity Framework Core
- MySQL
- Swagger (Swashbuckle)
- Postman
- Git + GitHub

---

## Configuracion del proyecto

### 1. Configurar la conexión a la base de datos

    Edita el archivo appsettings.json, en la seccion ConnectionStrings, ajusta la cadena de conexión MySQL con los datos de tu entorno local. Debes colocar:

      - El servidor (normalmente localhost para una base de datos local)

      - El nombre de la base de datos que deseas crear

      - El usuario y la contraseña de tu servidor MySQL

        Ejemplo( "Connection": "Server=localhost;Database=bd_entrevista;User=root;Password=123;")

### 2. Aplicar Migraciones
   
       Abre la Consola del Administrador de Paquetes en Visual Studio (Menú: Herramientas > Administrador de paquetes NuGet > Consola del Administrador de paquetes). Luego, ejecuta los siguientes comandos para          crear y aplicar las migraciones a la base de datos:
   
        - add-migration addNewTable
        - update-database
        Nota: si existen migraciones en la carpeta Migrations borrarlas para que se cree la nueva bd
---
### 3. Insertar datos en bd 
     Abre MySQL Workbench, entra a tu conexión, pega el script y ejecútalo, o usa el gestor de MySQL que tengas, no olvides seleccionar la base de datos que se crear al aplicar las migraciones.

```sql
INSERT INTO Empleado (nombre, edad, puesto, departamento) VALUES
('Ana Pérez', 35, 'Contadora', 'Contabilidad'),
('Carlos López', 28, 'Analista de Costos', 'Contabilidad'),
('Luis García', 42, 'Gerente Administrativo', 'Administración'),
('María Torres', 31, 'Asistente Administrativa', 'Administración'),
('Javier Méndez', 24, 'Desarrollador', 'Informatica'),
('Sofía Romero', 29, 'Diseñadora Gráfica', 'Marketing'),
('Roberto Díaz', 38, 'Jefe de Operaciones', 'Operaciones'),
('Elena Gutiérrez', 26, 'Asistente de Recursos Humanos', 'RRHH'),
('Pedro Martínez', 45, 'Jefe de Proyecto', 'Informatica'),
('Laura Jiménez', 32, 'Analista QA', 'Informatica'),
('Miguel Salazar', 40, 'Arquitecto de Software', 'Informatica'),
('Isabel Ruiz', 27, 'Ejecutiva de Ventas', 'Ventas'),
('Daniela Vega', 30, 'Diseñadora UX', 'Marketing'),
('Oscar Herrera', 33, 'Soporte Técnico', 'Informatica'),
('Patricia Castro', 36, 'Analista de Nómina', 'RRHH');
```

## 🚀 Clonar y correr la API

### 1. Clonar el repositorio

    git clone https://github.com/usuario/api-empleados.git

### 2. Abrir el proyecto

  Dar doble click sobre el archivo .sln que se encuentra en la raiz del proyecto, dar F5 para correr

### 🧪 Ejemplos de prueba con Swagger y Postman

### 📡 Lista de Endpoints con Swagger

| Método HTTP | Ruta                                       | Descripción                                                |
|-------------|--------------------------------------------|------------------------------------------------------------|
| POST        | `/api/empleados/CrearEmpleado`             | Crea un nuevo empleado.                                    |
| GET         | `/api/empleados/ListarEmpleados`           | Lista empleados, con filtros opcionales.                   |
| GET         | `/api/empleados/ListarMayoresDe30`         | Lista empleados con edad mayor a 30 años.                  |
| GET         | `/api/empleados/ObtenerEmpleadoPorId/{id}` | Obtiene un empleado por su ID.                             |
| PUT         | `/api/empleados/ActualizarEmpleado/{id}`   | Actualiza un empleado existente por su ID.                 |
| DELETE      | `/api/empleados/EliminarEmpleado/{id}`     | Elimina un empleado por su ID.                             |
| GET         | `/api/empleados/ObtenerEstadisticas`       | Obtiene estadísticas de empleados (total, promedios, etc). |

### 🔷 Probar con Swagger

   Ejecuta la API (en Visual Studio, presiona Ctrl + F5).

   Abre tu navegador y ve a:
   
#### https://localhost:7205/swagger/index.html

   Encontrarás una interfaz interactiva con todos los endpoints disponibles.

#### 1. Selecciona el endpoint que deseas probar.

#### 2. Haz clic en Try it out, completa los datos que solicita el formulario.

#### 3. Presiona Execute para enviar la petición.

#### 4. Revisa la respuesta y los códigos HTTP directamente en la interfaz.

### 🟧 Usando Postman

#### 1. Abre Postman.

##### 2. Crea una nueva petición.

##### 3. Configura la petición con el método HTTP y la URL correspondiente, 

      por ejemplo: Método: POST (Revisar la seccion de Enpoint para ver el tipo de Método)

## 📋 Uso de los Endpoints en Postman

Esta sección explica cómo probar cada endpoint de la API usando **Postman**.

| Endpoint                         | Método | Descripción                               | URL completa                                        | Uso en Postman                                      |
|---------------------------------|--------|-------------------------------------------|----------------------------------------------------|----------------------------------------------------|
| Crear empleado                  | POST   | Crea un nuevo empleado                    | `https://localhost:7205/api/empleados/CrearEmpleado`  | Enviar JSON en Body (raw, tipo JSON) con datos del empleado       |
| Listar empleados (con filtros) | GET    | Lista empleados, opcional con filtros     | `https://localhost:7205/api/empleados/ListarEmpleados` | Añadir query params opcionales: `edadMin`, `edadMax`, `puesto`, `departamento` |
| Listar empleados > 30 años      | GET    | Lista empleados mayores de 30 años        | `https://localhost:7205/api/empleados/ListarMayoresDe30` | No se envía body                                   |
| Obtener empleado por ID         | GET    | Obtiene un empleado por su ID             | `https://localhost:7205/api/empleados/ObtenerEmpleadoPorId/{id}` | Reemplazar `{id}` con el ID del empleado           |
| Actualizar empleado             | PUT    | Actualiza datos de un empleado            | `https://localhost:7205/api/empleados/ActualizarEmpleado/{id}` | Enviar JSON en Body con datos actualizados + reemplazar `{id}` |
| Eliminar empleado              | DELETE | Elimina un empleado por ID                 | `https://localhost:7205/api/empleados/EliminarEmpleado/{id}` | No se envía body + reemplazar `{id}`               |
| Obtener estadísticas           | GET    | Devuelve estadísticas sobre empleados     | `https://localhost:7205/api/empleados/ObtenerEstadisticas` | No se envía body                                   |

---

### Ejemplos detallados en Postman

#### Crear empleado (POST)
- URL:  
  `https://localhost:7205/api/empleados/CrearEmpleado`  
- Body (raw, JSON):  
```json
{
  "nombre": "Ana López",
  "edad": 28,
  "puesto": "Contadora",
  "departamento": "Finanzas"
}
```
#### Listar empleados con filtros (GET)

- URL: 
  `https://localhost:7205/api/empleados/ListarEmpleados?edadMin=25&puesto=Contadora`  
- Body (raw, JSON):
  
  No enviar body.
  
#### Listar empleados mayores de 30 (GET)

- URL:
  `https://localhost:7205/api/empleados/ListarMayoresDe30`  
- Body (raw, JSON):
  
  No enviar body.

#### Obtener empleado por ID (GET)

- URL ejemplo:
  `https://localhost:7205/api/empleados/ObtenerEmpleadoPorId/1`  
- Body (raw, JSON):
  
  No enviar body.
  
#### Actualizar empleado (PUT)

- URL:
  `https://localhost:7205/api/empleados/ActualizarEmpleado/1`  

- Body (raw, JSON):  
```json
{
"nombre": "Ana López Actualizada",
"edad": 29,
"puesto": "Contadora Senior",
"departamento": "Finanzas"
}
```
#### Eliminar empleado (DELETE)

- URL ejemplo:
    `https://localhost:7205/api/empleados/EliminarEmpleado/1` 
- Body (raw, JSON):
  
  No enviar body.

#### Obtener estadísticas (GET)

- URL:
  `https://localhost:7205/api/empleados/ObtenerEstadisticas` 
- Body (raw, JSON):
  
  No enviar body.
