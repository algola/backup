-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Server version:               5.5.21 - MySQL Community Server (GPL)
-- Server OS:                    Win32
-- HeidiSQL version:             7.0.0.4053
-- Date/time:                    2012-03-03 23:42:43
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!40014 SET FOREIGN_KEY_CHECKS=0 */;

-- Dumping structure for procedure db.AddColumnToTable
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddColumnToTable`(IN `tableName` TEXT, IN `columnName` TEXT, IN `typeName` TEXT, IN `typeDimension` INT)
BEGIN
  
SET @typeMysql = 'INT';

IF typeName = 'intPK' THEN
  SET @typeMysql = CONCAT('INT(10) UNSIGNED NOT NULL AUTO_INCREMENT, ADD PRIMARY KEY (',columnName,') ');
END IF;

IF typeName = 'intUS' THEN
  SET @typeMysql = CONCAT('INT(10) UNSIGNED NOT NULL ');
END IF;

IF typeName = 'int' THEN
  SET @typeMysql = 'INT';
END IF;
 
IF typeName = 'string' THEN
  SET @typeMysql = CONCAT('CHAR(',typeDimension,') NULL ');
END IF;

IF typeName = 'memo' THEN
  SET @typeMysql = 'TEXT NULL ';
END IF;

IF typeName = 'double' THEN
  SET @typeMysql = CONCAT('DOUBLE ');
END IF;

 
SET @sql = CONCAT('ALTER TABLE ', tableName , ' ADD ', columnName ,'  ', @typeMysql , ';');
 PREPARE s1 from @sql;
 EXECUTE s1;
    
END//
DELIMITER ;


-- Dumping structure for procedure db.AddForeignKey
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddForeignKey`(IN `tableName` TEXT, IN `columnName` TEXT, IN `tableName2` TEXT, IN `columnName2` TEXT)
BEGIN

SET @sql = CONCAT('ALTER TABLE ', 
							tableName , 
							' ADD CONSTRAINT FK_',tableName, '_', columnName,' FOREIGN KEY (',
							columnName,
							') REFERENCES ', 
							tableName2, 
							' (',columnName2,
							');');
    PREPARE s1 from @sql;
    EXECUTE s1;
    
END//
DELIMITER ;


-- Dumping structure for procedure db.AddIndex
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddIndex`(IN `TableName` TEXT, IN `ColumnName` TEXT)
BEGIN

SET @sql = CONCAT('ALTER TABLE ', tableName , ' ADD INDEX ' , columnName , '(', columnName,');'); 
    PREPARE s1 from @sql;
    EXECUTE s1;    
END//
DELIMITER ;


-- Dumping structure for procedure db.AddTable
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `AddTable`(IN `name` TEXT)
BEGIN

SET @sql = CONCAT('CREATE TABLE IF NOT EXISTS ', name , ' (`TimeStampTable` TIMESTAMP) ENGINE=InnoDB DEFAULT CHARSET=latin1;'); 
    PREPARE s1 from @sql;
    EXECUTE s1;
END//
DELIMITER ;


-- Dumping structure for procedure db.ChangeStringColumnLegth
DELIMITER //
CREATE DEFINER=`root`@`localhost` PROCEDURE `ChangeStringColumnLegth`(IN `tableName` TEXT, IN `columnName` TEXT, IN `columnLenght` INT)
BEGIN

SET @sql = CONCAT('ALTER TABLE ', tableName , ' CHANGE COLUMN ', columnName ,' ', columnName, ' CHAR(', columnLenght, ');');
 PREPARE s1 from @sql;
 EXECUTE s1;

END//
DELIMITER ;
/*!40014 SET FOREIGN_KEY_CHECKS=1 */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
