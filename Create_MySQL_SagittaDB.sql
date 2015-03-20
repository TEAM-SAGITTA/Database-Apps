CREATE SCHEMA `sagittadb_mysql` DEFAULT CHARACTER SET utf8 COLLATE utf8_unicode_ci ;

CREATE TABLE `sagittadb_mysql`.`vendors` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `Name` VARCHAR(200)  NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC));
  
  
  CREATE TABLE `sagittadb_mysql`.`products` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(500) NOT NULL,
   `vendor_id` INT NOT NULL,
  `Sum_of_sales` DECIMAL NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC));
  
  CREATE TABLE `sagittadb_mysql`.`expenses` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `date` DATE NULL,
  `vendor_id` INT NOT NULL,
  `sum` DECIMAL NULL,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `id_UNIQUE` (`id` ASC));
  
  ALTER TABLE `sagittadb_mysql`.`expenses` 
ADD INDEX `FK_expenses_vendors_idx` (`vendor_id` ASC);
ALTER TABLE `sagittadb_mysql`.`expenses` 
ADD CONSTRAINT `FK_expenses_vendors`
  FOREIGN KEY (`vendor_id`)
  REFERENCES `sagittadb_mysql`.`vendors` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
  
  
  ALTER TABLE `sagittadb_mysql`.`products` 
ADD INDEX `FK_products_vendors_idx` (`vendor_Id` ASC);
ALTER TABLE `sagittadb_mysql`.`products` 
ADD CONSTRAINT `FK_products_vendors`
  FOREIGN KEY (`vendor_Id`)
  REFERENCES `sagittadb_mysql`.`vendors` (`id`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;
