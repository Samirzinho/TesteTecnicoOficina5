CREATE TABLE veiculos (
  placa varchar(8) NOT NULL,
  marca varchar(20) DEFAULT NULL,
  ano int(11) DEFAULT NULL,
  descricao text,
  vendido tinyint(1) DEFAULT NULL,
  created datetime DEFAULT NULL,
  updated datetime DEFAULT NULL,
  PRIMARY KEY (placa)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;