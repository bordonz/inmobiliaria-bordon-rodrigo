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
- CREATE DATABASE nombreBD;

- CREATE TABLE `propietarios` (
  `id_propietario` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  `clave` varchar(255) NOT NULL,
  PRIMARY KEY (`id_propietario`),
  UNIQUE KEY `dni` (`dni`),
  UNIQUE KEY `email` (`email`)
)

- CREATE TABLE `inquilinos` (
  `id_inquilino` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  PRIMARY KEY (`id_inquilino`),
  UNIQUE KEY `dni` (`dni`),
  UNIQUE KEY `email` (`email`)
)

- CREATE TABLE `inmuebles` (
  `id_inmueble` int NOT NULL AUTO_INCREMENT,
  `direccion` varchar(50) NOT NULL,
  `cupo` int NOT NULL,
  `precio_por_dia` decimal(10,2) NOT NULL,
  `porcentaje_reserva` decimal(10,2) NOT NULL,
  `latitud` decimal(12,6) DEFAULT NULL,
  `longitud` decimal(12,6) DEFAULT NULL,
  `propietario_id` int NOT NULL,
  `habilitado` tinyint(1) DEFAULT '1',
  `tipo` varchar(150) NOT NULL,
  `portada` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`id_inmueble`),
  KEY `FK_Inmueble_Propietario` (`propietario_id`),
  CONSTRAINT `FK_Inmueble_Propietario` FOREIGN KEY (`propietario_id`) REFERENCES `propietarios` (`id_propietario`)
)

- CREATE TABLE `reservas` (
  `id_reserva` int NOT NULL AUTO_INCREMENT,
  `estado` varchar(50) NOT NULL,
  `monto` decimal(10,2) NOT NULL,
  `fecha_desde` datetime NOT NULL,
  `fecha_hasta` datetime NOT NULL,
  `inmueble_id` int NOT NULL,
  `inquilino_id` int NOT NULL,
  PRIMARY KEY (`id_reserva`),
  KEY `FK_Reserva_Inmueble` (`inmueble_id`),
  KEY `FK_Reserva_Inquilino` (`inquilino_id`),
  CONSTRAINT `FK_Reserva_Inmueble` FOREIGN KEY (`inmueble_id`) REFERENCES `inmuebles` (`id_inmueble`),
  CONSTRAINT `FK_Reserva_Inquilino` FOREIGN KEY (`inquilino_id`) REFERENCES `inquilinos` (`id_inquilino`)
)

- CREATE TABLE `imagenes` (
  `id_imagen` int NOT NULL AUTO_INCREMENT,
  `url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `inmueble_id` int NOT NULL,
  PRIMARY KEY (`id_imagen`),
  KEY `fk_inmueble_imagen` (`inmueble_id`),
  CONSTRAINT `fk_inmueble_imagen` FOREIGN KEY (`inmueble_id`) REFERENCES `inmuebles` (`id_inmueble`)
)

## En el archivo appsettings.json configurar la cadena de conexión
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=inmobiliaria;User=root;Password=tu_password;"
}
- Cambiar Database por el nombre de tu base de datos
- Si es necesario cambia el User por el que use DB
- Cambiar tu_password por tu password

## Ejecución del proyecyto
- Clonar repositorio
    https://github.com/bordonz/inmobiliaria-bordon-rodrigo.git

- En la terminal parado en el preyecto: 
    dotnet run

- En el navegador que uses coloca la url: 
    http://localhost:PORT