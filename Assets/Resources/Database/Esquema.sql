BEGIN TRANSACTION;

CREATE TABLE "TORNEO" (
    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
    `nombre` TEXT,
    `imagen` TEXT,
    `rondas` INTEGER,
    `ponderacion` INTEGER,
    `puntuacion_inicial` INTEGER
);

CREATE TABLE "EQUIPO" (
    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
    `nombre` TEXT,
    `imagen` TEXT, 
    `torneo_id` INTEGER NOT NULL,

    FOREIGN KEY (`torneo_id`) REFERENCES `TORNEO`(`id`) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE "CATEGORIA" (
    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
    `nombre` TEXT,
    `imagen` TEXT,
    `color` INTEGER,
    `torneo_id` INTEGER NOT NULL,

    FOREIGN KEY (`torneo_id`) REFERENCES `TORNEO`(`id`) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE TABLE "PREGUNTA" (
    `id` INTEGER PRIMARY KEY AUTOINCREMENT,
    `encabezado` TEXT,
    `tipo_pregunta` INTEGER,
    `multimedia` TEXT,
    `respuesta_a` TEXT,
    `respuesta_b` TEXT,
    `respuesta_c` TEXT,
    `respuesta_d` TEXT,
    `respuesta_correcta` INTEGER,
    `aprendizaje` TEXT,    
    `categoria_id` INTEGER NOT NULL,
    `torneo_id` INTEGER NOT NULL,

    FOREIGN KEY (`categoria_id`) REFERENCES `CATEGORIA`(`id`) ON UPDATE CASCADE ON DELETE CASCADE,    
    FOREIGN KEY (`torneo_id`) REFERENCES `TORNEO`(`id`) ON UPDATE CASCADE ON DELETE CASCADE
);

COMMIT;