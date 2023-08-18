# Backend API REST para el Sistema de Control de Ciudadanos

¡Bienvenido al Backend API REST del Sistema de Control de Ciudadanos! Este componente es la columna vertebral que permite la gestión de ciudadanos y sus datos asociados. A través de esta API, podrás llevar a cabo las operaciones básicas de Crear, Leer, Actualizar y Borrar (CRUD) en la base de datos para mantener información precisa y actualizada sobre los ciudadanos.

## Acerca de

Este backend está diseñado para trabajar en conjunto con la aplicación de frontend del Sistema de Control de Ciudadanos. Proporciona una interfaz a través de la cual el frontend puede interactuar con la base de datos y realizar operaciones esenciales para administrar los datos de los ciudadanos.

## Características Principales

- **CRUD de Ciudadanos:** Esta API permite crear nuevos registros de ciudadanos, obtener información detallada de los mismos, actualizar sus datos y eliminar registros existentes.

- **Documentación con Swagger:** Hemos integrado Swagger para facilitar la exploración y comprensión de los endpoints de la API. Puedes acceder a la documentación visitando `/swagger` una vez que la API esté en funcionamiento.

- **Base de Datos MS SQL:** Utilizamos Microsoft SQL Server como base de datos para almacenar los datos de los ciudadanos. Esto asegura la persistencia y la disponibilidad de la información.

- **Autenticación con JWT:** La autenticación se realiza a través de JSON Web Tokens (JWT), lo que garantiza la seguridad de las operaciones y la protección de los datos de los ciudadanos.

- **Manejo de Roles:** Implementamos roles de usuario para administradores y usuarios que acceden a través de Google. Los roles determinan los permisos de acceso y las acciones que los usuarios pueden realizar.

- **Borrado Lógico**: En lugar de eliminar físicamente los registros de ciudadanos, se implementa un borrado lógico. Esto significa que los registros se marcan como inactivos en lugar de ser eliminados de la base de datos, lo que permite mantener un historial de cambios y recuperar datos en caso necesario.

## Configuración

Antes de ejecutar el backend, asegúrate de configurar correctamente los siguientes aspectos:

1. **Base de Datos:** Configura la conexión a tu instancia de Microsoft SQL Server en el archivo de configuración.

2. **Autenticación Google:** Asegúrate de ajustar la configuración de GoogleKey según tus credenciales.
