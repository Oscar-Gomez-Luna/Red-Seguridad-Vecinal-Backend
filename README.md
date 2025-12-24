# Desarrollo-Movil Gestion Vecinal

API RESTful monolítica desarrollada en **ASP.NET Core 7.0** que gestiona toda la lógica de negocio del ecosistema de seguridad vecinal.

## Partes del proyecto:
* **Frontend:** https://github.com/Oscar-Gomez-Luna/Red-Seguridad-Vecinal-Frontend
* **Apliación móvil:** https://github.com/IDGS-901-22002224/Desarrollo-Movil_Gestion-Vecinal

## Tecnologías
* **Framework:** ASP.NET Core 7.0
* **Base de Datos:** Azure SQL  con Entity Framework y Firebase (pruebas en SQL Server)
* **Seguridad:** JWT & Firebase Auth

## Funcionalidades
* **Gestión de Alertas:** Recepción y procesamiento de alertas de pánico en tiempo real.
* **Control de Accesos:** Validación de códigos QR dinámicos.
* **Finanzas:** Lógica para estados de cuenta y registro de pagos.
* **Administración:** 11 Controladores para gestión de usuarios, zonas y servicios.

| Actividad | Se utilizó | Descripción | Fecha |
|-----------|------------|-------------|---------|
| Merge Final | GitHub | Se realizó la fusión final de todas las ramas de desarrollo a la rama principal (master), integrando todos los módulos completados y probados del proyecto. | 27/11/2025 |
| Cambios a appsettings | Android Studio | Se realizaron modificaciones en la configuración de la aplicación (appsettings) para ajustar parámetros de conexión y configuraciones necesarias para el despliegue. | 27/11/2025 |
| Cambios a config | Android Studio | Se actualizó la configuración general del proyecto para optimizar el rendimiento y ajustar valores de configuración previos al despliegue. | 26/11/2025 |
| Cambios a gitignore | GitHub | Se modificó el archivo .gitignore para excluir archivos innecesarios del repositorio y mantener limpio el control de versiones. | 26/11/2025 |
| Configuración para desplegar | Android Studio | Se realizaron las configuraciones necesarias para preparar la aplicación para su despliegue en producción, incluyendo ajustes de build y optimizaciones. | 26/11/2025 |
| Configuraciones terminadas | Android Studio | Se completaron todas las configuraciones finales del proyecto, verificando que todos los parámetros estén correctamente establecidos para el funcionamiento óptimo. | 26/11/2025 |
| Fix: ignorando archivo correctamente | GitHub | Se corrigió un problema con el archivo .gitignore para asegurar que ciertos archivos sean ignorados correctamente por el control de versiones. | 26/11/2025 |
| Cambios para el deploy | Android Studio | Se realizaron los últimos ajustes y modificaciones necesarias para el proceso de despliegue de la aplicación en el entorno de producción. | 26/11/2025 |
| Merge a rama principal | GitHub | Se fusionó la rama de desarrollo con la rama principal, integrando nuevas funcionalidades y correcciones antes del merge final. | 26/11/2025 |
| Merge Final Oscar | GitHub | Se realizó el merge final de la rama de Oscar a la rama principal, integrando sus últimas contribuciones al proyecto. | 26/11/2025 |
| Apis desactivar y reactivar agregadas | Android Studio | Se implementaron las APIs necesarias para permitir la funcionalidad de desactivar y reactivar servicios dentro de la aplicación. | 23/11/2025 |
| Nuevas api a pagos | Android Studio | Se agregaron nuevas APIs al módulo de pagos para mejorar la funcionalidad y permitir operaciones adicionales de transacciones. | 22/11/2025 |
| Migraciones nuevas DB | SQL, Firebase | Se crearon y ejecutaron nuevas migraciones de base de datos para actualizar el esquema y agregar nuevas tablas o campos necesarios. | 21/11/2025 |
| Cambios de rama oscar a develop 2 | GitHub | Se fusionaron los cambios de la rama personal de Oscar a la rama develop para continuar con las pruebas de integración. | 21/11/2025 |
| Pagos correcciones terminadas | Android Studio | Se completaron todas las correcciones identificadas en el módulo de pagos, asegurando su correcto funcionamiento. | 21/11/2025 |
| Nuevas migraciones | SQL, Firebase | Se ejecutaron nuevas migraciones de base de datos para mantener sincronizado el esquema con los requerimientos actuales del proyecto. | 20/11/2025 |
| Cambios en el gitignore | GitHub | Se actualizó el archivo .gitignore para incluir o excluir archivos específicos del control de versiones. | 20/11/2025 |
| Cambios en la configuración | Android Studio | Se realizaron ajustes en los archivos de configuración del proyecto para mejorar la compatibilidad y el rendimiento. | 20/11/2025 |
| CORRECCIÓN: SE AGREGA ESTADO EN MODELS/RESERVAS PARA MANEGAR EL ESTADO DE LA RESERVA, SE HACE NUEVA MIGRACION Y ACTUALIZACION DE BD | SQL, Firebase, Android Studio | Se agregó un nuevo campo de estado en el modelo de reservas para gestionar adecuadamente el ciclo de vida de las reservas. Se creó la migración correspondiente y se actualizó la base de datos. | 19/11/2025 |
| MODIFICACION A 7 DIAS DE QR PERSONAL | Android Studio | Se modificó la configuración del código QR personal para que tenga una vigencia de 7 días antes de expirar. | 19/11/2025 |
| PRUEBA DE BACKGROUND SERVICE DE INTERVALOS | Android Studio | Se implementó y probó un servicio en segundo plano que ejecuta tareas de manera periódica en intervalos definidos. | 19/11/2025 |
| CARGOS SERVICIOS | Android Studio | Se implementó la funcionalidad completa para la visualización y gestión de cargos de servicios dentro de la aplicación. | 19/11/2025 |
| Cambios a módulo pagos | Android Studio | Se realizaron modificaciones y mejoras en el módulo de pagos para corregir errores y agregar nuevas funcionalidades. | 19/11/2025 |
| gitignore modificado | GitHub | Se actualizó el archivo .gitignore con nuevas reglas para excluir archivos generados automáticamente o temporales. | 19/11/2025 |
| Cambios de oscar | Android Studio | Se integraron los cambios realizados por Oscar en su rama de desarrollo, incluyendo correcciones y nuevas implementaciones. | 19/11/2025 |
| Cambios Landin | Android Studio | Se realizaron modificaciones en la página de inicio (landing) de la aplicación para mejorar la experiencia del usuario. | 19/11/2025 |
| Archivo firebase agregado | Firebase | Se agregó el archivo de configuración de Firebase necesario para conectar la aplicación con los servicios de Firebase. | 19/11/2025 |
| ACCESOS E INVITADOS QR CONTROLLER | Android Studio | Se implementó el controlador completo para gestionar los códigos QR de accesos e invitados, incluyendo generación y validación. | 14/11/2025 |
| API'S cuenta-usuario y cuenta-usuario por id de usuario abregadas | Android Studio | Se desarrollaron las APIs necesarias para obtener información de cuenta de usuario y consultar cuentas por ID de usuario específico. | 14/11/2025 |
| Corrección a update usuario | Android Studio | Se corrigieron errores en la funcionalidad de actualización de datos de usuario para asegurar que los cambios se guarden correctamente. | 15/11/2025 |
| APIS TERMINADAS, CORRECCION DE DTOS/REQUEST/FIREBASEDATASERVICE, AMENIDADES Y RESERVAS | Android Studio, Firebase | Se finalizaron todas las APIs pendientes, se corrigieron los DTOs y requests, se actualizó el servicio de Firebase y se completaron los módulos de amenidades y reservas. | 17/11/2025 |
| Cambios update numero tarjeta y agregar cmpos de cuenta usuario | Android Studio | Se implementó la funcionalidad para actualizar el número de tarjeta del usuario y se agregaron nuevos campos a la tabla de cuenta de usuario. | 13/11/2025 |
| Cambios a modulo avisos | Android Studio | Se realizaron modificaciones y mejoras en el módulo de avisos para corregir bugs y mejorar la interfaz de usuario. | 13/11/2025 |
| Cambios a 4 digistos automaticos en update y marcadores mapas | Android Studio | Se implementó la funcionalidad para generar automáticamente 4 dígitos en las actualizaciones y se agregaron marcadores personalizados en los mapas. | 13/11/2025 |
