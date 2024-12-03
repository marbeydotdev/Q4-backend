CREATE TABLE IF NOT EXISTS `maintenance_plans`
(
    `id`               int                                 NOT NULL PRIMARY KEY AUTO_INCREMENT,
    `planned_date`     datetime                            NOT NULL,
    `mold_id`          int                                 NOT NULL,
    `maintenance_type` ENUM ('Preventative', 'Corrective') NOT NULL,
    `description`      text,
    `assigned_to`      int                                 NOT NULL,
    `status`           ENUM ('Planned', 'Busy', 'Finished') DEFAULT 'Planned'
);

CREATE TABLE IF NOT EXISTS `mechanics`
(
    `id`             int  NOT NULL PRIMARY KEY AUTO_INCREMENT,
    `name`           text NOT NULL,
    `specialization` text NOT NULL
);

REPLACE INTO mechanics (id, name, specialization)
VALUES (0, 'Jan Hendriks', 'Hybride');

REPLACE INTO mechanics (id, name, specialization)
VALUES (1, 'Luc Lammers', 'Polijster');

REPLACE INTO mechanics (id, name, specialization)
VALUES (2, 'Peter van Berg', 'Elektrisch');

REPLACE INTO mechanics (id, name, specialization)
VALUES (3, 'Pedro Vissers', 'Elektrisch');

ALTER TABLE `maintenance_plans`
    ADD FOREIGN KEY (`id`) REFERENCES `mechanics` (`id`);
