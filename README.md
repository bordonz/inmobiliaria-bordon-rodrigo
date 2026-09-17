# Proyecto inmobiliaria-bordon-rodrigo

Este proyecto implementa un sistema de ABM (Alta, Baja, Modificación) para **Propietarios, Inquilinos, Inmuebles con sus imagenes y Reservas** utilizando ASP.NET Core y MySQL.

## ✨ Características
- ABM completo de Propietarios (Alta, Baja, Modificación)
- ABM completo de Inquilinos
- ABM completo de Inmuebles e Imagenes
- ABM completo de Reservas
- Panel nav intuitivo
- Arquitectura MVC

## Requisitos previos
- [.NET 6 o superior](https://dotnet.microsoft.com/download)
- [MySQL Server](https://dev.mysql.com/downloads/mysql/) instalado y corriendo en tu máquina
- [MySQL Workbench](https://dev.mysql.com/downloads/workbench/) o [DBeaver](https://dbeaver.io/) (opcional, para administrar la base de datos)

## Dependencias necesarias
El proyecto utiliza el paquete oficial de MySQL para .NET:

**Comando**
dotnet add package MySql.Data

## Si No tenes una base de datos
- En la carpeta docs esta el script para la DB

## En el archivo appsettings.json configurar. Tomar como ejemplo appsettingsExample.json

- Cambiar el DefaultConnection
- Cambiar el salt
- Cambiar la SecretKey por una segura

## Ejecución del proyecyto
- Clonar repositorio
    https://github.com/bordonz/inmobiliaria-bordon-rodrigo.git

- En la terminal parado en el preyecto: 
    dotnet run

- En el navegador que uses coloca la url: 
    http://localhost:PORT