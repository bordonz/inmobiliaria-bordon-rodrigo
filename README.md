# Proyecto inmobiliaria-bordon-rodrigo

Este proyecto implementa un sistema de ABM (Alta, Baja, Modificación) para **Propietarios** e **Inquilinos** utilizando ASP.NET Core y MySQL.

## ✨ Características
- ABM completo de Propietarios (Alta, Baja, Modificación)
- ABM completo de Inquilinos
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
- CREATE DATABASE nombreBD;

- CREATE TABLE Propietarios (                           
    id_propietario INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    dni VARCHAR(20),
    telefono VARCHAR(20),
    email VARCHAR(100),
    clave VARCHAR(100)
);

- CREATE TABLE Inquilinos (
    id_inquilino INT AUTO_INCREMENT PRIMARY KEY,
    nombre VARCHAR(50),
    apellido VARCHAR(50),
    dni VARCHAR(20),
    telefono VARCHAR(20),
    email VARCHAR(100),
);

## En el archivo appsettings.json configurar la cadena de conexión
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=inmobiliaria;User=root;Password=tu_password;"
}
- Cambiar Database por el nombre de tu base de datos
- Cambiar tu Password por tu password

## Ejecución del proyecyto
- Clonar repositorio
    https://github.com/bordonz/inmobiliaria-bordon-rodrigo.git

- dotnet run

- http://localhost:PORT