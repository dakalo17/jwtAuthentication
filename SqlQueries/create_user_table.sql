﻿CREATE TABLE "user" (
  id SERIAL PRIMARY KEY,
  email VARCHAR(255) NOT NULL unique,
  password VARCHAR(255) NOT NULL
);
