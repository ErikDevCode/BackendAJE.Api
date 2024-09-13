-- MySQL dump 10.13  Distrib 8.0.37, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: assetmanagement
-- ------------------------------------------------------
-- Server version	8.0.37

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

--
-- Table structure for table `appusers`
--

DROP TABLE IF EXISTS `appusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appusers` (
  `appUserId` int NOT NULL AUTO_INCREMENT,
  `userId` int DEFAULT NULL,
  `routeOrEmail` varchar(150) NOT NULL,
  `passwordHash` varchar(255) NOT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`appUserId`),
  UNIQUE KEY `email` (`routeOrEmail`),
  KEY `userId` (`userId`),
  CONSTRAINT `appusers_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `users` (`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `appusers`
--

LOCK TABLES `appusers` WRITE;
/*!40000 ALTER TABLE `appusers` DISABLE KEYS */;
INSERT INTO `appusers` (`appUserId`, `userId`, `routeOrEmail`, `passwordHash`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,1,'erik.abanto@gmail.com','$2a$11$3IU9VQFjQWSsVq43LU1RXOO5128iE7SBwVUlLyAUGvKYwyvYRH1m.','2024-09-09 15:54:13','2024-09-10 04:12:44',1,1),(2,2,'cesargpq@gmail.com','$2a$11$KsuyOxoB6G6wf1KZmtjXnewign5iORBMnG0JE2Mt0duGGHnSYFSiS','2024-09-09 22:40:43','2024-09-09 22:42:33',NULL,NULL),(3,3,'10001','$2a$11$EExk37co4nWRKgvKutxzc.qU15qCXdjB.zhuIDbbvnLgMtjzzKDfq','2024-09-10 03:48:15','2024-09-10 03:48:15',NULL,NULL);
/*!40000 ALTER TABLE `appusers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `auditlog`
--

DROP TABLE IF EXISTS `auditlog`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `auditlog` (
  `auditId` int NOT NULL AUTO_INCREMENT,
  `userId` int DEFAULT NULL,
  `action` varchar(255) DEFAULT NULL,
  `description` text,
  `actionDate` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`auditId`),
  KEY `userId` (`userId`),
  CONSTRAINT `auditlog_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `users` (`userId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `auditlog`
--

LOCK TABLES `auditlog` WRITE;
/*!40000 ALTER TABLE `auditlog` DISABLE KEYS */;
/*!40000 ALTER TABLE `auditlog` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `cedis`
--

DROP TABLE IF EXISTS `cedis`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cedis` (
  `cediId` int NOT NULL AUTO_INCREMENT,
  `regionId` int DEFAULT NULL,
  `cediName` varchar(150) NOT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`cediId`),
  KEY `cedis___fk_regionId` (`regionId`),
  CONSTRAINT `cedis___fk_regionId` FOREIGN KEY (`regionId`) REFERENCES `regions` (`regionId`)
) ENGINE=InnoDB AUTO_INCREMENT=57 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `cedis`
--

LOCK TABLES `cedis` WRITE;
/*!40000 ALTER TABLE `cedis` DISABLE KEYS */;
INSERT INTO `cedis` (`cediId`, `regionId`, `cediName`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,1,'TRADE MARKETING','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(2,1,'ECOMMERCE','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(3,1,'AJE','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(4,2,'CHINCHA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(5,2,'ICA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(6,2,'LIMA CENTRO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(7,2,'LIMA SUR','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(8,2,'LOGISTICO DEL SUR','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(9,2,'NAZCA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(10,2,'JERICO LIMA SUR','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(11,3,'BARRANCA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(12,3,'CHIMBOTE','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(13,3,'HUACHO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(14,3,'LIMA CALLAO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(15,3,'LIMA NORTE','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(16,3,'HUARAL','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(17,4,'CHICLAYO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(18,4,'PIURA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(19,4,'SULLANA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(20,4,'TUMBES','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(21,4,'TRUJILLO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(22,4,'JAEN','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(23,5,'AREQUIPA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(24,5,'CUSCO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(25,5,'JULIACA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(26,5,'MAJES','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(27,5,'MERAKI','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(28,5,'TACNA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(29,6,'IQUITOS','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(30,6,'JUANJUI','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(31,6,'PUCALLPA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(32,6,'TARAPOTO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(33,6,'TOCACHE','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(34,7,'AYACUCHO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(35,7,'HUANCAYO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(36,7,'LA MERCED','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(37,7,'TARMA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(38,8,'ABANCAY','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(39,8,'ANDAHUAYLAS','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(40,8,'HUANUCO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(41,8,'HUARAZ','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(42,8,'MOYOBAMBA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(43,8,'PUERTO MALDONADO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(44,8,'TINGO MARIA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(45,8,'YURIMAGUAS','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(46,9,'AUTOSERVICIOS','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(47,9,'CASH & CARRY','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(48,9,'HORECA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(49,9,'INSTITUCIONAL','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(50,9,'T. CONVENIENCIA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(51,9,'T.ESPECIALIZADA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(52,9,'MODERNO','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(53,11,'CRUCERISTA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(54,11,'MAYORISTA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(55,11,'MAYORISTAS AJE LIMA','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1),(56,11,'MERCADOS','2024-09-10 19:54:56','2024-09-10 19:54:56',1,1);
/*!40000 ALTER TABLE `cedis` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `documenttype`
--

DROP TABLE IF EXISTS `documenttype`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `documenttype` (
  `documentTypeId` int NOT NULL AUTO_INCREMENT,
  `documentTypeName` varchar(100) NOT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`documentTypeId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `documenttype`
--

LOCK TABLES `documenttype` WRITE;
/*!40000 ALTER TABLE `documenttype` DISABLE KEYS */;
INSERT INTO `documenttype` (`documentTypeId`, `documentTypeName`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,'DNI','2024-09-09 16:05:20','2024-09-09 16:05:20',1,1),(2,'RUC','2024-09-09 16:05:20','2024-09-09 16:05:20',1,1),(3,'CE','2024-09-09 16:05:20','2024-09-09 16:05:20',1,1),(4,'OTROS','2024-09-09 16:05:20','2024-09-09 16:05:20',1,1);
/*!40000 ALTER TABLE `documenttype` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `paymentmethods`
--

DROP TABLE IF EXISTS `paymentmethods`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `paymentmethods` (
  `paymentMethodId` int NOT NULL AUTO_INCREMENT,
  `paymentMethod` varchar(50) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`paymentMethodId`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `paymentmethods`
--

LOCK TABLES `paymentmethods` WRITE;
/*!40000 ALTER TABLE `paymentmethods` DISABLE KEYS */;
INSERT INTO `paymentmethods` (`paymentMethodId`, `paymentMethod`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,'CONTADO','2024-09-09 16:06:52','2024-09-09 16:06:52',1,1),(2,'CREDITO','2024-09-09 16:06:52','2024-09-09 16:06:52',1,1);
/*!40000 ALTER TABLE `paymentmethods` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `permissions`
--

DROP TABLE IF EXISTS `permissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `permissions` (
  `permissionId` int NOT NULL AUTO_INCREMENT,
  `permissionName` varchar(80) DEFAULT NULL,
  `Action` varchar(50) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`permissionId`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `permissions`
--

LOCK TABLES `permissions` WRITE;
/*!40000 ALTER TABLE `permissions` DISABLE KEYS */;
INSERT INTO `permissions` (`permissionId`, `permissionName`, `Action`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,'Dashboard','Read','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1),(2,'Dashboard','Write','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1),(3,'Dashboard','Update','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1),(4,'Users','Read','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1),(5,'Users','Write','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1),(6,'Users','Update','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1),(7,'Roles','Read','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1),(8,'Roles','Write','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1),(9,'Roles','Update','2024-09-09 15:24:21','2024-09-09 15:24:21',1,1);
/*!40000 ALTER TABLE `permissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `regions`
--

DROP TABLE IF EXISTS `regions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `regions` (
  `regionId` int NOT NULL AUTO_INCREMENT,
  `regionName` varchar(255) NOT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`regionId`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `regions`
--

LOCK TABLES `regions` WRITE;
/*!40000 ALTER TABLE `regions` DISABLE KEYS */;
INSERT INTO `regions` (`regionId`, `regionName`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,'OFICINA','2024-09-09 15:31:11','2024-09-09 15:31:11',1,1),(2,'LIMA S','2024-09-09 15:31:11','2024-09-09 15:31:11',1,1),(3,'LIMA N','2024-09-09 15:31:12','2024-09-09 15:31:12',1,1),(4,'NORTE','2024-09-09 15:31:12','2024-09-09 15:31:12',1,1),(5,'SUR','2024-09-09 15:31:12','2024-09-09 15:31:12',1,1),(6,'ORIENTE','2024-09-09 15:31:12','2024-09-09 15:31:12',1,1),(7,'CENTRO','2024-09-09 15:31:12','2024-09-09 15:31:12',1,1),(8,'ECONORED','2024-09-09 15:31:12','2024-09-09 15:31:12',1,1),(9,'NO TRADICIONAL','2024-09-09 15:31:12','2024-09-09 15:31:12',1,1),(10,'AJE','2024-09-09 15:31:12','2024-09-09 15:31:12',1,1),(11,'MAYOR','2024-09-10 15:23:18','2024-09-10 15:23:18',1,1);
/*!40000 ALTER TABLE `regions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `rolepermissions`
--

DROP TABLE IF EXISTS `rolepermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rolepermissions` (
  `rolePermissionId` int NOT NULL AUTO_INCREMENT,
  `roleId` int DEFAULT NULL,
  `permissionId` int DEFAULT NULL,
  `status` bit(1) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`rolePermissionId`),
  KEY `roleId` (`roleId`),
  KEY `permissionId` (`permissionId`),
  CONSTRAINT `rolepermissions_ibfk_1` FOREIGN KEY (`roleId`) REFERENCES `roles` (`roleId`),
  CONSTRAINT `rolepermissions_ibfk_2` FOREIGN KEY (`permissionId`) REFERENCES `permissions` (`permissionId`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `rolepermissions`
--

LOCK TABLES `rolepermissions` WRITE;
/*!40000 ALTER TABLE `rolepermissions` DISABLE KEYS */;
INSERT INTO `rolepermissions` (`rolePermissionId`, `roleId`, `permissionId`, `status`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,1,1,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1),(2,1,2,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1),(3,1,3,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1),(4,1,4,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1),(5,1,5,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1),(6,1,6,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1),(7,1,7,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1),(8,1,8,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1),(9,1,9,_binary '','2024-09-09 15:27:17','2024-09-09 15:27:17',1,1);
/*!40000 ALTER TABLE `rolepermissions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `roles`
--

DROP TABLE IF EXISTS `roles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `roles` (
  `roleId` int NOT NULL AUTO_INCREMENT,
  `roleName` varchar(50) NOT NULL,
  `description` varchar(255) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`roleId`),
  UNIQUE KEY `roleName` (`roleName`)
) ENGINE=InnoDB AUTO_INCREMENT=17 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `roles`
--

LOCK TABLES `roles` WRITE;
/*!40000 ALTER TABLE `roles` DISABLE KEYS */;
INSERT INTO `roles` (`roleId`, `roleName`, `description`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,'Administrador','Administra el sistema','2024-09-09 05:00:00','2024-09-09 05:00:00',1,1),(2,'Back Office','Back office','2024-09-09 15:17:17','2024-09-09 15:17:17',1,1),(3,'Jefe Regional','Jefe regional','2024-09-09 15:17:17','2024-09-09 15:17:17',1,1),(4,'Jefe de Cedi','Jefe de Cedi','2024-09-09 15:17:17','2024-09-09 15:17:17',1,1),(5,'Supervisor','Supervisor','2024-09-09 15:17:17','2024-09-09 15:17:17',1,1),(6,'Vendedor Tradicional','Vendedor Tradicional','2024-09-09 15:17:17','2024-09-10 22:43:20',1,1),(7,'Vendedor Horeca','Vendedor Horeca','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1),(8,'Vendedor Mayorista','Vendedor Mayorista','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1),(9,'Vendedor Mercados','Vendedor Mercados','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1),(10,'Coordinador','Coordinador','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1),(11,'Desarrollador de Canal','Desarrollador de Canal','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1),(12,'Jefe de Canal','Jefe de Canal','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1),(13,'KAM','KAM','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1),(14,'Mercaderista','Mercaderista','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1),(15,'Palanca','Palanca','2024-09-10 22:42:34','2024-09-10 22:42:34',1,1);
/*!40000 ALTER TABLE `roles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `userroles`
--

DROP TABLE IF EXISTS `userroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userroles` (
  `userRoleId` int NOT NULL AUTO_INCREMENT,
  `userId` int DEFAULT NULL,
  `roleId` int DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`userRoleId`),
  KEY `userId` (`userId`),
  KEY `roleId` (`roleId`),
  CONSTRAINT `userroles_ibfk_1` FOREIGN KEY (`userId`) REFERENCES `users` (`userId`),
  CONSTRAINT `userroles_ibfk_2` FOREIGN KEY (`roleId`) REFERENCES `roles` (`roleId`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `userroles`
--

LOCK TABLES `userroles` WRITE;
/*!40000 ALTER TABLE `userroles` DISABLE KEYS */;
INSERT INTO `userroles` (`userRoleId`, `userId`, `roleId`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,1,1,'2024-09-09 16:01:32','2024-09-09 16:01:32',1,1),(2,2,1,'2024-09-09 17:48:13','2024-09-09 17:48:13',NULL,NULL),(4,3,6,'2024-09-09 22:48:15','2024-09-09 22:48:15',NULL,NULL);
/*!40000 ALTER TABLE `userroles` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `userId` int NOT NULL AUTO_INCREMENT,
  `regionId` int DEFAULT NULL,
  `cediId` int DEFAULT NULL,
  `zoneId` int DEFAULT NULL,
  `route` int DEFAULT NULL,
  `code` int DEFAULT NULL,
  `paternalSurName` varchar(150) DEFAULT NULL,
  `maternalSurName` varchar(150) DEFAULT NULL,
  `names` varchar(150) DEFAULT NULL,
  `email` varchar(150) DEFAULT NULL,
  `phone` varchar(150) DEFAULT NULL,
  `isActive` tinyint(1) DEFAULT '1',
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`userId`),
  KEY `regionId` (`regionId`),
  KEY `routeId` (`route`),
  KEY `users__fk_cediId` (`cediId`),
  KEY `users_zone__fk` (`zoneId`),
  CONSTRAINT `users__fk_cediId` FOREIGN KEY (`cediId`) REFERENCES `cedis` (`cediId`),
  CONSTRAINT `users_ibfk_1` FOREIGN KEY (`regionId`) REFERENCES `regions` (`regionId`),
  CONSTRAINT `users_zone__fk` FOREIGN KEY (`zoneId`) REFERENCES `zone` (`zoneId`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` (`userId`, `regionId`, `cediId`, `zoneId`, `route`, `code`, `paternalSurName`, `maternalSurName`, `names`, `email`, `phone`, `isActive`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,1,1,NULL,NULL,NULL,'Abanto','Ramirez','Erik','erik.abanto@gmail.com','988366523',1,'2024-09-09 15:48:21','2024-09-09 22:40:54',1,2),(2,1,1,NULL,NULL,NULL,'Paredes','Rata','Cesar','cesargpq@gmail.com','912212122',1,'2024-09-09 22:40:40','2024-09-09 22:40:40',1,1),(3,1,1,1,10001,12212,'Martinez','Sanchez','Maria',NULL,'922121223',1,'2024-09-10 03:48:14','2024-09-10 03:48:14',1,1);
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `zone`
--

DROP TABLE IF EXISTS `zone`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `zone` (
  `zoneId` int NOT NULL AUTO_INCREMENT,
  `cediId` int DEFAULT NULL,
  `zoneCode` int DEFAULT NULL,
  `zoneName` varchar(100) DEFAULT NULL,
  `createdAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `updatedAt` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `createdBy` int DEFAULT NULL,
  `updatedBy` int DEFAULT NULL,
  PRIMARY KEY (`zoneId`),
  KEY `zone_cedi__fk_cediId` (`cediId`),
  CONSTRAINT `zone_cedi__fk_cediId` FOREIGN KEY (`cediId`) REFERENCES `cedis` (`cediId`)
) ENGINE=InnoDB AUTO_INCREMENT=186 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `zone`
--

LOCK TABLES `zone` WRITE;
/*!40000 ALTER TABLE `zone` DISABLE KEYS */;
INSERT INTO `zone` (`zoneId`, `cediId`, `zoneCode`, `zoneName`, `createdAt`, `updatedAt`, `createdBy`, `updatedBy`) VALUES (1,4,1680,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(2,4,1681,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(3,4,1682,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(4,4,1686,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(5,4,3681,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(6,5,1770,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(7,5,1771,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(8,5,3710,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(9,6,1120,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(10,6,1121,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(11,6,1122,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(12,6,1123,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(13,6,1124,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(14,6,1125,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(15,6,1126,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(16,7,1080,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(17,7,1081,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(18,7,1082,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(19,7,1083,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(20,7,1084,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(21,7,1085,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(22,7,1086,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(23,7,1087,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(24,7,3080,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(25,8,1520,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(26,8,3520,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(27,9,1790,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(28,9,3715,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(29,11,1690,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(30,11,3692,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(31,12,1840,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(32,12,1841,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(33,12,3040,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(34,12,6850,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(35,12,8850,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(36,13,1040,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(37,13,1691,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(38,13,3691,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(39,13,6040,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(40,13,6050,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(41,14,1720,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(42,14,1721,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(43,14,1722,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(44,14,1723,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(45,14,1724,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(46,14,1725,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(47,15,1000,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(48,15,1001,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(49,15,1002,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(50,15,1003,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(51,15,1004,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(52,15,1005,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(53,15,1006,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(54,15,1008,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(55,15,3030,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(56,17,1160,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(57,17,1161,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(58,17,1162,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(59,17,1163,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(60,17,3160,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(61,17,6160,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(62,17,8160,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(63,18,1200,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(64,18,1201,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(65,18,1202,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(66,18,3200,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(67,18,5200,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(68,18,6200,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(69,18,8200,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(70,19,1240,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(71,19,1241,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(72,19,1242,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(73,19,3240,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(74,19,5240,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(75,19,6240,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(76,19,8240,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(77,20,1280,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(78,20,1281,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(79,20,3280,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(80,20,6280,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(81,20,8280,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(82,23,1700,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(83,23,1701,'','2024-09-11 00:07:08','2024-09-11 00:07:08',1,1),(84,23,1702,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(85,23,1703,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(86,23,3590,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(87,23,6690,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(88,23,8690,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(89,23,9590,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(90,24,1920,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(91,24,1921,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(92,24,3830,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(93,24,6680,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(94,24,8680,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(95,25,1940,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(96,25,1941,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(97,25,1942,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(98,25,1944,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(99,25,3851,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(100,26,1500,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(101,26,3500,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(102,27,1970,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(103,27,1971,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(104,28,1975,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(105,28,3886,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(106,28,6701,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(107,29,1320,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(108,29,1321,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(109,29,1322,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(110,29,3320,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(111,29,6320,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(112,29,8320,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(113,30,1590,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(114,30,3599,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(115,31,1360,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(116,31,1361,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(117,31,1362,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(118,31,3360,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(119,31,6360,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(120,31,8360,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(121,32,1400,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(122,32,1401,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(123,32,3400,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(124,32,6400,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(125,32,8400,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(126,34,1760,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(127,34,3760,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(128,35,1440,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(129,35,1441,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(130,35,1442,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(131,35,1443,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(132,35,1444,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(133,35,3440,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(134,35,6440,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(135,35,8440,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(136,36,1480,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(137,36,6480,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(138,37,1490,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(139,37,1491,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(140,38,1957,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(141,39,1955,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(142,40,1779,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(143,40,3370,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(144,41,1980,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(145,41,3300,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(146,42,1800,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(147,43,1950,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(148,43,3870,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(149,44,1710,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(150,44,3701,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(151,45,1900,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(152,46,5600,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(153,46,5601,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(154,46,5602,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(155,46,5603,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(156,47,5610,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(157,47,5611,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(158,47,5612,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(159,47,5613,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(160,47,5614,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(161,48,6600,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(162,48,6601,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(163,48,6720,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(164,48,6820,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(165,48,6840,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(166,49,7600,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(167,49,7601,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(168,50,5600,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(169,50,5604,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(170,50,5605,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(171,50,5606,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(172,50,5607,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(173,50,5608,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(174,50,5609,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(175,50,7601,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(176,51,5603,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(177,51,5617,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(178,51,5618,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(179,53,3601,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(180,55,3600,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(181,55,3602,'','2024-09-11 00:07:09','2024-09-11 00:07:09',1,1),(182,56,8060,'','2024-09-11 00:07:10','2024-09-11 00:07:10',1,1),(183,56,8720,'','2024-09-11 00:07:10','2024-09-11 00:07:10',1,1),(184,56,8820,'','2024-09-11 00:07:10','2024-09-11 00:07:10',1,1),(185,56,8840,'','2024-09-11 00:07:10','2024-09-11 00:07:10',1,1);
/*!40000 ALTER TABLE `zone` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'assetmanagement'
--

--
-- Dumping routines for database 'assetmanagement'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-09-13  0:02:47
