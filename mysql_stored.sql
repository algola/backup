-- --------------------------------------------------------
-- Host:                         192.168.0.99
-- Server version:               5.5.29 - MySQL Community Server (GPL)
-- Server OS:                    osx10.6
-- HeidiSQL version:             7.0.0.4053
-- Date/time:                    2013-05-13 14:20:20
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET FOREIGN_KEY_CHECKS=0 */;

-- Dumping structure for procedure db.AddColumnToTable
DROP PROCEDURE IF EXISTS `AddColumnToTable`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddColumnToTable`(IN `dbName` TEXT, IN `tableName` TEXT, IN `columnName` TEXT, IN `typeName` TEXT, IN `typeDimension` INT)
BEGIN
  
SET @typeMysql = 'INT';

IF typeName = 'time' THEN
  SET @typeMysql = CONCAT('TIME');
END IF;

IF typeName = 'intPK' THEN
  SET @typeMysql = CONCAT('INT(10) UNSIGNED NOT NULL AUTO_INCREMENT, ADD PRIMARY KEY (',columnName,') ');
END IF;

IF typeName = 'intUS' THEN
  SET @typeMysql = CONCAT('INT(10) UNSIGNED');
END IF;

IF typeName = 'bool' THEN
  SET @typeMysql = 'BIT(1)';
END IF;

IF typeName = 'int' THEN
  SET @typeMysql = 'INT';
END IF;

IF typeName = 'byteUS' THEN
  SET @typeMysql = CONCAT('TINYINT UNSIGNED');
END IF;

IF typeName = 'byte' THEN
  SET @typeMysql = 'TINYINT';
END IF;
 
IF typeName = 'string' THEN
  SET @typeMysql = CONCAT('CHAR(',typeDimension,') NULL ');
END IF;

IF typeName = 'stringPK' THEN
  SET @typeMysql =  CONCAT('CHAR(',typeDimension,'), ADD PRIMARY KEY (',columnName,')');
END IF;

IF typeName = 'memo' THEN
  SET @typeMysql = 'TEXT NULL ';
END IF;

IF typeName = 'double' THEN
  SET @typeMysql = CONCAT('DOUBLE ');
END IF;

SET @sql = CONCAT('ALTER TABLE ', dbName , '.', tableName , ' ADD ', columnName ,'  ', @typeMysql , ';');
 PREPARE s1 from @sql;
 EXECUTE s1;
    
END//
DELIMITER ;


-- Dumping structure for procedure db.AddDatabase
DROP PROCEDURE IF EXISTS `AddDatabase`;
DELIMITER //
CREATE DEFINER=`root`@`%` PROCEDURE `AddDatabase`(IN `name` TEXT)
BEGIN
  
SET @sql = CONCAT('CREATE DATABASE IF NOT EXISTS ', name ,';');

    PREPARE s1 from @sql;
    EXECUTE s1;
    
END//
DELIMITER ;


-- Dumping structure for procedure db.AddForeignKey
DROP PROCEDURE IF EXISTS `AddForeignKey`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddForeignKey`(IN `dbName` TEXT, IN `tableName` TEXT, IN `columnName` TEXT, IN `tableName2` TEXT, IN `columnName2` TEXT)
BEGIN

SET @sql = CONCAT('ALTER TABLE ', 
							dbName , '.', 
							tableName , 
							' ADD CONSTRAINT FK_',tableName, '_', columnName,' FOREIGN KEY (',
							columnName,
							') REFERENCES ', 
							dbName , '.',
							tableName2, 							
							' (',columnName2,
							')  ON UPDATE CASCADE ON DELETE CASCADE;');
    PREPARE s1 from @sql;
    EXECUTE s1;
    
END//
DELIMITER ;


-- Dumping structure for procedure db.AddIndex
DROP PROCEDURE IF EXISTS `AddIndex`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddIndex`(IN `TableName` TEXT, IN `ColumnName` TEXT)
BEGIN

SET @sql = CONCAT('ALTER TABLE ', tableName , ' ADD INDEX ' , columnName , '(', columnName,');'); 
    PREPARE s1 from @sql;
    EXECUTE s1;    
END//
DELIMITER ;


-- Dumping structure for procedure db.AddTable
DROP PROCEDURE IF EXISTS `AddTable`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddTable`(IN `dbName` TEXT, IN `name` TEXT)
BEGIN

SET @sql = CONCAT('CREATE TABLE IF NOT EXISTS ', dbName , '.', name , ' (`TimeStampTable` TIMESTAMP) ENGINE=InnoDB DEFAULT CHARSET=latin1;'); 
    PREPARE s1 from @sql;
    EXECUTE s1;
END//
DELIMITER ;


-- Dumping structure for procedure db.ChangeStringColumnLegth
DROP PROCEDURE IF EXISTS `ChangeStringColumnLegth`;
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `ChangeStringColumnLegth`(IN `dbName` TEXT, IN `tableName` TEXT, IN `columnName` TEXT, IN `columnLenght` INT)
BEGIN

SET @sql = CONCAT('ALTER TABLE ', dbName , '.', tableName , ' CHANGE COLUMN ', columnName ,' ', columnName, ' CHAR(', columnLenght, ');');
 PREPARE s1 from @sql;
 EXECUTE s1;

END//
DELIMITER ;
/*!40014 SET FOREIGN_KEY_CHECKS=1 */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
