-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: inmobiliaria_airbnb
-- ------------------------------------------------------
-- Server version	26.7.0

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
SET @MYSQLDUMP_TEMP_LOG_BIN = @@SESSION.SQL_LOG_BIN;
SET @@SESSION.SQL_LOG_BIN= 0;

--
-- GTID state at the beginning of the backup 
--

SET @@GLOBAL.GTID_PURGED=/*!80000 '+'*/ 'b2a2087a-98e3-11f1-b534-0a0027000006:1-202';

--
-- Table structure for table `imagenes`
--

DROP TABLE IF EXISTS `imagenes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `imagenes` (
  `id_imagen` int NOT NULL AUTO_INCREMENT,
  `url` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `inmueble_id` int NOT NULL,
  PRIMARY KEY (`id_imagen`),
  KEY `fk_inmueble_imagen` (`inmueble_id`),
  CONSTRAINT `fk_inmueble_imagen` FOREIGN KEY (`inmueble_id`) REFERENCES `inmuebles` (`id_inmueble`)
) ENGINE=InnoDB AUTO_INCREMENT=28 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `imagenes`
--

LOCK TABLES `imagenes` WRITE;
/*!40000 ALTER TABLE `imagenes` DISABLE KEYS */;
INSERT INTO `imagenes` VALUES (6,'/Uploads/Inmuebles/2/78cdf74b-1cd5-4c9e-bd29-cc156df83205.jpg',2),(7,'/Uploads/Inmuebles/2/4b44982b-61aa-409f-9641-dcba94635170.jpg',2),(8,'/Uploads/Inmuebles/2/696ad0fc-8795-4f9c-9b0d-15af2048ad77.jpg',2),(9,'/Uploads/Inmuebles/2/9dc18dcc-74b9-4eee-a5fc-fb3c9ca5d12b.jpg',2),(10,'/Uploads/Inmuebles/2/0a143d17-e11b-417a-ae25-da9bbf0a11e4.jpg',2),(26,'/Uploads/Inmuebles/1/029b53db-87d7-43c6-a0c2-b2f66e549c3b.jpg',1);
/*!40000 ALTER TABLE `imagenes` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inmuebles`
--

DROP TABLE IF EXISTS `inmuebles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inmuebles` (
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
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inmuebles`
--

LOCK TABLES `inmuebles` WRITE;
/*!40000 ALTER TABLE `inmuebles` DISABLE KEYS */;
INSERT INTO `inmuebles` VALUES (1,'Calle Sarmiento 450, 2° B Barrio Palermo',3,150000.00,600000.00,-345965.000000,-583745.000000,1,0,'Apartamento','/Uploads/Inmuebles\\portada_1.jpg'),(2,'Calle Junin 450, 2° B Barrio Pringles',2,100000.00,300000.00,-245963.000000,-583745.000000,1,0,'','/Uploads/Inmuebles\\portada_2.jpg'),(3,'Calle Concaran 150, 4° A Barrio Sarmiento',2,200000.00,500000.00,-445965.000000,-763744.000000,1,1,'',NULL),(4,'Calle Jose 150, 1° A Barrio Palermo',4,250000.00,700000.00,-145963.000000,-383745.000000,3,1,'',NULL),(5,'Calle San Luis 150, 4° A Barrio Mercedez',4,250000.00,600000.00,-295963.000000,-863564.000000,1,1,'Casa',NULL),(6,'Calle prueba123',1,80000.00,35000.00,-345965.000000,-563744.000000,1,1,'Departamento',NULL);
/*!40000 ALTER TABLE `inmuebles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `inquilinos`
--

DROP TABLE IF EXISTS `inquilinos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `inquilinos` (
  `id_inquilino` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(50) NOT NULL,
  `apellido` varchar(50) NOT NULL,
  `dni` varchar(20) NOT NULL,
  `telefono` varchar(20) DEFAULT NULL,
  `email` varchar(100) NOT NULL,
  PRIMARY KEY (`id_inquilino`),
  UNIQUE KEY `dni` (`dni`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `inquilinos`
--

LOCK TABLES `inquilinos` WRITE;
/*!40000 ALTER TABLE `inquilinos` DISABLE KEYS */;
INSERT INTO `inquilinos` VALUES (3,'Soraya','Echeto','43434232','23424354','Echeto@gmail.com'),(4,'PRUEBA','prueba','22349234','2665432563','adsad@gmail.com');
/*!40000 ALTER TABLE `inquilinos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `pagos`
--

DROP TABLE IF EXISTS `pagos`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pagos` (
  `id_pago` int NOT NULL AUTO_INCREMENT,
  `concepto` varchar(150) NOT NULL,
  `fecha_pago` datetime NOT NULL,
  `monto` decimal(10,2) NOT NULL,
  `estado` varchar(50) NOT NULL,
  `reserva_id` int NOT NULL,
  `id_usuario_creador` int NOT NULL,
  `id_usuario_anulador` int DEFAULT NULL,
  PRIMARY KEY (`id_pago`),
  KEY `FK_Pago_Reserva` (`reserva_id`),
  KEY `FK_Pago_Creador` (`id_usuario_creador`),
  KEY `FK_Pago_Anulador` (`id_usuario_anulador`),
  CONSTRAINT `FK_Pago_Anulador` FOREIGN KEY (`id_usuario_anulador`) REFERENCES `usuarios` (`id_usuario`),
  CONSTRAINT `FK_Pago_Creador` FOREIGN KEY (`id_usuario_creador`) REFERENCES `usuarios` (`id_usuario`),
  CONSTRAINT `FK_Pago_Reserva` FOREIGN KEY (`reserva_id`) REFERENCES `reservas` (`id_reserva`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `pagos`
--

LOCK TABLES `pagos` WRITE;
/*!40000 ALTER TABLE `pagos` DISABLE KEYS */;
INSERT INTO `pagos` VALUES (1,'Pago completo','2026-09-09 20:06:00',700000.00,'Anulado',1,2,NULL),(2,'Pago completo','2026-09-15 15:44:00',200000.00,'Cancelado',6,2,NULL),(3,'Pago completo','2026-09-16 19:07:00',80000.00,'Cancelado',7,1,NULL),(4,'Pago completo','2026-09-16 19:50:00',700000.00,'Cancelado',7,1,NULL);
/*!40000 ALTER TABLE `pagos` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `propietarios`
--

DROP TABLE IF EXISTS `propietarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `propietarios` (
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
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `propietarios`
--

LOCK TABLES `propietarios` WRITE;
/*!40000 ALTER TABLE `propietarios` DISABLE KEYS */;
INSERT INTO `propietarios` VALUES (1,'German','Chavez','22349234','2665666666','germanc@gmail.com','Sni8ZQGhvm5UAhkbtEglBOU8Q3KiNoS7hD7ustxNM9M='),(3,'Ignacio','Poder','29874621','2665432563','ignaciop@gmail.com','Iger123'),(5,'Micaela','Jofre','36093823','2665341234','micaela@gmail.com','mire123'),(6,'Delfina','Miranda','25093823','2665794823','delfinamiranda@gmail.com','deda123'),(7,'Miguel','Vargas','29838123','2665982576','miguel@gmail.com','mias123'),(8,'Roman','Riquelme','23984762','2665236854','roman@gmail.com','rome123'),(9,'Rodrigo','Bordon','44312456','2665217845','rodrigo@gmail.com','bDteahFYM+Uy9ki0ilghci5U+2S3XA16/VfxA6uRA4Y='),(10,'prueba','prueba','44530836','2665666666','prueba@gmail.com','Ku/iQEmiB3QTkm1fPu26+8oiFtXH0GAgJlV40NzDHKU=');
/*!40000 ALTER TABLE `propietarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `reservas`
--

DROP TABLE IF EXISTS `reservas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `reservas` (
  `id_reserva` int NOT NULL AUTO_INCREMENT,
  `estado` varchar(50) NOT NULL,
  `monto` decimal(10,2) NOT NULL,
  `fecha_desde` datetime NOT NULL,
  `fecha_hasta` datetime NOT NULL,
  `fecha_anticipada` date DEFAULT NULL,
  `inmueble_id` int NOT NULL,
  `inquilino_id` int NOT NULL,
  `pago_id` int DEFAULT NULL,
  `id_usuario_creador` int DEFAULT NULL,
  `id_usuario_finalizador` int DEFAULT NULL,
  PRIMARY KEY (`id_reserva`),
  KEY `FK_Reserva_Inmueble` (`inmueble_id`),
  KEY `FK_Reserva_Inquilino` (`inquilino_id`),
  KEY `FK_IdPago_Reserva` (`pago_id`),
  KEY `FK_Reserva_Creador` (`id_usuario_creador`),
  KEY `FK_Reserva_Finalizador` (`id_usuario_finalizador`),
  CONSTRAINT `FK_IdPago_Reserva` FOREIGN KEY (`pago_id`) REFERENCES `pagos` (`id_pago`),
  CONSTRAINT `FK_Reserva_Creador` FOREIGN KEY (`id_usuario_creador`) REFERENCES `usuarios` (`id_usuario`),
  CONSTRAINT `FK_Reserva_Finalizador` FOREIGN KEY (`id_usuario_finalizador`) REFERENCES `usuarios` (`id_usuario`),
  CONSTRAINT `FK_Reserva_Inmueble` FOREIGN KEY (`inmueble_id`) REFERENCES `inmuebles` (`id_inmueble`),
  CONSTRAINT `FK_Reserva_Inquilino` FOREIGN KEY (`inquilino_id`) REFERENCES `inquilinos` (`id_inquilino`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `reservas`
--

LOCK TABLES `reservas` WRITE;
/*!40000 ALTER TABLE `reservas` DISABLE KEYS */;
INSERT INTO `reservas` VALUES (1,'Confirmada',250000.00,'2026-08-28 12:00:00','2026-09-10 12:00:00',NULL,4,3,1,2,1),(6,'Confirmada',700000.00,'2026-09-15 14:24:00','2026-09-18 14:24:00',NULL,1,3,2,2,1),(7,'Concluida',250000.00,'2026-09-15 00:00:00','2026-09-17 00:00:00','2026-09-16',4,3,3,2,1);
/*!40000 ALTER TABLE `reservas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tipos_inmueble`
--

DROP TABLE IF EXISTS `tipos_inmueble`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tipos_inmueble` (
  `id_tipo_inmueble` int NOT NULL AUTO_INCREMENT,
  `descripcion` varchar(100) NOT NULL,
  PRIMARY KEY (`id_tipo_inmueble`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tipos_inmueble`
--

LOCK TABLES `tipos_inmueble` WRITE;
/*!40000 ALTER TABLE `tipos_inmueble` DISABLE KEYS */;
INSERT INTO `tipos_inmueble` VALUES (1,'Loft'),(3,'Departamento'),(4,'Casa');
/*!40000 ALTER TABLE `tipos_inmueble` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `usuarios`
--

DROP TABLE IF EXISTS `usuarios`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuarios` (
  `id_usuario` int NOT NULL AUTO_INCREMENT,
  `nombre` varchar(100) NOT NULL,
  `apellido` varchar(100) NOT NULL,
  `avatar` varchar(255) DEFAULT NULL,
  `email` varchar(150) NOT NULL,
  `clave` varchar(255) NOT NULL,
  `rol` int NOT NULL,
  PRIMARY KEY (`id_usuario`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=8 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `usuarios`
--

LOCK TABLES `usuarios` WRITE;
/*!40000 ALTER TABLE `usuarios` DISABLE KEYS */;
INSERT INTO `usuarios` VALUES (1,'Admin','Admin',NULL,'admin@gmail.com','5lxDWfUOv2LfObP4U6/yj7mXgoYro8/ljuQ7IEhHmik=',1),(2,'Rodolfo','Gimenez',NULL,'rodolfo@gmail.com','j9Ol5WjTgZM1Rt9NahGNh/nduUz0rbNxGBcuph+grl8=',2),(6,'Rodrigo','Bordon','/Uploads\\avatar_6.jfif','rodrigo@gmail.com','dyQWwIEnTwfM8iAHAVovYDdn7lLv4hTKUFAtZqnC1iE=',1),(7,'PRUEBA','prueba','/Uploads\\avatar_7.jpg','prueba@gmail.com','FeAYNcGzqpOoKfyAubGaqGNkFSQEaBDo1IjzmcTkLjI=',2);
/*!40000 ALTER TABLE `usuarios` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping routines for database 'inmobiliaria_airbnb'
--
SET @@SESSION.SQL_LOG_BIN = @MYSQLDUMP_TEMP_LOG_BIN;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-17 17:41:25
